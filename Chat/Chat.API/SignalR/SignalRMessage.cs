using Chat.Application.DTO;

namespace Chat.API.SignalR
{
    public class SignalRMessage
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime SendDate { get; set; }

        public UserDTO Sender { get; set; }
    }
}
