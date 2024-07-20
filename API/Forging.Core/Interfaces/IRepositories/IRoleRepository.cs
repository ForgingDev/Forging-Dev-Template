using Forging.Domain.Models;

namespace Forging.Core.Interfaces.IRepositories
{
    public interface IRoleRepository
    {
        public Task<IEnumerable<Role>> GetAllRolesAsync();

        public Task<Role> GetRoleByIdAsync(Guid id);

        public Task<IEnumerable<Role>> GetUserRolesAsync(string userId);

        public Task<int> CreateRoleAsync(Role role);

        public Task<int> UpdateRoleAsync(Guid id, Role role);

        public Task<int> DeleteRoleAsync(Guid id);

        public Task<int> AddUserRoleAsync(string userId, string addRoleDto);

        public Task<int> RemoveUserRoleAsync(string userId, string removeRoleDto);
    }
}
