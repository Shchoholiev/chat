using Chat.Application.Interfaces.Services.Identity;
using Chat.Application.Models.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    public class TokensController : ApiControllerBase
    {
        private readonly ITokensService _tokenService;

        public TokensController(ITokensService tokenService)
        {
            this._tokenService = tokenService;
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<TokensModel>> RefreshAsync([FromBody] TokensModel tokensModel,
                                                                  CancellationToken cancellationToken)
        {
            return await this._tokenService.RefreshAsync(tokensModel, Email, cancellationToken);
        }
    }
}
