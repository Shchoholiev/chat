using Chat.Application.Interfaces;
using Chat.Application.IRepositories;
using Chat.Infrastructure.EF;
using Chat.Infrastructure.Repository;
using Chat.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var connectionString = @"Server=tcp:shchoholiev-chat-server.database.windows.net," +
                "1433;Initial Catalog=shchoholiev-chat-db;Persist Security Info=False;" +
                "User ID=shchoholiev;Password=27rhefoYvd20;MultipleActiveResultSets=False;" +
                "Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IMessagesRepository, MessagesRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
