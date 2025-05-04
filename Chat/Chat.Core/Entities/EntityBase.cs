using System.ComponentModel.DataAnnotations;

namespace Chat.Core.Entities
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
