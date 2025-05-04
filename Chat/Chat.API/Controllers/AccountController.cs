using Chat.Application.Interfaces.Services;
using Chat.Application.Models.Dtos;
using Chat.Application.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    [Authorize]
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<UserDto>> GetUserAsync(string email, CancellationToken cancellationToken)
        {
            return await this._accountService.GetUserAsync(email, cancellationToken);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<TokensModel>> RegisterAsync([FromBody] RegisterModel model, 
            CancellationToken cancellationToken)
        {
            return await this._accountService.RegisterAsync(model, cancellationToken);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokensModel>> LoginAsync([FromBody] LoginModel model, 
            CancellationToken cancellationToken)
        {
            return await this._accountService.LoginAsync(model, cancellationToken);
        }
    }
}
