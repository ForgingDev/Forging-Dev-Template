using Dapper;
using Dapper.Contrib.Extensions;
using Forging.Api.Dtos;
using Forging.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Forging.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public RolesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection GetConnection()
        {
            var connectionString =
                @$"Host={_configuration["DATABASE_HOST_SUPABASE"]};
                                    Port={_configuration["DATABASE_PORT_SUPABASE"]};
                                    Database={_configuration["DEFAULT_DATABASE_NAME"]};
                                    User Id={_configuration["DATABASE_USERNAME_SUPABASE"]};
                                    Password={_configuration["DATABASE_PASSWORD_SUPABASE"]};";
            return new NpgsqlConnection(connectionString);
        }

        [HttpGet("/roles")]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllRoles()
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            var roles = await connection.QueryAsync<Role>("SELECT * FROM roles");

            await connection.CloseAsync();
            return Ok(roles);
        }

        [HttpGet("/roles/{id}")]
        public async Task<ActionResult<Role>> GetRoleById(Guid id)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            var role = connection.Get<Role>(id);

            await connection.CloseAsync();
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [HttpGet("/roles/user={id}")]
        public async Task<ActionResult<IEnumerable<Role>>> GetUserRoles(string id)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            var userRolesSql =
                @"SELECT r.id AS Id, r.name AS Name
            FROM user_roles ur
            JOIN roles r ON ur.role_id = r.id
            WHERE ur.user_id = @UserId";

            var roles = await connection.QueryAsync<Role>(userRolesSql, new { UserId = id });

            await connection.CloseAsync();
            return Ok(roles);
        }

        [HttpPost("/roles")]
        public async Task<ActionResult<Role>> CreateRole([FromBody] RoleDto createRoleDto)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            var newRole = new Role { Id = Guid.NewGuid(), Name = createRoleDto.Name, };

            var roleInsertSql = @"INSERT INTO roles (id, name) VALUES (@Id, @Name)";
            var result = await connection.ExecuteAsync(roleInsertSql, newRole);

            await connection.CloseAsync();
            return CreatedAtAction(nameof(GetRoleById), new { id = newRole.Id }, newRole);
        }

        [HttpPost("/roles/user={id}/add-role")]
        public async Task<ActionResult<Role>> AddUserRole(string id, [FromBody] RoleDto addRoleDto)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            using (var transaction = connection.BeginTransaction())
                try
                {
                    var roleExistsSql = @"SELECT id FROM roles WHERE name = @Name";
                    var roleId = await connection.ExecuteScalarAsync<Guid?>(
                        roleExistsSql,
                        new { addRoleDto.Name },
                        transaction
                    );
                    if (roleId == null)
                    {
                        await transaction.RollbackAsync();
                        await connection.CloseAsync();
                        return BadRequest("Role does not exist.");
                    }

                    var addRoleSql =
                        @"UPDATE users 
                        SET roles = CASE 
                                        WHEN roles IS NULL OR roles = '' THEN @Name
                                        ELSE CONCAT(roles, ',', @Name) 
                                    END
                        WHERE id = @UserId AND (roles IS NULL OR roles NOT LIKE CONCAT('%,', @Name, ',%') 
                        AND roles NOT LIKE CONCAT(@Name, ',%') AND roles NOT LIKE CONCAT('%,', @Name))";
                    var affectedRows = await connection.ExecuteAsync(
                        addRoleSql,
                        new { addRoleDto.Name, UserId = id },
                        transaction
                    );

                    var insertUserRoles =
                        @"INSERT INTO user_roles (user_id, role_id, role)
                    VALUES (@UserId, @RoleId, @Name)";
                    await connection.ExecuteAsync(
                        insertUserRoles,
                        new
                        {
                            UserId = id,
                            RoleId = roleId,
                            addRoleDto.Name
                        },
                        transaction
                    );

                    await transaction.CommitAsync();
                    await connection.CloseAsync();
                    return Ok();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    await connection.CloseAsync();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
        }

        [HttpPut("/roles/{id}")]
        public async Task<ActionResult<Role>> UpdateRole(Guid id, RoleDto updateRoleDto)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            var updateRoleSql = @"UPDATE roles SET name = @Name WHERE id = @Id";
            var result = await connection.ExecuteAsync(
                updateRoleSql,
                new { Id = id, updateRoleDto.Name, }
            );

            await connection.CloseAsync();
            return NoContent();
        }

        [HttpDelete("/roles/{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            var roleDeleteSql = @"DELETE FROM roles WHERE id = @Id";
            var result = await connection.ExecuteAsync(roleDeleteSql, new { Id = id });

            await connection.CloseAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("/roles/user={id}/remove-role")]
        public async Task<ActionResult> DeleteRole(string id, [FromBody] RoleDto removeRoleDto)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();
            try
            {
                var getRoleIdSql = @"SELECT id FROM roles WHERE name = @Name";
                var roleId = await connection.ExecuteScalarAsync<Guid?>(
                    getRoleIdSql,
                    new { removeRoleDto.Name },
                    transaction
                );

                var removeRoleInUsersSql =
                    @"UPDATE users
                SET roles = TRIM(BOTH ',' FROM REGEXP_REPLACE(',' || roles || ',', '(,|^)' || @Name || '(,|$)', ',', 'g'))
                WHERE id = @UserId AND roles LIKE '%' || @Name || '%'";
                var affectedRows = await connection.ExecuteAsync(
                    removeRoleInUsersSql,
                    new { removeRoleDto.Name, UserId = id },
                    transaction
                );

                if (affectedRows == 0)
                {
                    await transaction.RollbackAsync();
                    await connection.CloseAsync();
                    return BadRequest("Role does not exist for the user or user not found.");
                }

                var removeRoleInUserRolesSql =
                    @"DELETE FROM user_roles
                    WHERE user_id = @UserId AND role_id = @RoleId";
                await connection.ExecuteAsync(
                    removeRoleInUserRolesSql,
                    new { UserId = id, RoleId = roleId, },
                    transaction
                );

                await transaction.CommitAsync();
                await connection.CloseAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await connection.CloseAsync();
                return StatusCode(500, "Internal server error" + ex.Message);
            }
        }
    }
}
