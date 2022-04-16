using System.ComponentModel.DataAnnotations;

namespace Chat.Core.Entities.Identity
{
    public class Connection
    {
        [Key]
        public int ConnectionId { get; set; }

        public bool IsConnected { get; set; }

        public User User { get; set; }
    }
}
