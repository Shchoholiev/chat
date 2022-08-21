using Chat.Application.Models.Identity;

namespace Chat.Application.Interfaces.Services.Identity
{
    public interface IUserManager
    {
        Task<TokensModel> RegisterAsync(RegisterModel register, CancellationToken cancellationToken);

        Task<TokensModel> LoginAsync(LoginModel login, CancellationToken cancellationToken);
    }
}
