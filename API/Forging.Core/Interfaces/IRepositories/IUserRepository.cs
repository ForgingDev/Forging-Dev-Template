using Forging.Core.Dtos;
using Forging.Domain.Models;

namespace Forging.Core.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<User> GetUserByIdAsync(string id);
        public Task<int> CreateUserAsync(User createUserDto);
        public Task<int> UpdateUserAsync(string id, User updateUserDto);
        public Task<int> DeleteUserAsync(string id);
    }
}
