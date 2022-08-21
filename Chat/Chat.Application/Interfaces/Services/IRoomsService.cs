using Chat.Application.Models.Chat;
using Chat.Application.Models.Dtos;
using Chat.Application.Paging;

namespace Chat.Application.Interfaces.Services
{
    public interface IRoomsService
    {
        Task<RoomDto> GetRoomAsync(int id, CancellationToken cancellationToken);

        Task<PagedList<RoomDto>> GetRoomsAsync(PageParameters pageParameters, string userEmail, CancellationToken cancellationToken);

        Task<RoomDto> CreateAsync(RoomDto roomDTO, string userName, CancellationToken cancellationToken);

        Task AddMemberAsync(AddToRoomModel model, CancellationToken cancellationToken);
    }
}
