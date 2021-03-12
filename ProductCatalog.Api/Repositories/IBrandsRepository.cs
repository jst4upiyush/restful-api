using ProductCatalog.Api.Infrastructure;
using ProductCatalog.Api.Infrastructure.Data;

namespace ProductCatalog.Api.Repositories
{
    public interface IBrandsRepository : IAsyncRepository<CatalogBrand>
    {
    }

    public class BrandsRepository : EfRepository<CatalogBrand>, IBrandsRepository
    {
        public BrandsRepository(CatalogContext dbContext) : base(dbContext) { }

    }
}
