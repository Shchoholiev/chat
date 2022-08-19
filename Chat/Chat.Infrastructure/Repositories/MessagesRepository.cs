using Chat.Application.Interfaces.Repositories;
using Chat.Application.Paging;
using Chat.Core.Entities;
using Chat.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly ApplicationContext _db;

        private readonly DbSet<Message> _table;

        public MessagesRepository(ApplicationContext context)
        {
            this._db = context;
            this._table = _db.Set<Message>();
        }

        public async Task AddAsync(Message message, CancellationToken cancellationToken)
        {
            this._db.Attach(message);
            await this._table.AddAsync(message);
            await this.SaveAsync(cancellationToken);
        }

        public async Task UpdateAsync(Message message, CancellationToken cancellationToken)
        {
            this._db.Attach(message);
            this._table.Update(message);
            await this.SaveAsync(cancellationToken);
        }

        public async Task DeleteAsync(Message message, CancellationToken cancellationToken)
        {
            this._table.Remove(message);
            await this.SaveAsync(cancellationToken);
        }

        public async Task<Message?> GetMessageAsync(int id, CancellationToken cancellationToken)
        {
            return await this._table.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }


        public async Task<Message?> GetFullMessageAsync(int id, CancellationToken cancellationToken)
        {
            return await this._table
                             .Include(m => m.Sender)
                             .Include(m => m.RepliedTo)
                             .Include(m => m.Room)
                             .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<PagedList<Message>> GetPageAsync(PageParameters pageParameters, int roomId, 
            string email, CancellationToken cancellationToken)
        {
            var messages = await this._table
                                     .AsNoTracking()
                                     .Where(m => m.Room.Id == roomId && ((m.Sender.Email == email && m.HideForSender) ? false : true))
                                     .OrderByDescending(m => m.SendDate)
                                     .Include(m => m.Sender)
                                     .Include(m => m.RepliedTo)
                                     .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                     .Take(pageParameters.PageSize)
                                     .ToListAsync(cancellationToken);
            var totalCount = await this._table.Where(m => m.Room.Id == roomId 
                && m.Sender.Email == email && m.HideForSender).CountAsync(cancellationToken);
            messages.Reverse();

            return new PagedList<Message>(messages, pageParameters, totalCount);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await this._db.SaveChangesAsync(cancellationToken);
        }
    }
}
