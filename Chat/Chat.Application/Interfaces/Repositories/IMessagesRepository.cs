using Chat.Application.Paging;
using Chat.Core.Entities;
using System.Linq.Expressions;

namespace Chat.Application.Interfaces.Repositories
{
    public interface IMessagesRepository
    {
        Task AddAsync(Message message);

        Task UpdateAsync(Message message);

        Task DeleteAsync(Message message);

        Task<Message?> GetMessageAsync(int id);

        Task<Message?> GetFullMessageAsync(int id);

        Task<PagedList<Message>> GetPageAsync(PageParameters pageParameters, int roomId, string email);
    }
}
