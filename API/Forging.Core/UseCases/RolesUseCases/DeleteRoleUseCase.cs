using Forging.Core.Interfaces.IRepositories;
using Forging.Domain.Models;

namespace Forging.Core.UseCases.RoleUseCases
{
    public class DeleteRoleUseCase
    {
        private readonly IRoleRepository _roleRepository;

        public DeleteRoleUseCase(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<int> ExecuteAsync(Guid id)
        {
            return _roleRepository.DeleteRoleAsync(id);
        }
    }
}
