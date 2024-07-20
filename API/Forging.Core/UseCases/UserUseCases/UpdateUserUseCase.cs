using Forging.Core.Interfaces.IRepositories;
using Forging.Domain.Models;

namespace Forging.Core.UseCases.UserUseCases
{
    public class UpdateUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<int> ExecuteAsync(string id, User user)
        {
            return _userRepository.UpdateUserAsync(id, user);
        }
    }
}
