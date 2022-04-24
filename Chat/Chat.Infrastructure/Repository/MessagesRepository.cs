using Chat.Application.IRepositories;
using Chat.Application.Paging;
using Chat.Core.Entities;
using Chat.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repository
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly ApplicationContext _db;

        private readonly DbSet<Message> _table;

        public MessagesRepository()
        {
            this._db = new ApplicationContext();
            this._table = _db.Set<Message>();
        }

        public async Task AddAsync(Message message)
        {
            this._db.Attach(message);
            await this._table.AddAsync(message);
            await this.SaveAsync();
        }

        public async Task UpdateAsync(Message message)
        {
            this._db.Attach(message);
            this._table.Update(message);
            await this.SaveAsync();
        }

        public async Task DeleteAsync(Message message)
        {
            this._table.Remove(message);
            await this.SaveAsync();
        }

        public async Task<Message?> GetMessageAsync(int id)
        {
            return await this._table.FirstOrDefaultAsync(m => m.Id == id);
        }


        public async Task<Message?> GetFullMessageAsync(int id)
        {
            return await this._table
                             .Include(m => m.Sender)
                             .Include(m => m.RepliedTo)
                             .Include(m => m.Room)
                             .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetPageAsync(PageParameters pageParameters, int roomId, string email)
        {
            var messages = await this._table
                                     .AsNoTracking()
                                     .Where(m => m.Room.Id == roomId && ((m.Sender.Email == email && m.HideForSender) ? false : true))
                                     .OrderByDescending(m => m.SendDate)
                                     .Include(m => m.Sender)
                                     .Include(m => m.RepliedTo)
                                     .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                     .Take(pageParameters.PageSize)
                                     .ToListAsync();
            var totalCount = await this._table.Where(m => m.Room.Id == roomId 
                && ((m.Sender.Email == email && m.HideForSender) ? false : true)).CountAsync();

            return new PagedList<Message>(messages, pageParameters, totalCount);
        }

        public async Task SaveAsync()
        {
            await this._db.SaveChangesAsync();
        }
    }
}
