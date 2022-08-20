using Chat.Application.Interfaces.Services.Identity;
using Chat.Application.Models.Dtos;
using Chat.Application.Models.Identity;
using Chat.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Chat.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IUserManager _usersService;

        private readonly ITokensService _tokenService;

        public AccountController(IUserManager usersService, ITokensService tokenService)
        {
            this._usersService = usersService;
            this._tokenService = tokenService;
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<User>> GetUser(string email)
        {
            return await this._usersService.GetUserAsync(email);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var userDTO = new UserDto { Name = model.Name, Email = model.Email, Password = model.Password };
                var result = await this._usersService.RegisterAsync(userDTO);

                if (result.Succeeded)
                {
                    var user = await this._usersService.GetUserAsync(userDTO.Email);
                    var tokens = await this.UpdateUserTokens(user);
                    return Ok(tokens);
                }
                else
                {
                    return BadRequest(new { errors = result.Messages });
                }
            }

            return BadRequest();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var userDTO = new UserDto { Email = model.Email, Password = model.Password };
                var result = await this._usersService.LoginAsync(userDTO);

                if (result.Succeeded)
                {
                    var user = await this._usersService.GetUserAsync(userDTO.Email);
                    var tokens = await this.UpdateUserTokens(user);
                    return Ok(tokens);
                }
                else
                {
                    return BadRequest(new { errors = result.Messages });
                }
            }

            return BadRequest();
        }

        private async Task<Object> UpdateUserTokens(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var accessToken = this._tokenService.GenerateAccessToken(claims);
            var refreshToken = this._tokenService.GenerateRefreshToken();

            if (user?.UserToken == null)
            {
                user.UserToken = new UserToken
                {
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
                };
            }
            else
            {
                user.UserToken.RefreshToken = refreshToken;
                user.UserToken.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            }
            await this._usersService.SaveAsync();

            return new TokensModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
