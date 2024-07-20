using Forging.Core.Dtos;
using Forging.Core.Interfaces.IRepositories;
using Forging.Domain.Models;

namespace Forging.Core.UseCases.RoleUseCases
{
    public class RemoveUserRoleUseCase
    {
        private readonly IRoleRepository _roleRepository;

        public RemoveUserRoleUseCase(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<int> ExecuteAsync(string userId, RoleDto roleDto)
        {
            return _roleRepository.RemoveUserRoleAsync(userId, roleDto.Name);
        }
    }
}
