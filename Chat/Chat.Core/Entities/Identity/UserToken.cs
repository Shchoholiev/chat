namespace Chat.Core.Entities.Identity
{
    public class UserToken : EntityBase
    {
        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
