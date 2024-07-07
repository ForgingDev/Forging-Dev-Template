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
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = createUserDto.Username,
                Description = createUserDto.Description,
                Email = createUserDto.Email,
                JoinedAt = DateTime.UtcNow
            };

            var result = await _connection.ExecuteAsync(
                @"INSERT INTO users (id, username, description, email) 
              VALUES (@Id, @Username, @Description, @Email)",
                newUser
            );

            if (result > 0)
            {
                return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
            }
            return BadRequest();
        }

        [HttpPut("/users/{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateUserDto updateUserDto)
        {
            var result = await _connection.ExecuteAsync(
                @"UPDATE users 
              SET username = @Username, description = @Description, email = @Email 
              WHERE id = @Id",
                new
                {
                    Id = id,
                    updateUserDto.Username,
                    updateUserDto.Description,
                    updateUserDto.Email
                }
            );
            if (result > 0)
            {
                return NoContent();
            }
            return NotFound();
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
