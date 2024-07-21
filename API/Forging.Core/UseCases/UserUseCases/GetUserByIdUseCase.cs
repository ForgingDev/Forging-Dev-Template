using Forging.Core.Interfaces.IRepositories;
using Forging.Domain.Models;

namespace Forging.Core.UseCases.UserUseCases
{
    public class GetUserByIdUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> ExecuteAsync(string id)
        {
            return _userRepository.GetUserByIdAsync(id);
        }
    }
}
