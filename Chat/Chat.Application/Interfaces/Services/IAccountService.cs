using Chat.Application.Models.Dtos;
using Chat.Application.Models.Identity;

namespace Chat.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<TokensModel> RegisterAsync(RegisterModel register, CancellationToken cancellationToken);

        Task<TokensModel> LoginAsync(LoginModel login, CancellationToken cancellationToken);

        Task<UserDto> GetUserAsync(string email, CancellationToken cancellationToken);
    }
}
