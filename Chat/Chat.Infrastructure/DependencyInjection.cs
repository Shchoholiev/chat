using Chat.Application.Interfaces.Repositories;
using Chat.Application.Interfaces.Services;
using Chat.Application.Interfaces.Services.Identity;
using Chat.Infrastructure.EF;
using Chat.Infrastructure.Repositories;
using Chat.Infrastructure.Services;
using Chat.Infrastructure.Services.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var connectionString = @"server=(LocalDb)\MSSQLLocalDB;database=Chat;integrated security=True;
                    MultipleActiveResultSets=True;App=EntityFramework;";

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IMessagesRepository, MessagesRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokensService, TokensService>();
            services.AddScoped<IMessagesService, MessagesService>();
            services.AddScoped<IRoomsService, RoomsService>();
            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}
