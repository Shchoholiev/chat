namespace Chat.Application.Models.Identity
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string? RefreshToken { get; set; }
    }
}
