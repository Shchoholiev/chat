using Chat.Application.Paging;
using Chat.Core.Entities;
using System.Linq.Expressions;

namespace Chat.Application.Interfaces.Repositories
{
    public interface IMessagesRepository
    {
        Task AddAsync(Message message, CancellationToken cancellationToken);

        Task UpdateAsync(Message message, CancellationToken cancellationToken);

        Task DeleteAsync(Message message, CancellationToken cancellationToken);

        Task<Message?> GetMessageAsync(int id, CancellationToken cancellationToken);

        Task<Message?> GetFullMessageAsync(int id, CancellationToken cancellationToken);

        Task<PagedList<Message>> GetPageAsync(PageParameters pageParameters, int roomId, string email, 
            CancellationToken cancellationToken);
    }
}
