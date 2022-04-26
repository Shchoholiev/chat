using Chat.Application.IRepositories;
using Chat.Application.Paging;
using Chat.Core.Entities;
using Chat.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Chat.Infrastructure.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase
    {
        private readonly ApplicationContext _db;

        private readonly DbSet<TEntity> _table;

        public GenericRepository()
        {
            this._db = new ApplicationContext();
            this._table = _db.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await this._table.AddAsync(entity);
            await this.SaveAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            this._table.Update(entity);
            await this.SaveAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            this._table.Remove(entity);
            await this.SaveAsync();
        }

        public async Task<TEntity> GetOneAsync(int id)
        {
            return await this._table.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<TEntity> GetOneAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this._table;
            return await this.Include(query, includeProperties).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate, 
                                               params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this._table;
            return await this.Include(query, includeProperties).FirstOrDefaultAsync(predicate);
        }

        public async Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters)
        {
            var entities = await this._table
                                     .AsNoTracking()
                                     .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                     .Take(pageParameters.PageSize)
                                     .ToListAsync();
            var totalCount = await this._table.CountAsync();

            return new PagedList<TEntity>(entities, pageParameters, totalCount);
        }

        public async Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters,
                                                    params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this._table.AsNoTracking()
                                   .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                   .Take(pageParameters.PageSize);
            var entities = await this.Include(query, includeProperties).ToListAsync();
            var totalCount = await this._table.CountAsync();

            return new PagedList<TEntity>(entities, pageParameters, totalCount);
        }


        public async Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters,
                                                           Expression<Func<TEntity, bool>> predicate,
                                                     params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = this._table
                                            .AsNoTracking()
                                            .Where(predicate)
                                            .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                            .Take(pageParameters.PageSize);
            var entities = await this.Include(query, includeProperties).ToListAsync();
            var totalCount = await this._table.CountAsync();

            return new PagedList<TEntity>(entities, pageParameters, totalCount);
        }

        public void Attach(params object[] obj)
        {
            foreach (var o in obj)
            {
                this._db.Attach(o);
            }
        }

        private IQueryable<TEntity> Include(IQueryable<TEntity> query, 
                                            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(query, (current, includeProperty)
                                            => current.Include(includeProperty));
        }

        public async Task SaveAsync()
        {
            await this._db.SaveChangesAsync();
        }
    }
}
