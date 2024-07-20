using Forging.Core.Dtos;
using Forging.Core.Interfaces.IRepositories;
using Forging.Domain.Models;

namespace Forging.Core.UseCases.RoleUseCases
{
    public class CreateRoleUseCase
    {
        private readonly IRoleRepository _roleRepository;

        public CreateRoleUseCase(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<int> ExecuteAsync(RoleDto roleDto)
        {
            var newRole = new Role { Id = Guid.NewGuid(), Name = roleDto.Name };
            return await _roleRepository.CreateRoleAsync(newRole);
        }
    }
}
