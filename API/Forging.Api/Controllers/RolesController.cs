using Dapper;
using Dapper.Contrib.Extensions;
using Forging.Core.Dtos;
using Forging.Core.UseCases.RoleUseCases;
using Forging.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Forging.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly GetRolesUseCase _getRolesUseCase;
        private readonly GetRoleByIdUseCase _getRoleByIdUseCase;
        private readonly GetUserRolesUseCase _getUserRolesUseCase;
        private readonly CreateRoleUseCase _createRoleUseCase;
        private readonly AddUserRoleUseCase _addUserRoleUseCase;
        private readonly UpdateRoleUseCase _updateRoleUseCase;
        private readonly DeleteRoleUseCase _deleteRoleUseCase;
        private readonly RemoveUserRoleUseCase _removeUserRoleUseCase;

        public RolesController(
            GetRolesUseCase getRolesUseCase,
            GetRoleByIdUseCase getRoleByIdUseCase,
            GetUserRolesUseCase getUserRolesUseCase,
            CreateRoleUseCase createRoleUseCase,
            AddUserRoleUseCase addUserRoleUseCase,
            UpdateRoleUseCase updateRoleUseCase,
            DeleteRoleUseCase deleteRoleUseCase,
            RemoveUserRoleUseCase removeUserRoleUseCase
        )
        {
            _getRolesUseCase = getRolesUseCase;
            _getRoleByIdUseCase = getRoleByIdUseCase;
            _getUserRolesUseCase = getUserRolesUseCase;
            _createRoleUseCase = createRoleUseCase;
            _addUserRoleUseCase = addUserRoleUseCase;
            _updateRoleUseCase = updateRoleUseCase;
            _deleteRoleUseCase = deleteRoleUseCase;
            _removeUserRoleUseCase = removeUserRoleUseCase;
        }

        [HttpGet("/roles")]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllRoles()
        {
            var roles = await _getRolesUseCase.ExecuteAsync();
            return Ok(roles);
        }

        [HttpGet("/roles/{id}")]
        public async Task<ActionResult<Role>> GetRoleById(Guid id)
        {
            var role = await _getRoleByIdUseCase.ExecuteAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [HttpGet("/roles/user={id}")]
        public async Task<ActionResult<IEnumerable<Role>>> GetUserRoles(string id)
        {
            var roles = await _getUserRolesUseCase.ExecuteAsync(id);
            return Ok(roles);
        }

        [HttpPost("/roles")]
        public async Task<ActionResult<Role>> CreateRole([FromBody] RoleDto createRoleDto)
        {
            var result = await _createRoleUseCase.ExecuteAsync(createRoleDto);
            if (result > 0)
            {
                return CreatedAtAction(
                    nameof(GetRoleById),
                    new { id = createRoleDto.Name },
                    createRoleDto
                );
            }
            return BadRequest("Role could not be created.");
        }

        [HttpPost("/roles/user={id}/add-role")]
        public async Task<ActionResult<Role>> AddUserRole(string id, [FromBody] RoleDto addRoleDto)
        {
            var result = await _addUserRoleUseCase.ExecuteAsync(id, addRoleDto);
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest("Role does not exist.");
        }

        [HttpPut("/roles/{id}")]
        public async Task<ActionResult<Role>> UpdateRole(Guid id, RoleDto updateRoleDto)
        {
            var result = await _updateRoleUseCase.ExecuteAsync(id, updateRoleDto);
            if (result > 0)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("/roles/{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var result = await _deleteRoleUseCase.ExecuteAsync(id);
            if (result > 0)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("/roles/user={id}/remove-role")]
        public async Task<ActionResult> RemoveUserRole(string id, [FromBody] RoleDto removeRoleDto)
        {
            var result = await _removeUserRoleUseCase.ExecuteAsync(id, removeRoleDto);
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest("Role does not exist for the user or user not found.");
        }
    }
}
