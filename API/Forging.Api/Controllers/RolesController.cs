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

            var roleInsertSql = @"INSERT INTO roles (id, role_name) VALUES (@Id, @Name)";
            var result = await connection.ExecuteAsync(roleInsertSql, newRole);

            await connection.CloseAsync();
            return CreatedAtAction(nameof(GetRoleById), new { id = newRole.Id }, newRole);
        }

        [HttpPut("/roles/{id}")]
        public async Task<ActionResult<Role>> UpdateRole(Guid id, RoleDto updateRoleDto)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            var updateRoleSql = @"UPDATE roles SET role_name = @Name WHERE id = @Id";
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
    }
}
