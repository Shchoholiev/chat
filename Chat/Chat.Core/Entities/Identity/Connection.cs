namespace Chat.Core.Entities.Identity
{
    public class Connection : EntityBase
    {
        public bool IsConnected { get; set; }

        public User User { get; set; }
    }
}
