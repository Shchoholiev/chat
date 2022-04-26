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
    [Route("api/rooms")]
    public class RoomsController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;

        private readonly IGenericRepository<Room> _roomsRepository;

        private readonly IGenericRepository<User> _usersRepository;

        private readonly Mapper _mapper = new();

        public RoomsController(IHubContext<ChatHub> hubContext, IGenericRepository<Room> roomsRepository, 
                               IGenericRepository<User> usersRepository)
        {
            this._roomsRepository = roomsRepository;
            this._usersRepository = usersRepository;
            this._hubContext = hubContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            return await this._roomsRepository.GetOneAsync(id, r => r.Users);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms([FromQuery] PageParameters pageParameters)
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var rooms = await this._roomsRepository.GetPageAsync(pageParameters, 
                                                                 r => r.Users.Any(u => u.Email == email));
            foreach (var personalRoom in rooms.Where(r => r.DisplayName == null))
            {
                var index = rooms.FindIndex(r => r.Id == personalRoom.Id);
                var room = await this._roomsRepository.GetOneAsync(personalRoom.Id, r => r.Users);
                rooms[index].Users = room.Users.Where(u => u.Email != email).ToList();
            }

            var metadata = new
            {
                rooms.TotalItems,
                rooms.PageSize,
                rooms.PageNumber,
                rooms.TotalPages,
                rooms.HasNextPage,
                rooms.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return rooms;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomDTO roomDTO)
        {
            if (ModelState.IsValid)
            {
                var room = this._mapper.Map(roomDTO);
                var name = this.User?.Identity?.Name;
                room.Messages.Add(new Message { Text = $"Chat created by {name}.", SendDate = DateTime.Now });
                this._roomsRepository.Attach(room);
                await this._roomsRepository.AddAsync(room);

                return CreatedAtAction("GetRoom", new { id = room.Id }, room);
            }

            return BadRequest();
        }

        [HttpPut("add-member")]
        public async Task<IActionResult> AddMember([FromBody] AddToRoomModel model)
        {
            var room = await this._roomsRepository.GetOneAsync(model.RoomId, r => r.Users);
            var user = await this._usersRepository.GetOneAsync(u => u.Email == model.Email);
            if (room == null || room.DisplayName == null || user == null)
            {
                return BadRequest();
            }

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
