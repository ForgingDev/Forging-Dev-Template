using Forging.Core.Dtos;
using Forging.Core.Interfaces.IRepositories;
using Forging.Domain.Models;

namespace Forging.Core.UseCases.RoleUseCases
{
    public class UpdateRoleUseCase
    {
        private readonly IRoleRepository _roleRepository;

        public UpdateRoleUseCase(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<int> ExecuteAsync(Guid id, RoleDto roleDto)
        {
            var role = new Role { Id = id, Name = roleDto.Name };
            return _roleRepository.UpdateRoleAsync(id, role);
        }
    }
}
