using Chat.Application.Models.Chat;
using Chat.Application.Models.Dtos;
using Chat.Application.Paging;

namespace Chat.Application.Interfaces.Services
{
    public interface IRoomsService
    {
        Task<RoomDto> GetRoomAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<RoomDto>> GetRoomsAsync(PageParameters pageParameters, CancellationToken cancellationToken);

        Task CreateAsync(RoomDto roomDTO, CancellationToken cancellationToken);

        Task AddMemberAsync(AddToRoomModel model, CancellationToken cancellationToken);
    }
}
