using Chat.Core.Entities;
using Chat.Core.Entities.Identity;
using Chat.Infrastructure.EF;
using Chat.Infrastructure.Services;
using Chat.Infrastructure.Services.Identity;
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

            var privateChatMessage1 = new Message
            {
                Text = "Is rollerblading popular in America?",
                SendDate = DateTime.Now.AddHours(-8.9),
                Sender = petro
            };

            var privateChatMessage2 = new Message
            {
                Text = "Should I buy a ticket before travelling?",
                SendDate = DateTime.Now.AddHours(-5.7),
                Sender = petya
            };

            var privateChat = new Room
            {
                Users = new List<User> { petro, petya },
                Messages = new List<Message>
                {
                    new Message { Text = "Good morning", SendDate = DateTime.Now.AddHours(-10), Sender = petro },
                    new Message { Text = "Good morning. Are you going somewhere?",
                                  SendDate = DateTime.Now.AddHours(-10), Sender = petya },
                    new Message { Text = " Yes. I and Patrick are going to the city to do some shopping and have lunch.",
                                  SendDate = DateTime.Now.AddHours(-9.9), Sender = petro },
                    new Message { Text = "That sounds fun. How will you get there?",
                                  SendDate = DateTime.Now.AddHours(-9.8), Sender = petya },
                    new Message { Text = "I want to take the train, but Petro prefers to ride her bicycle.",
                                  SendDate = DateTime.Now.AddHours(-9.7), Sender = petro },
                    new Message { Text = "Don’t you like using the train, Petro?",
                                  SendDate = DateTime.Now.AddHours(-9.6), Sender = petya },
                    new Message { Text = "The train station is far away and riding my bicycle does not cost anything",
                                  SendDate = DateTime.Now.AddHours(-9.5), Sender = petro },
                    new Message { Text = "Ok. Petro, why don’t we travel by bus, there is a bus stop just behind the university",
                                  SendDate = DateTime.Now.AddHours(-9.4), Sender = petya },
                    new Message { Text = "It might take longer. I think we will need to take two buses to the city centre. ",
                                  SendDate = DateTime.Now.AddHours(-9.3), Sender = petro },
                    new Message { Text = "Petro is right. There is not a direct bus into the city. Why don’t you use a taxi?",
                                  SendDate = DateTime.Now.AddHours(-9.2), Sender = petya },
                    new Message { Text = "That will be quick, but expensive. Can you rollerblade?",
                                  SendDate = DateTime.Now.AddHours(-9.1), Sender = petro },
                    new Message { Text = "I hope you don’t mean we should rollerblade to the city.",
                                  SendDate = DateTime.Now.AddHours(-9), Sender = petya },
                    privateChatMessage1,
                    new Message { Text = "Yes. People often use roller-skates. There is even a designated lane for it.",
                                  SendDate = DateTime.Now.AddHours(-8.8), Sender = petya, RepliedTo = privateChatMessage1 },
                    new Message { Text = "In UK we can’t do that, the path is just for walking on.",
                                  SendDate = DateTime.Now.AddHours(-8.7), Sender = petro },
                    new Message { Text = "I need to go to the train station to buy some tickets. I can give you a lift in my car.",
                                  SendDate = DateTime.Now.AddHours(-8.6), Sender = petya },
                    new Message { Text = "That would be wonderful. Thanks so much.",
                                  SendDate = DateTime.Now.AddHours(-8.5), Sender = petro },
                    new Message { Text = "That is very kind of you. Let’s go to the city centre!",
                                  SendDate = DateTime.Now.AddHours(-8.4), Sender = petya, HideForSender = true },

                    new Message { Text = "What are you doing?",
                                  SendDate = DateTime.Now.AddHours(-7.0), Sender = petro },
                    new Message { Text = "I’m planning my trip back to Thailand for the mid-term break.",
                                  SendDate = DateTime.Now.AddHours(-6.9), Sender = petya },
                    new Message { Text = "That sounds exciting.",
                                  SendDate = DateTime.Now.AddHours(-6.8), Sender = petro },
                    new Message { Text = "Not really. I must do a lot of travelling to get back to my home.",
                                  SendDate = DateTime.Now.AddHours(-6.7), Sender = petya },
                    new Message { Text = "Really? Why? How will you get home and how long will it take?",
                                  SendDate = DateTime.Now.AddHours(-6.6), Sender = petro },
                    new Message { Text = "It will take more than 24 hours because I have to use many different types of transport.",
                                  SendDate = DateTime.Now.AddHours(-6.5), Sender = petya },
                    new Message { Text = "Will your family meet you at the airport in Thailand?",
                                  SendDate = DateTime.Now.AddHours(-6.4), Sender = petro },
                    new Message { Text = "No. I must take a bus from the airport to my home.",
                                  SendDate = DateTime.Now.AddHours(-6.3), Sender = petya },
                    new Message { Text = "That does not sound nice. How will you get to London Heathrow airport?",
                                  SendDate = DateTime.Now.AddHours(-6.2), Sender = petro },
                    new Message { Text = "I think it will be cheapest to use the coach, but I have a lot of luggage.",
                                  SendDate = DateTime.Now.AddHours(-6.1), Sender = petya },
                    new Message { Text = "That’s ok. Usually you can pay extra to take more luggage.",
                                  SendDate = DateTime.Now.AddHours(-6), Sender = petro },
                    new Message { Text = "Really? That’s good. Do you know where the bus station is from here?",
                                  SendDate = DateTime.Now.AddHours(-5.9), Sender = petya },
                    new Message { Text = "Yes, you can walk there. It will only take you 5 minutes.",
                                  SendDate = DateTime.Now.AddHours(-5.8), Sender = petro },
                    privateChatMessage2,
                    new Message { Text = "Buy your ticket online if you can. It is always cheaper online.",
                                  SendDate = DateTime.Now.AddHours(-5.6), Sender = petro, RepliedTo = privateChatMessage2 },
                    new Message { Text = "I want to travel direct to London. Do they have direct buses to the airport?",
                                  SendDate = DateTime.Now.AddHours(-5.5), Sender = petya },
                    new Message { Text = "Yes, you can walk there. It will only take you 5 minutes.",
                                  SendDate = DateTime.Now.AddHours(-5.4), Sender = petro },
                    new Message { Text = "Yes, it takes about 2 hours.",
                                  SendDate = DateTime.Now.AddHours(-5.3), Sender = petya },
                    new Message { Text = "Perfect! I will take the bus. I was thinking about a taxi, but it’s expensive.",
                                  SendDate = DateTime.Now.AddHours(-5.2), Sender = petro },
                    new Message { Text = "So, when is your flight?",
                                  SendDate = DateTime.Now.AddHours(-5.1), Sender = petya, RepliedTo = privateChatMessage2 },
                    new Message { Text = "Tuesday morning. I need to leave really early as I must check in three hours before.",
                                  SendDate = DateTime.Now.AddHours(-5), Sender = petro },
                    new Message { Text = "Would you like to take some of my delicious homemade cake for your family?",
                                  SendDate = DateTime.Now.AddHours(-4.9), Sender = petya },
                    new Message { Text = "Thanks, Lucy. But I don’t think I can. The customs officer is usually really strict.",
                                  SendDate = DateTime.Now.AddHours(-4.8), Sender = petro },
                    new Message { Text = "I understand. Have a safe journey and see you when you get back.",
                                  SendDate = DateTime.Now.AddHours(-4.7), Sender = petya },
                    new Message { Text = "Thanks. See you soon!",
                                  SendDate = DateTime.Now.AddHours(-4.6), Sender = petro },
                }
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

            var dotNetGroupMessage1 = new Message
            {
                Text = $"Do you know how to add database to app in general?",
                SendDate = DateTime.Now.AddHours(-10.4),
                Sender = maks
            };

            var dotNetGroup = new Room
            {
                DisplayName = ".NET Devs",
                Users = new List<User> { petro, petya, maks, misha },
                Messages = new List<Message>
                {
                    new Message { Text = $"Chat created by {maks.Name}", SendDate = DateTime.Now.AddHours(-11) },
                    new Message { Text = $"Hi guys!", SendDate = DateTime.Now.AddHours(-10.9), Sender = petro },
                    new Message { Text = $"Hey petro", SendDate = DateTime.Now.AddHours(-10.8), Sender = misha },
                    new Message { Text = $"Can you help me?", SendDate = DateTime.Now.AddHours(-10.7), Sender = petro },
                    new Message { Text = $"Sure, what's wrong?", SendDate = DateTime.Now.AddHours(-10.6), Sender = maks },
                    new Message { Text = $"I'm making chat web app using Signal R and I don't understand how to add " +
                                    $"database to app. To save messages and retrieve them later",
                                    SendDate = DateTime.Now.AddHours(-10.5), Sender = petro },
                    dotNetGroupMessage1,
                    new Message { Text = $"Of course", SendDate = DateTime.Now.AddHours(-10.3),
                                  Sender = petro, RepliedTo = dotNetGroupMessage1 },
                    new Message { Text = $"So add db and then use it in Hub class or in controller",
                                  SendDate = DateTime.Now.AddHours(-10.2), Sender = maks },
                    new Message { Text = $"In second case you should add it as IHubContext<YourHubClass>",
                                  SendDate = DateTime.Now.AddHours(-10.1), Sender = misha },
                    new Message { Text = $"Great, thanks!", SendDate = DateTime.Now, Sender = petro },
                    new Message { Text = $"Hah", SendDate = DateTime.Now.AddHours(-10), Sender = petya,
                                         RepliedTo = dotNetGroupMessage1 },
                    new Message { Text = $"Cool joke", SendDate = DateTime.Now.AddHours(-9.9), Sender = petya,
                                         RepliedTo = dotNetGroupMessage1 },
                }
            };
            context.Rooms.Add(dotNetGroup);
            context.SaveChanges();

            var angularGroupMessage1 = new Message
            {
                Text = $"I thought he uses react, no?",
                SendDate = DateTime.Now.AddHours(-4.8),
                Sender = petya
            };

            var angularGroup = new Room
            {
                DisplayName = "Angular",
                Users = new List<User> { petro, petya, maks },
                Messages = new List<Message>
                {
                    new Message { Text = $"Chat created by {petya.Name}", SendDate = DateTime.Now.AddHours(-5) },
                    new Message { Text = $"Petya, can you add misha?", SendDate = DateTime.Now.AddHours(-4.9), Sender = petro },
                    angularGroupMessage1,
                    new Message { Text = $"He uses both", SendDate = DateTime.Now.AddHours(-4.7), Sender = petro,
                                  RepliedTo = angularGroupMessage1 },
                    new Message { Text = $"Ok", SendDate = DateTime.Now.AddHours(-4.6), Sender = petya },
                    new Message { Text = $"Ok, I'll add him later, have to go.",
                                  SendDate = DateTime.Now.AddHours(-4.5), Sender = petya },
                }
            };
            context.Rooms.Add(angularGroup);
            context.SaveChanges();
        }
    }
}