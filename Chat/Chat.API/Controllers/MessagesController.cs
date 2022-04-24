using Chat.API.Mapping;
using Chat.API.SignalR;
using Chat.Application.DTO;
using Chat.Application.IRepositories;
using Chat.Application.Paging;
using Chat.Core.Entities;
using Chat.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Chat.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;

        private readonly IGenericRepository<User> _usersRepository;

        private readonly IGenericRepository<Room> _roomsRepository;

        private readonly IMessagesRepository _messagesReposiory;

        private readonly Mapper _mapper = new();

        public MessagesController(IHubContext<ChatHub> hubContext, IGenericRepository<User> usersRepository,
                                  IGenericRepository<Room> roomsRepository, IMessagesRepository messagesReposiory)
        {
            this._hubContext = hubContext;
            this._usersRepository = usersRepository;
            this._roomsRepository = roomsRepository;
            this._messagesReposiory = messagesReposiory;
        }

        [HttpGet("{roomId}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetPage([FromQuery] PageParameters pageParameters, int roomId)
        {
            var email = this.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var messages = await this._messagesReposiory.GetPageAsync(pageParameters, roomId, email);
            var metadata = new
            {
                messages.TotalItems,
                messages.PageSize,
                messages.PageNumber,
                messages.TotalPages,
                messages.HasNextPage,
                messages.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return messages;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] MessageDTO messageDTO)
        {
            var email = this.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await this._usersRepository.GetOneAsync(u => u.Email == email);
            var room = await this._roomsRepository.GetOneAsync(messageDTO.RoomId);
            if (user == null || room == null)
            {
                return BadRequest();
            }

            var message = new Message
            {
                Text = messageDTO.Text,
                SendDate = DateTime.Now,
                Sender = user,
                Room = room
            };

            if (messageDTO.RepliedTo > 0)
            {
                var repliedTo = await this._messagesReposiory.GetMessageAsync(messageDTO.RepliedTo);
                message.RepliedTo = repliedTo;
            }
            await this._messagesReposiory.AddAsync(message);

            var signalrMessage = this._mapper.Map(message);
            await this._hubContext.Clients.Group(room.Id.ToString()).SendAsync("MessageSent", signalrMessage);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] MessageDTO messageDTO)
        {
            var message = await this._messagesReposiory.GetMessageAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            message.Text = messageDTO.Text;
            await this._messagesReposiory.UpdateAsync(message);
            var signalrMessage = this._mapper.Map(message);
            await this._hubContext.Clients.Group(messageDTO.RoomId.ToString())
                                          .SendAsync("MessageEdited", signalrMessage);

            return NoContent();
        }

        [HttpPut("hide/{id}")]
        public async Task<IActionResult> HideForSender(int id)
        {
            var message = await this._messagesReposiory.GetFullMessageAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            message.HideForSender = true;
            await this._messagesReposiory.UpdateAsync(message);
            var signalrMessage = this._mapper.Map(message);
            await this._hubContext.Clients.Group(message.Room.Id.ToString())
                                          .SendAsync("MessageHiddenForUser", signalrMessage);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await this._messagesReposiory.GetFullMessageAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            await this._messagesReposiory.DeleteAsync(message);
            await this._hubContext.Clients.Group(message.Room.Id.ToString()).SendAsync("MessageDeleted", id);

            return NoContent();
        }
    }
}
