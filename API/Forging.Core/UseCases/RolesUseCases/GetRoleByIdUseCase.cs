using Forging.Core.Interfaces.IRepositories;
using Forging.Domain.Models;

namespace Forging.Core.UseCases.RoleUseCases
{
    public class GetRoleByIdUseCase
    {
        private readonly IRoleRepository _roleRepository;

        public GetRoleByIdUseCase(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<Role> ExecuteAsync(Guid id)
        {
            return _roleRepository.GetRoleByIdAsync(id);
        }
    }
}
