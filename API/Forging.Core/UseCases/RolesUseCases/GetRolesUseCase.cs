using Forging.Core.Interfaces.IRepositories;
using Forging.Domain.Models;

namespace Forging.Core.UseCases.RoleUseCases
{
    public class GetRolesUseCase
    {
        private readonly IRoleRepository _roleRepository;

        public GetRolesUseCase(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<IEnumerable<Role>> ExecuteAsync()
        {
            return _roleRepository.GetAllRolesAsync();
        }
    }
}
