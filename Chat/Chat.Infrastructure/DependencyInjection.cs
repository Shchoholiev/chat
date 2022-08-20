using Chat.Application.Interfaces.Repositories;
using Chat.Application.Interfaces.Services.Identity;
using Chat.Infrastructure.EF;
using Chat.Infrastructure.Repositories;
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
            services.AddScoped<IUserManager, UsersService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokensService, TokenService>();

            return services;
        }
    }
}
