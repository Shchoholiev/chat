using Chat.Application.Paging;
using System.Linq.Expressions;

namespace Chat.Application.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity item);

        Task UpdateAsync(TEntity item);

        Task DeleteAsync(TEntity item);

        void Attach(params object[] obj);

        Task<TEntity> GetOneAsync(int id);

        Task<TEntity> GetOneAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters);

        Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters,
                                              params Expression<Func<TEntity, object>>[] includeProperties);

        Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters, 
                                              Expression<Func<TEntity, bool>> predicate,
                                              params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
