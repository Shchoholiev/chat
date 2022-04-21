using Chat.Application.Paging;
using Chat.Core.Entities;
using System.Linq.Expressions;

namespace Chat.Application.IRepositories
{
    public interface IMessagesRepository
    {
        Task AddAsync(Message message);

        Task UpdateAsync(Message message);

        Task DeleteAsync(Message message);

        Task<Message?> GetOneAsync(int id);

        Task<PagedList<Message>> GetPageAsync(PageParameters pageParameters, int roomId, string email);
    }
}
