using Forging.Core.Interfaces.IRepositories;

namespace Forging.Core.UseCases.UserUseCases
{
    public class DeleteUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<int> ExecuteAsync(string id)
        {
            return _userRepository.DeleteUserAsync(id);
        }
    }
}
