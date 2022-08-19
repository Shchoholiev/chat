using Chat.Application.Descriptions;
using Chat.Application.Models.Dtos;
using Chat.Core.Entities.Identity;

namespace Chat.Application.Interfaces.Services.Identity
{
    public interface IUsersService
    {
        Task<OperationDetails> RegisterAsync(UserDto userDTO);

        Task<OperationDetails> LoginAsync(UserDto userDTO);

        Task<OperationDetails> UpdateUserAsync(User user);

        Task DeleteAsync(string id);

        Task<User?> GetUserAsync(string email);

        Task SaveAsync();
    }
}
