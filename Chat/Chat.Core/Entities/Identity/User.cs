using System.ComponentModel.DataAnnotations;

namespace Chat.Core.Entities.Identity
{
    public class User : EntityBase
    {
        [Key]
        public new string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public UserToken? UserToken { get; set; }

        public List<Connection>? Connections { get; set; } = new();

        public List<Room>? Rooms { get; set; }
    }
}
