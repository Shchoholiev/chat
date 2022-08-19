using Chat.Application.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Interfaces.Services
{
    public interface IRoomsService
    {
        Task<RoomDto> GetRoomAsync(int id, CancellationToken cancellationToken);
    }
}
