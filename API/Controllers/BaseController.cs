using API.Dtos;
using API.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : Controller
    {
        private readonly NpgsqlConnection _connection;

        public BaseController(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        [HttpGet("/users")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _connection.QueryAsync<User>("SELECT * FROM users");
            return users;
        }

        [HttpGet("/users/{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM users WHERE id = @Id",
                new { Id = id }
            );
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost("/users")]
        public async Task<ActionResult<User>> PostUser(CreateUserDto createUserDto)
        {
            var sql =
                @"INSERT INTO users (id, username, description, email, first_name, last_name, phone_number, image_url) 
              VALUES (@Id, @Username, @Description, @Email, @FirstName, @LastName, @PhoneNumber, @ImageUrl)";

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = createUserDto.Username,
                Description = createUserDto.Description,
                Email = createUserDto.Email,
                PhoneNumber = createUserDto.PhoneNumber,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                ImageUrl = createUserDto.ImageUrl,
                JoinedAt = DateTime.UtcNow
            };

            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    var result = await _connection.ExecuteAsync(sql, newUser, transaction);
                    if (result > 0)
                    {
                        foreach (var email in newUser.Email)
                        {
                            var emailSql =
                                @"INSERT INTO user_emails (id, user_id, email) VALUES (@EmailId, @UserId, @Email)";
                            var insertEmailResult = await _connection.ExecuteAsync(
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
                            var phoneNumberResult = await _connection.ExecuteAsync(
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
        public async Task<IActionResult> Put(Guid id, UpdateUserDto updateUserDto)
        {
            using (var transaction = _connection.BeginTransaction())
            {
                var sql =
                    @"UPDATE users 
              SET username = @Username, description = @Description, email = @Email, first_name = @FirstName, 
              last_name = @LastName, phone_number = @PhoneNumber, image_url = @ImageUrl 
              WHERE id = @Id";

                var result = await _connection.ExecuteAsync(
                    sql,
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
                    return NoContent();
                }
                return NotFound();
            }
        }

        [HttpDelete("/users/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _connection.ExecuteAsync(
                "DELETE FROM users WHERE id = @Id",
                new { Id = id }
            );
            if (result > 0)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
