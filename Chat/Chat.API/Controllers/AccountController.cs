using Chat.API.Mapping;
using Chat.API.Models;
using Chat.Application.DTO;
using Chat.Application.Interfaces;
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
        private readonly IUsersService _usersService;

        private readonly ITokenService _tokenService;

        private readonly Mapper _mapper = new();

        public AccountController(IUsersService usersService, ITokenService tokenService)
        {
            this._usersService = usersService;
            this._tokenService = tokenService;
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserDTO userDTO)
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await this._usersService.GetUserAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            if (email != userDTO.Email && await this._usersService.GetUserAsync(email) != null)
            {
                return BadRequest("User with this email already exists");
            }

            this._mapper.Map(user, userDTO);
            var tokens = await UpdateUserTokens(user);
            return Ok(tokens);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userDTO = new UserDTO { Name = model.Name, Email = model.Email, Password = model.Password };
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
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userDTO = new UserDTO { Email = model.Email, Password = model.Password };
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

            user.UserToken = new UserToken
            {
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
            };
            await this._usersService.UpdateUserAsync(user);

            return new TokensModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
