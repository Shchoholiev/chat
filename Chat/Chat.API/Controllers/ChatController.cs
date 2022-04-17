using Chat.API.SignalR;
using Chat.Application.DTO;
using Chat.Application.IRepositories;
using Chat.Core.Entities;
using Chat.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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

        public ChatController(IHubContext<ChatHub> hubContext, IGenericRepository<User> usersRepository,
                              IGenericRepository<Room> roomsRepository)
        {
            this._hubContext = hubContext;
            this._usersRepository = usersRepository;
            this._roomsRepository = roomsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessageToUser([FromBody] MessageDTO messageDTO)
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await this._usersRepository.GetOneAsync(u => u.Email == email, u => u.Connections);
            var room = await this._roomsRepository.GetOneAsync(messageDTO.RoomId, r => r.Users, r => r.Messages);

            if (user == null || room == null || room.Name != null || !room.Users.Any(u => u.Email == email))
            {
                return BadRequest();
            }
            var recipient = room.Users.FirstOrDefault(u => u.Id != user.Id);

            foreach (var connection in user.Connections.Where(c => c.IsConnected == true))
            {
                await this._hubContext.Clients.User(recipient.Id).SendAsync("SendMessageToUser", messageDTO.Text);
            }
            var message = new Message { Text = messageDTO.Text, SendDate = DateTime.Now, Sender = user, Room = room };
            this._roomsRepository.Attach(message);
            await this._roomsRepository.SaveAsync();

            return Ok();
        }
    }
}
