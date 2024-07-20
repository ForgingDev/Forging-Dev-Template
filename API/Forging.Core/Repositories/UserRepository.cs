using Dapper;
using Forging.Core.Dtos;
using Forging.Core.Interfaces.IRepositories;
using Forging.Domain.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Forging.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
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

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();
            var users = await connection.QueryAsync<User>("SELECT * FROM users");
            await connection.CloseAsync();
            return users;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM users WHERE id = @Id",
                new { Id = id }
            );
            await connection.CloseAsync();
            return user!;
        }

        public async Task<int> CreateUserAsync(User createUserDto)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            var usersSql =
                @"INSERT INTO users (id, username, email, first_name, last_name, phone_number, image_url, roles) 
                VALUES (@Id, @Username, @Email, @FirstName, @LastName, @PhoneNumber, @ImageUrl, @Roles)";

            using var transaction = connection.BeginTransaction();
            try
            {
                var result = await connection.ExecuteAsync(usersSql, createUserDto, transaction);
                if (result > 0)
                {
                    foreach (var email in createUserDto.Email)
                    {
                        var emailSql =
                            @"INSERT INTO user_emails (id, user_id, email) VALUES (@EmailId, @UserId, @Email)";
                        await connection.ExecuteAsync(
                            emailSql,
                            new
                            {
                                EmailId = Guid.NewGuid(),
                                UserId = createUserDto.Id,
                                Email = email
                            },
                            transaction
                        );
                    }

                    foreach (var phoneNumber in createUserDto.PhoneNumber)
                    {
                        var phoneSql =
                            @"INSERT INTO user_phone_numbers (id, user_id, phone_number) VALUES (@PhoneId, @UserId, @PhoneNumber)";
                        await connection.ExecuteAsync(
                            phoneSql,
                            new
                            {
                                PhoneId = Guid.NewGuid(),
                                UserId = createUserDto.Id,
                                PhoneNumber = phoneNumber
                            },
                            transaction
                        );
                    }

                    foreach (var role in createUserDto.Roles)
                    {
                        var roleSql =
                            @"INSERT INTO user_roles (user_id, role) VALUES (@UserId, @Role)";
                        await connection.ExecuteAsync(
                            roleSql,
                            new { UserId = createUserDto.Id, Role = role },
                            transaction
                        );
                    }
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
                await connection.CloseAsync();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                await connection.CloseAsync();
                throw;
            }
        }

        public async Task<int> UpdateUserAsync(string id, User updateUserDto)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            var usersSql =
                @"UPDATE users 
                SET username = @Username, email = @Email, phone_number = @PhoneNumber, first_name = @FirstName, last_name = @LastName, image_url = @ImageUrl 
                WHERE id = @Id";

            using var transaction = connection.BeginTransaction();
            try
            {
                var result = await connection.ExecuteAsync(usersSql, updateUserDto, transaction);
                if (result > 0)
                {
                    var deleteEmailSql = @"DELETE FROM user_emails WHERE user_id = @UserId";
                    var deletePhoneSql = @"DELETE FROM user_phone_numbers WHERE user_id = @UserId";
                    var deleteRoleSql = @"DELETE FROM user_roles WHERE user_id = @UserId";

                    await connection.ExecuteAsync(deleteEmailSql, new { UserId = id }, transaction);
                    await connection.ExecuteAsync(deletePhoneSql, new { UserId = id }, transaction);
                    await connection.ExecuteAsync(deleteRoleSql, new { UserId = id }, transaction);

                    foreach (var email in updateUserDto.Email)
                    {
                        var emailSql =
                            @"INSERT INTO user_emails (id, user_id, email) VALUES (@EmailId, @UserId, @Email)";
                        await connection.ExecuteAsync(
                            emailSql,
                            new
                            {
                                EmailId = Guid.NewGuid(),
                                UserId = id,
                                Email = email
                            },
                            transaction
                        );
                    }

                    foreach (var phoneNumber in updateUserDto.PhoneNumber)
                    {
                        var phoneSql =
                            @"INSERT INTO user_phone_numbers (id, user_id, phone_number) VALUES (@PhoneId, @UserId, @PhoneNumber)";
                        await connection.ExecuteAsync(
                            phoneSql,
                            new
                            {
                                PhoneId = Guid.NewGuid(),
                                UserId = id,
                                PhoneNumber = phoneNumber
                            },
                            transaction
                        );
                    }

                    foreach (var role in updateUserDto.Roles)
                    {
                        var roleSql =
                            @"INSERT INTO user_roles (user_id, role) VALUES (@UserId, @Role)";
                        await connection.ExecuteAsync(
                            roleSql,
                            new { UserId = id, Role = role },
                            transaction
                        );
                    }

                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
                await connection.CloseAsync();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                await connection.CloseAsync();
                throw;
            }
        }

        public async Task<int> DeleteUserAsync(string id)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();
            var result = await connection.ExecuteAsync(
                "DELETE FROM users WHERE id = @Id",
                new { Id = id }
            );
            await connection.CloseAsync();
            return result;
        }
    }
}
