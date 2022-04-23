using Chat.API.Mapping;
using Chat.Application.DTO;
using Chat.Application.IRepositories;
using Chat.Application.Paging;
using Chat.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Chat.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/rooms")]
    public class RoomsController : Controller
    {
        private readonly IGenericRepository<Room> _roomsRepository;

        private readonly Mapper _mapper = new();

        public RoomsController(IGenericRepository<Room> roomsRepository)
        {
            this._roomsRepository = roomsRepository;
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
                await this._roomsRepository.AddAsync(room);
                return CreatedAtAction("GetRoom", new { id = room.Id }, room);
            }

            return BadRequest();
        }

    }
}
