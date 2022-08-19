using System.Security.Claims;

namespace Chat.Application.Interfaces.Services.Identity
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
