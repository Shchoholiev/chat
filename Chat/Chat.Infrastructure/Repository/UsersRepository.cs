using Chat.Application.IRepositories;
using Chat.Core.Entities.Identity;
using Chat.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationContext _db;
        private readonly DbSet<User> _table;

        public UsersRepository()
        {
            this._db = new ApplicationContext();
            this._table = _db.Set<User>();
        }

        public async Task AddAsync(User user)
        {
            this._db.Attach(user);
            await this._table.AddAsync(user);
            await this.SaveAsync();
        }

        public async Task UpdateAsync(User user)
        {
            this._db.Attach(user);
            this._table.Update(user);
            await this.SaveAsync();
        }

        public async Task DeleteAsync(User user)
        {
            this._table.Remove(user);
            await this.SaveAsync();
        }

        public async Task<User?> GetUserAsync(string email)
        {
            return await this._table.Include(u => u.UserToken)
                                    .Include(u => u.Connections)
                                    .Include(u => u.Rooms)
                                    .FirstOrDefaultAsync(u => u.Email == email);
        }

        private async Task SaveAsync()
        {
            await this._db.SaveChangesAsync();
        }
    }
}
