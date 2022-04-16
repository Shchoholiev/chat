using Chat.Core.Entities.Identity;

namespace Chat.Core.Entities
{
    public class Chat : EntityBase
    {
        public string? Name { get; set; }

        public List<Message>? Messages { get; set; }

        public List<User> Users { get; set; }
    }
}
