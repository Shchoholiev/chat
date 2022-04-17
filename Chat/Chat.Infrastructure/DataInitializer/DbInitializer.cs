using Chat.Infrastructure.EF;

namespace Chat.Infrastructure.DataInitializer
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationContext context)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }
    }
}
