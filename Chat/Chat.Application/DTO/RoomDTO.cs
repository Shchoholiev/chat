using Chat.Core.Entities.Identity;

namespace Chat.Application.DTO
{
    public class RoomDTO
    {
        public string? DisplayName { get; set; }

        public List<UserDTO> Users { get; set; }
    }
}
