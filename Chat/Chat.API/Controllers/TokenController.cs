using Chat.Application.Interfaces;
using Chat.Application.IRepositories;
using Chat.Application.Models.Identity;
using Chat.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Chat.API.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;

        private readonly IGenericRepository<User> _usersRepository;

        public TokenController(ITokenService tokenService, IGenericRepository<User> usersRepository)
        {
            this._tokenService = tokenService;
            this._usersRepository = usersRepository;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokensModel tokensModel)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokensModel.AccessToken);
            var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var user = await _usersRepository.GetOneAsync(u => u.Email == email, u => u.UserToken);
            if (user == null || user?.UserToken?.RefreshToken != tokensModel.RefreshToken
                             || user?.UserToken?.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest();
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.UserToken.RefreshToken = newRefreshToken;
            await this._usersRepository.UpdateAsync(user);

            return Ok(new TokensModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
