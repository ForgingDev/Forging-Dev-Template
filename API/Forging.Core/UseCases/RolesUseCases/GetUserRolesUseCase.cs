using Forging.Core.Interfaces.IRepositories;
using Forging.Domain.Models;

namespace Forging.Core.UseCases.RoleUseCases
{
    public class GetUserRolesUseCase
    {
        private readonly IRoleRepository _roleRepository;

        public GetUserRolesUseCase(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<IEnumerable<Role>> ExecuteAsync(string userId)
        {
            return _roleRepository.GetUserRolesAsync(userId);
        }
    }
}
