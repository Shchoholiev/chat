using Chat.Application.Interfaces.Repositories;
using Chat.Application.Paging;
using Chat.Core.Entities;
using Chat.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Chat.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase
    {
        private readonly ApplicationContext _db;

        private readonly DbSet<TEntity> _table;

        public GenericRepository(ApplicationContext context)
        {
            this._db = context;
            this._table = _db.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await this._table.AddAsync(entity, cancellationToken);
            await this.SaveAsync(cancellationToken);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            this._table.Update(entity);
            await this.SaveAsync(cancellationToken);
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            this._table.Remove(entity);
            await this.SaveAsync(cancellationToken);
        }

        public async Task<TEntity?> GetOneAsync(int id, CancellationToken cancellationToken)
        {
            return await this._table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<TEntity?> GetOneAsync(int id, CancellationToken cancellationToken, 
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this._table;
            return await this.Include(query, includeProperties).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> predicate, 
            CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this._table;
            return await this.Include(query, includeProperties).FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters, 
            CancellationToken cancellationToken)
        {
            var entities = await this._table
                                     .AsNoTracking()
                                     .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                     .Take(pageParameters.PageSize)
                                     .ToListAsync(cancellationToken);
            var totalCount = await this._table.CountAsync(cancellationToken);

            return new PagedList<TEntity>(entities, pageParameters, totalCount);
        }

        public async Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters, 
            CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this._table.AsNoTracking()
                                   .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                   .Take(pageParameters.PageSize);
            var entities = await this.Include(query, includeProperties).ToListAsync(cancellationToken);
            var totalCount = await this._table.CountAsync(cancellationToken);

            return new PagedList<TEntity>(entities, pageParameters, totalCount);
        }


        public async Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters, 
            Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = this._table
                                            .AsNoTracking()
                                            .Where(predicate)
                                            .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                            .Take(pageParameters.PageSize);
            var entities = await this.Include(query, includeProperties).ToListAsync(cancellationToken);
            var totalCount = await this._table.CountAsync(cancellationToken);

            return new PagedList<TEntity>(entities, pageParameters, totalCount);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await this._db.SaveChangesAsync(cancellationToken);
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
    }
}
