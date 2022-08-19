using Chat.Application.Models.Dtos;

namespace Chat.API.SignalR
{
    public class SignalRMessage
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime SendDate { get; set; }

        public UserDto Sender { get; set; }

        public bool HideForSender { get; set; }

        public SignalRMessage RepliedTo { get; set; }
    }
}
