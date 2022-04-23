using Chat.API.Mapping;
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

        public ChatController(IHubContext<ChatHub> hubContext, IGenericRepository<User> usersRepository,
                              IGenericRepository<Room> roomsRepository)
        {
            this._hubContext = hubContext;
            this._usersRepository = usersRepository;
            this._roomsRepository = roomsRepository;
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

            var message = new Message
            {
                Text = $"{user.Name} has joined the group {room.DisplayName}.",
                SendDate = DateTime.Now,
                Room = room
            };
            await this._hubContext.Clients.Group(room.Id.ToString()).SendAsync("MessageSent", message);

            return Ok();
        }
    }
}
