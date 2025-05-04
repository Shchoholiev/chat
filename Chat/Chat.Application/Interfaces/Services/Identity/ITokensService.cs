using Chat.Application.Models.Identity;
using System.Security.Claims;

namespace Chat.Application.Interfaces.Services.Identity
{
    public interface ITokensService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();

        Task<TokensModel> RefreshAsync(TokensModel tokensModel, string email, CancellationToken cancellationToken);
    }
}
