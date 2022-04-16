using Chat.Core.Entities.Identity;

namespace Chat.Application.IRepositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(User user);

        Task<User?> GetUserAsync(string email);
    }
}
