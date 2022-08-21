using Chat.Application.Interfaces.Services;
using Chat.Application.Models.Chat;
using Chat.Application.Models.Dtos;
using Chat.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    [Authorize]
    public class RoomsController : ApiControllerBase
    {
        private readonly IRoomsService _roomsService;

        public RoomsController(IRoomsService roomsService)
        {
            this._roomsService = roomsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetRoomsAsync(
            [FromQuery] PageParameters pageParameters, CancellationToken cancellationToken)
        {
            var rooms = await this._roomsService.GetRoomsAsync(pageParameters, Email, cancellationToken);
            this.SetPagingMetadata(rooms);
            return rooms;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetRoomAsync(int id, CancellationToken cancellationToken)
        {
            return await this._roomsService.GetRoomAsync(id, cancellationToken);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomDto roomDTO, CancellationToken cancellationToken)
        {
            var room = await this._roomsService.CreateAsync(roomDTO, Email, cancellationToken);
            return CreatedAtAction("GetRoom", new { id = room.Id }, room);
        }

        [HttpPut("add-member")]
        public async Task<IActionResult> AddMember([FromBody] AddToRoomModel model, CancellationToken cancellationToken)
        {
            await this._roomsService.AddMemberAsync(model, cancellationToken);
            return Ok();
        }
    }
}
