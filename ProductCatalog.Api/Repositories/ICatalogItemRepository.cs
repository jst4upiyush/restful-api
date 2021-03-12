using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Infrastructure;
using ProductCatalog.Api.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCatalog.Api.Repositories
{
    public interface ICatalogItemRepository : IAsyncRepository<CatalogItem>
    {
        Task<IReadOnlyList<CatalogItem>> ListCatalogItemsForBrandAsync(
            int brandId,
            CancellationToken cancellationToken = default);
    }

    public class CatalogItemRepository : EfRepository<CatalogItem>, ICatalogItemRepository
    {
        public CatalogItemRepository(CatalogContext dbContext) : base(dbContext) { }

        public async Task<IReadOnlyList<CatalogItem>> ListCatalogItemsForBrandAsync(
            int brandId,
            CancellationToken cancellationToken = default)
            => await GetQueryable()
                .Where(p => p.CatalogBrandId == brandId)
                .ToListAsync(cancellationToken: cancellationToken);

        protected override IQueryable<CatalogItem> GetQueryable()
            => _dbContext.Set<CatalogItem>().Include(p => p.CatalogBrand);
    }
}
