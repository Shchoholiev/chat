using Chat.API.Models;
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
    [Route("api/chat")]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;

        private readonly IGenericRepository<User> _usersRepository;

        private readonly IGenericRepository<Room> _roomsRepository;

        private readonly IMessagesRepository _messagesReposiory;

        public ChatController(IHubContext<ChatHub> hubContext, IGenericRepository<User> usersRepository,
                              IGenericRepository<Room> roomsRepository, IMessagesRepository messagesReposiory)
        {
            this._hubContext = hubContext;
            this._usersRepository = usersRepository;
            this._roomsRepository = roomsRepository;
            this._messagesReposiory = messagesReposiory;
        }

        [HttpPost("add-to-group")]
        public async Task<IActionResult> AddToGroup([FromBody] AddToGroupModel model)
        {
            var room = await this._roomsRepository.GetOneAsync(model.RoomId, r => r.Users);
            if (room == null || room.DisplayName == null)
            {
                return BadRequest();
            }

            var user = await this._usersRepository.GetOneAsync(u => u.Email == model.Email);
            room.Users.Add(user);
            this._roomsRepository.Attach(room);
            await this._roomsRepository.UpdateAsync(room);

            await this.SendMessage(new MessageDTO
            {
                RoomId = room.Id,
                Text = $"{user.Name} has joined the group {room.DisplayName}."
            });

            return Ok();
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDTO messageDTO)
        {
            var email = this.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await this._usersRepository.GetOneAsync(u => u.Email == email, u => u.Connections);
            var room = await this._roomsRepository.GetOneAsync(messageDTO.RoomId, r => r.Users);

            if (user == null || room == null || !room.Users.Any(u => u.Email == email))
            {
                return BadRequest();
            }

            var message = new Message { Text = messageDTO.Text, SendDate = DateTime.Now, Sender = user, Room = room };
            await this._hubContext.Clients.Group(room.Id.ToString()).SendAsync("MessageSent", message);
            this._roomsRepository.Attach(message);
            await this._roomsRepository.SaveAsync();

            return Ok();
        }

        [HttpGet("{roomId}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages([FromQuery] PageParameters pageParameters, int roomId)
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
    }
}
