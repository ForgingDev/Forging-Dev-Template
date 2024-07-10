using Forging.Api.Dtos;
using Forging.Api.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Forging.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : Controller
    {
        private readonly IConfiguration _configuration;

        public BaseController(IConfiguration configuration)
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

        [HttpGet("/users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            await using var connection = GetConnection();

            await connection.OpenAsync();

            var users = await connection.QueryAsync<User>("SELECT * FROM users");

            await connection.CloseAsync();
            return Ok(users);
        }

        [HttpGet("/users/{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            await using var connection = GetConnection();

            await connection.OpenAsync();

            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM users WHERE id = @Id",
                new { Id = id }
            );

            await connection.CloseAsync();

            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost("/users")]
        public async Task<ActionResult<User>> CreateUser(CreateUserDto createUserDto)
        {
            using var connection = GetConnection();

            await connection.OpenAsync();

            var usersSql =
                @"INSERT INTO users (id, username, description, email, first_name, last_name, phone_number, image_url) 
              VALUES (@Id, @Username, @Description, @Email, @FirstName, @LastName, @PhoneNumber, @ImageUrl)";

            var newUser = new User
            {
                Id = createUserDto.Id,
                Username = createUserDto.Username,
                Description = createUserDto.Description,
                Email = createUserDto.Email,
                PhoneNumber = createUserDto.PhoneNumber,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                ImageUrl = createUserDto.ImageUrl,
                JoinedAt = DateTime.UtcNow
            };

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var result = await connection.ExecuteAsync(usersSql, newUser, transaction);
                    if (result > 0)
                    {
                        foreach (var email in newUser.Email)
                        {
                            var emailSql =
                                @"INSERT INTO user_emails (id, user_id, email) VALUES (@EmailId, @UserId, @Email)";
                            var insertEmailResult = await connection.ExecuteAsync(
                                emailSql,
                                new
                                {
                                    EmailId = Guid.NewGuid(),
                                    UserId = newUser.Id,
                                    Email = email
                                },
                                transaction
                            );

                            if (insertEmailResult <= 0)
                            {
                                transaction.Rollback();
                                return BadRequest("Failed to insert provided email address(es)");
                            }
                        }

                        foreach (var phoneNumber in newUser.PhoneNumber)
                        {
                            var phoneNrSql =
                                @"INSERT INTO user_phone_numbers (id, user_id, phone_number) VALUES (@PhoneId, @UserId, @PhoneNumber)";
                            var phoneNumberResult = await connection.ExecuteAsync(
                                phoneNrSql,
                                new
                                {
                                    PhoneId = Guid.NewGuid(),
                                    UserId = newUser.Id,
                                    PhoneNumber = phoneNumber
                                },
                                transaction
                            );

                            if (phoneNumberResult <= 0)
                            {
                                transaction.Rollback();
                                return BadRequest("Failed to insert provided phone number(s)");
                            }
                        }

                        transaction.Commit();
                        await connection.CloseAsync();
                        return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
                    }
                    else
                    {
                        transaction.Rollback();
                        return BadRequest("Error at inserting user");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, "Internal Server Error: " + ex.Message);
                }
            }
        }

        [HttpPut("/users/{id}")]
        public async Task<ActionResult<User>> UpdateUser(Guid id, UpdateUserDto updateUserDto)
        {
            using var connection = GetConnection();

            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var usersSql =
                        @"UPDATE users 
                      SET username = @Username, description = @Description, email = @Email, first_name = @FirstName, 
                      last_name = @LastName, phone_number = @PhoneNumber, image_url = @ImageUrl 
                      WHERE id = @Id";

                    var result = await connection.ExecuteAsync(
                        usersSql,
                        new
                        {
                            Id = id,
                            updateUserDto.Username,
                            updateUserDto.Description,
                            updateUserDto.Email,
                            updateUserDto.FirstName,
                            updateUserDto.LastName,
                            updateUserDto.PhoneNumber,
                            updateUserDto.ImageUrl,
                        },
                        transaction
                    );
                    if (result > 0)
                    {
                        var deleteEmailSql = @"DELETE FROM user_emails WHERE user_id = @UserId";
                        var deletePhoneNrSql =
                            @"DELETE FROM user_phone_numbers WHERE user_id = @UserId";

                        await connection.ExecuteAsync(
                            deleteEmailSql,
                            new { UserId = id },
                            transaction
                        );
                        await connection.ExecuteAsync(
                            deletePhoneNrSql,
                            new { UserId = id },
                            transaction
                        );

                        foreach (var email in updateUserDto.Email)
                        {
                            var insertEmailSql =
                                @"INSERT INTO user_emails (id, user_id, email) VALUES (@EmailId, @UserId, @Email)";
                            var EmailResponse = connection.ExecuteAsync(
                                insertEmailSql,
                                new
                                {
                                    EmailId = Guid.NewGuid(),
                                    UserId = id,
                                    Email = email
                                },
                                transaction
                            );

                            if (await EmailResponse <= 0)
                            {
                                transaction.Rollback();
                                return BadRequest("Failed to update email adress(es)");
                            }
                        }

                        foreach (var phoneNumber in updateUserDto.PhoneNumber)
                        {
                            var insertPhoneNrSql =
                                @"INSERT INTO user_phone_numbers (id, user_id, phone_number) VALUES (@PhoneId, @UserId, @PhoneNumber)";
                            var PhoneNumberResponse = connection.ExecuteAsync(
                                insertPhoneNrSql,
                                new
                                {
                                    PhoneId = Guid.NewGuid(),
                                    UserId = id,
                                    PhoneNumber = phoneNumber
                                },
                                transaction
                            );

                            if (await PhoneNumberResponse <= 0)
                            {
                                transaction.Rollback();
                                return BadRequest("Failed to update phone number(s)");
                            }
                        }
                        transaction.Commit();
                        await connection.CloseAsync();
                        return NoContent();
                    }
                    else
                    {
                        transaction.Rollback();
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
        }

        [HttpDelete("/users/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            using var connection = GetConnection();

            await connection.OpenAsync();

            var result = await connection.ExecuteAsync(
                "DELETE FROM users WHERE id = @Id",
                new { Id = id }
            );

            await connection.CloseAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
