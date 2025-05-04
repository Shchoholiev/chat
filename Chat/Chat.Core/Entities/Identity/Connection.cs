using System.ComponentModel.DataAnnotations;

namespace Chat.Core.Entities.Identity
{
    public class Connection : EntityBase
    {
        [Key]
        public new string Id { get; set; }

        public User User { get; set; }
    }
}
