using Chat.API.Models;
using Chat.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Chat.API.Controllers
{
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;

        private readonly IUsersService _usersService;

        public TokenController(ITokenService tokenService, IUsersService usersService)
        {
            this._tokenService = tokenService;
            this._usersService = usersService;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokensModel tokensModel)
        {
            if (tokensModel == null)
            {
                return BadRequest();
            }

            var principal = _tokenService.GetPrincipalFromExpiredToken(tokensModel.AccessToken);
            var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var user = await _usersService.GetUserAsync(email);
            if (user == null || user.UserToken.RefreshToken != tokensModel.RefreshToken
                             || user.UserToken.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest();
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.UserToken.RefreshToken = newRefreshToken;
            await this._usersService.UpdateUserAsync(user);

            return Ok(new
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await this._usersService.GetUserAsync(email);
            if (user == null)
            {
                return BadRequest();
            }
            user.UserToken = null;
            await this._usersService.UpdateUserAsync(user);

            return NoContent();
        }
    }
}
