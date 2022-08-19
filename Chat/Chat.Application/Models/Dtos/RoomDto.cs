namespace Chat.Application.Models.Dtos
{
    public class RoomDto
    {
        public string? DisplayName { get; set; }

        public List<MessageDto> Messages { get; set; }

        public List<UserDto> Users { get; set; }
    }
}
