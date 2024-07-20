using Forging.Core.Dtos;
using Forging.Core.Interfaces.IRepositories;
using Forging.Domain.Models;

namespace Forging.Core.UseCases.RoleUseCases
{
    public class AddUserRoleUseCase
    {
        private readonly IRoleRepository _roleRepository;

        public AddUserRoleUseCase(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<int> ExecuteAsync(string userId, RoleDto addRoleDto)
        {
            return _roleRepository.AddUserRoleAsync(userId, addRoleDto.Name);
        }
    }
}
