using Forging.Core.Dtos;
using Forging.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Forging.Core.UseCases.UserUseCases;

namespace Forging.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly GetAllUsersUseCase _getAllUsersUseCase;
        private readonly GetUserByIdUseCase _getUserByIdUseCase;
        private readonly CreateUserUseCase _createUserUseCase;
        private readonly UpdateUserUseCase _updateUserUseCase;
        private readonly DeleteUserUseCase _deleteUserUseCase;

        public UserController(
            GetAllUsersUseCase getAllUsersUseCase,
            GetUserByIdUseCase getUserByIdUseCase,
            CreateUserUseCase createUserUseCase,
            UpdateUserUseCase updateUserUseCase,
            DeleteUserUseCase deleteUserUseCase
        )
        {
            _getAllUsersUseCase = getAllUsersUseCase;
            _getUserByIdUseCase = getUserByIdUseCase;
            _createUserUseCase = createUserUseCase;
            _updateUserUseCase = updateUserUseCase;
            _deleteUserUseCase = deleteUserUseCase;
        }

        [HttpGet("/users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _getAllUsersUseCase.ExecuteAsync();
            return Ok(users);
        }

        [HttpGet("/users/{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _getUserByIdUseCase.ExecuteAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("/users")]
        public async Task<ActionResult<User>> CreateUser(CreateUserDto createUserDto)
        {
            var newUser = new User
            {
                Id = createUserDto.Id,
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                PhoneNumber = createUserDto.PhoneNumber,
                Roles = createUserDto.Roles,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                ImageUrl = createUserDto.ImageUrl,
                JoinedAt = DateTime.UtcNow
            };

            var result = await _createUserUseCase.ExecuteAsync(newUser);
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
            }
            else
            {
                return BadRequest("Error at inserting user");
            }
        }

        [HttpPut("/users/{id}")]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserDto updateUserDto)
        {
            var existingUser = await _getUserByIdUseCase.ExecuteAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Username = updateUserDto.Username;
            existingUser.Email = updateUserDto.Email;
            existingUser.FirstName = updateUserDto.FirstName;
            existingUser.LastName = updateUserDto.LastName;
            existingUser.PhoneNumber = updateUserDto.PhoneNumber;
            existingUser.ImageUrl = updateUserDto.ImageUrl;
            existingUser.Roles = updateUserDto.Roles;

            var result = await _updateUserUseCase.ExecuteAsync(id, existingUser);
            if (result > 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Error at updating user");
            }
        }

        [HttpDelete("/users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _deleteUserUseCase.ExecuteAsync(id);
            if (result > 0)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
