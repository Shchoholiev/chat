using Chat.Core.Entities;
using Chat.Core.Entities.Identity;

namespace Chat.Application.Models.Dtos
{
    public class MessageDto
    {
        public string Text { get; set; }

        public DateTime SendDateUTC { get; set; }

        public Room Room { get; set; }

        public User Sender { get; set; }

        public bool HideForSender { get; set; }

        public Message RepliedTo { get; set; }
    }
}
