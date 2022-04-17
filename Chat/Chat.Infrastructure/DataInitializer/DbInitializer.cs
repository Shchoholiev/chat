using Chat.Core.Entities.Identity;
using Chat.Infrastructure.EF;
using Chat.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace Chat.Infrastructure.DataInitializer
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationContext context, IConfiguration configuration)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var tokenService = new TokenService(configuration);
            var passwordHasher = new PasswordHasher();

            var user = new User
            {
                Id = DateTime.Now.Ticks.ToString(),
                Name = "Petro",
                Email = "petro@petro",
                PasswordHash = passwordHasher.Hash("petro"),
                UserToken = new UserToken
                {
                    RefreshToken = tokenService.GenerateRefreshToken(),
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
                }
            };

            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
