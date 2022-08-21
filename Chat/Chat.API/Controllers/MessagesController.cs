using Chat.Application.Interfaces.Services;
using Chat.Application.Models.Dtos;
using Chat.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    [Authorize]
    public class MessagesController : ApiControllerBase
    {
        private readonly IMessagesService _messagesService;

        public MessagesController(IMessagesService messagesService)
        {
            this._messagesService = messagesService;
        }

        [HttpGet("{roomId}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetPageAsync(
            [FromQuery] PageParameters pageParameters, int roomId, CancellationToken cancellationToken)
        {
            var messages = await this._messagesService.GetPageAsync(pageParameters, roomId, Email, cancellationToken);
            this.SetPagingMetadata(messages);
            return messages;
        }

        [HttpPost]
        public async Task<IActionResult> SendAsync([FromBody] MessageCreateDto messageDTO, 
            CancellationToken cancellationToken)
        {
            await this._messagesService.SendAsync(messageDTO, Email, cancellationToken);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(int id, [FromBody] MessageCreateDto messageDTO, 
            CancellationToken cancellationToken)
        {
            await this._messagesService.EditAsync(id, messageDTO, cancellationToken);
            return NoContent();
        }

        [HttpPut("hide/{id}")]
        public async Task<IActionResult> HideForSenderAsync(int id, CancellationToken cancellationToken)
        {
            await this._messagesService.HideForSenderAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPost("replyInPerson/{email}")]
        public async Task<IActionResult> ReplyInPersonAsync(string email, 
            [FromBody] MessageCreateDto messageDTO, CancellationToken cancellationToken)
        {
            await this._messagesService.ReplyInPersonAsync(email, Email, messageDTO, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await this._messagesService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
