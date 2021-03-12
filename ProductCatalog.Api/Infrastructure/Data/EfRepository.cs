using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCatalog.Api.Infrastructure.Data
{
    public class EfRepository<T> : IAsyncRepository<T> where T : class, IEntity
    {
        protected readonly CatalogContext _dbContext;
        protected virtual IQueryable<T> GetQueryable() => _dbContext.Set<T>();

        public EfRepository(CatalogContext dbContext) => _dbContext = dbContext;

        public virtual async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await GetQueryable().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
            => await GetQueryable().AnyAsync(entity => entity.Id == id, cancellationToken);

        public virtual async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default) 
            => await GetQueryable().ToListAsync(cancellationToken);

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
