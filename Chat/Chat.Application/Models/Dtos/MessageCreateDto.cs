using Chat.Core.Entities;
using Chat.Core.Entities.Identity;

namespace Chat.Application.Models.Dtos
{
    public class MessageCreateDto
    {
        public string Text { get; set; }

        public int RepliedTo { get; set; }

        public int RoomId { get; set; }
    }
}
