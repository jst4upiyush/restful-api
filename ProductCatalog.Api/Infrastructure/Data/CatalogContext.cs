using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Infrastructure.Data.EntityConfiguration;

namespace ProductCatalog.Api.Infrastructure.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }

        public DbSet<CatalogItem> Products { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CatalogItemConfiguration());
            builder.ApplyConfiguration(new CatalogBrandConfiguration());
        }
    }
}
