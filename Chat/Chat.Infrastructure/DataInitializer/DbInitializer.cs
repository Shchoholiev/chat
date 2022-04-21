using Chat.Core.Entities;
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

            var petro = new User
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
            context.Users.Add(petro);
            context.SaveChanges();

            var petya = new User
            {
                Id = DateTime.Now.Ticks.ToString(),
                Name = "Petya",
                Email = "petya@petya",
                PasswordHash = passwordHasher.Hash("petya"),
                UserToken = new UserToken
                {
                    RefreshToken = tokenService.GenerateRefreshToken(),
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
                }
            };
            context.Users.Add(petya);
            context.SaveChanges();

            var privateChat = new Room
            {
                Users = new List<User> { petro, petya },
            };
            context.Rooms.Add(privateChat);
            context.SaveChanges();

            var maks = new User
            {
                Id = DateTime.Now.Ticks.ToString(),
                Name = "Maks",
                Email = "maks@maks",
                PasswordHash = passwordHasher.Hash("maks"),
                UserToken = new UserToken
                {
                    RefreshToken = tokenService.GenerateRefreshToken(),
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
                }
            };
            context.Users.Add(maks);
            context.SaveChanges();

            var misha = new User
            {
                Id = DateTime.Now.Ticks.ToString(),
                Name = "Misha",
                Email = "misha@misha",
                PasswordHash = passwordHasher.Hash("misha"),
                UserToken = new UserToken
                {
                    RefreshToken = tokenService.GenerateRefreshToken(),
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
                }
            };
            context.Users.Add(misha);
            context.SaveChanges();
            
            var dotNetGroup = new Room
            {
                DisplayName = ".NET Devs",
                Users = new List<User> { petro, petya, maks, misha },
                Messages = new List<Message>
                {
                    new Message { Text = $"Chat created by {maks.Name}", SendDate = DateTime.Now.AddHours(-1) },
                    new Message { Text = $"Hi guys!", SendDate = DateTime.Now.AddHours(-0.9), Sender = petro },
                    new Message { Text = $"Hey petro", SendDate = DateTime.Now.AddHours(-0.8), Sender = misha },
                    new Message { Text = $"Can you help me?", SendDate = DateTime.Now.AddHours(-0.7), Sender = petro },
                    new Message { Text = $"Sure, what's wrong?", SendDate = DateTime.Now.AddHours(-0.6), Sender = maks },
                    new Message 
                    { 
                        Text = $"I'm making chat web app using Signal R and I don't understand how to add " +
                               $"database to app. To save messages and retrieve them later", 
                        SendDate = DateTime.Now.AddHours(-0.5), Sender = petro 
                    },
                    new Message { Text = $"Do you know how to add database to app in general?", 
                                  SendDate = DateTime.Now.AddHours(-0.4), Sender = maks },
                    new Message { Text = $"Of course", SendDate = DateTime.Now.AddHours(-0.3), Sender = petro },
                    new Message { Text = $"So add db and then use it in Hub class or in controller", 
                                  SendDate = DateTime.Now.AddHours(-0.2), Sender = maks },
                    new Message { Text = $"In second case you should add it as IHubContext<YourHubClass>",
                                  SendDate = DateTime.Now.AddHours(-0.1), Sender = misha },
                    new Message { Text = $"Great, thanks!", SendDate = DateTime.Now, Sender = petro },
                }
            };
            context.Rooms.Add(dotNetGroup);
            context.SaveChanges();

            var angularGroup = new Room
            {
                DisplayName = "Angular",
                Users = new List<User> { petro, petya, maks },
                Messages = new List<Message>
                {
                    new Message { Text = $"Chat created by {petya.Name}", SendDate = DateTime.Now.AddHours(-1) },
                    new Message { Text = $"Petya, can you add misha?", SendDate = DateTime.Now.AddHours(-0.9), Sender = petro },
                    new Message { Text = $"I thought he uses react, no?", SendDate = DateTime.Now.AddHours(-0.8), Sender = petya },
                    new Message { Text = $"He uses both", SendDate = DateTime.Now.AddHours(-0.7), Sender = petro },
                    new Message { Text = $"Ok", SendDate = DateTime.Now.AddHours(-0.6), Sender = petya },

                }
            };
            context.Rooms.Add(angularGroup);
            context.SaveChanges();
        }
    }
}
