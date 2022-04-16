using Chat.Core.Entities.Identity;

namespace Chat.Core.Entities
{
    public class Message : EntityBase
    {
        public string Text { get; set; }

        public DateTime SendDate { get; set; }

        public Chat Chat { get; set; }

        public User? Sender { get; set; }

        public bool HideForSender { get; set; }
    }
}
