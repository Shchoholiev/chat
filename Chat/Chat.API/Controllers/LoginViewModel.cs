namespace Chat.API.Controllers
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
