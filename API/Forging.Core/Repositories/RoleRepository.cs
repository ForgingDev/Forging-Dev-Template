using Dapper;
using Forging.Core.Interfaces.IRepositories;
using Microsoft.Extensions.Configuration;
using Forging.Domain.Models;
using Npgsql;

namespace Forging.Core.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;

        public RoleRepository(IConfiguration configuration)
        {
            _connectionString =
                @$"Host={configuration["DATABASE_HOST_SUPABASE"]};
                Port={configuration["DATABASE_PORT_SUPABASE"]};
                Database={configuration["DEFAULT_DATABASE_NAME"]};
                User Id={configuration["DATABASE_USERNAME_SUPABASE"]};
                Password={configuration["DATABASE_PASSWORD_SUPABASE"]};";
        }

        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();
            var roles = await connection.QueryAsync<Role>("SELECT * FROM roles");
            await connection.CloseAsync();
            return roles;
        }

        public async Task<Role> GetRoleByIdAsync(Guid id)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();
            var role = await connection.QueryFirstOrDefaultAsync<Role>(
                "SELECT * FROM roles WHERE id = @Id",
                new { Id = id }
            );
            await connection.CloseAsync();
            return role!;
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(string userId)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();
            var userRolesSql =
                @"SELECT r.id AS Id, r.name AS Name
                FROM user_roles ur
                JOIN roles r ON ur.role_id = r.id
                WHERE ur.user_id = @UserId";
            var roles = await connection.QueryAsync<Role>(userRolesSql, new { UserId = userId });
            await connection.CloseAsync();
            return roles;
        }

        public async Task<int> CreateRoleAsync(Role role)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();
            var roleInsertSql = @"INSERT INTO roles (id, name) VALUES (@Id, @Name)";
            var result = await connection.ExecuteAsync(roleInsertSql, role);
            await connection.CloseAsync();
            return result;
        }

        public async Task<int> UpdateRoleAsync(Guid id, Role role)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();
            var updateRoleSql = @"UPDATE roles SET name = @Name WHERE id = @Id";
            var result = await connection.ExecuteAsync(updateRoleSql, new { Id = id, role.Name });
            await connection.CloseAsync();
            return result;
        }

        public async Task<int> DeleteRoleAsync(Guid id)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();
            var roleDeleteSql = @"DELETE FROM roles WHERE id = @Id";
            var result = await connection.ExecuteAsync(roleDeleteSql, new { Id = id });
            await connection.CloseAsync();
            return result;
        }

        public async Task<int> AddUserRoleAsync(string userId, string roleName)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var roleExistsSql = @"SELECT id FROM roles WHERE name = @Name";
                    var roleId = await connection.ExecuteScalarAsync<Guid?>(
                        roleExistsSql,
                        new { Name = roleName },
                        transaction
                    );
                    if (roleId == null)
                    {
                        await transaction.RollbackAsync();
                        await connection.CloseAsync();
                        return 0;
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
                        new { Name = roleName, UserId = userId },
                        transaction
                    );

                    var insertUserRoles =
                        @"INSERT INTO user_roles (user_id, role_id, role)
                        VALUES (@UserId, @RoleId, @Name)";
                    await connection.ExecuteAsync(
                        insertUserRoles,
                        new
                        {
                            UserId = userId,
                            RoleId = roleId,
                            Name = roleName
                        },
                        transaction
                    );

                    await transaction.CommitAsync();
                    await connection.CloseAsync();
                    return affectedRows;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    await connection.CloseAsync();
                    throw;
                }
            }
        }

        public async Task<int> RemoveUserRoleAsync(string userId, string roleName)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();
            try
            {
                var getRoleIdSql = @"SELECT id FROM roles WHERE name = @Name";
                var roleId = await connection.ExecuteScalarAsync<Guid?>(
                    getRoleIdSql,
                    new { Name = roleName },
                    transaction
                );

                var removeRoleInUsersSql =
                    @"UPDATE users
                    SET roles = TRIM(BOTH ',' FROM REGEXP_REPLACE(',' || roles || ',', '(,|^)' || @Name || '(,|$)', ',', 'g'))
                    WHERE id = @UserId AND roles LIKE '%' || @Name || '%'";
                var affectedRows = await connection.ExecuteAsync(
                    removeRoleInUsersSql,
                    new { Name = roleName, UserId = userId },
                    transaction
                );

                if (affectedRows == 0)
                {
                    await transaction.RollbackAsync();
                    await connection.CloseAsync();
                    return 0;
                }

                var removeRoleInUserRolesSql =
                    @"DELETE FROM user_roles
                    WHERE user_id = @UserId AND role_id = @RoleId";
                await connection.ExecuteAsync(
                    removeRoleInUserRolesSql,
                    new { UserId = userId, RoleId = roleId },
                    transaction
                );

                await transaction.CommitAsync();
                await connection.CloseAsync();
                return affectedRows;
            }
            catch
            {
                await transaction.RollbackAsync();
                await connection.CloseAsync();
                throw;
            }
        }
    }
}
