using Chat.Application.Descriptions;
using Chat.Application.DTO;
using Chat.Core.Entities.Identity;

namespace Chat.Application.Interfaces
{
    public interface IUsersService
    {
        Task<OperationDetails> RegisterAsync(UserDTO userDTO);

        Task<OperationDetails> LoginAsync(UserDTO userDTO);

        Task<OperationDetails> UpdateUserAsync(User user);

        Task DeleteAsync(string id);

        Task<User?> GetUserAsync(string email);
    }
}
