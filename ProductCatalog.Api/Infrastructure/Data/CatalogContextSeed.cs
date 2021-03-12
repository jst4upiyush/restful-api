using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCatalog.Api.Infrastructure.Data
{
    public class CatalogContextSeed
    {
        public static async Task SeedAsync(
            CatalogContext catalogContext,
            ILoggerFactory loggerFactory)
        {
            try
            {
                if (!await catalogContext.Products.AnyAsync())
                {
                    await catalogContext.CatalogBrands.AddRangeAsync(GetPreconfiguredBrands());
                    await catalogContext.SaveChangesAsync();

                    await catalogContext.Products.AddRangeAsync(GetPreconfiguredItems());
                    await catalogContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                loggerFactory.CreateLogger<CatalogContextSeed>().LogError(ex.Message);
                throw;
            }
        }
        static IEnumerable<CatalogBrand> GetPreconfiguredBrands()
        {
            return new List<CatalogBrand>()
            {
                new CatalogBrand("Azure"),
                new CatalogBrand(".NET"),
                new CatalogBrand("Visual Studio"),
                new CatalogBrand("SQL Server"),
                new CatalogBrand("Other")
            };
        }

        static IEnumerable<CatalogItem> GetPreconfiguredItems()
        {
            return new List<CatalogItem>()
            {
                new CatalogItem(2, ".NET Bot Black Sweatshirt", ".NET Bot Black Sweatshirt", 19.5M),
                new CatalogItem(1, ".NET Black & White Mug", ".NET Black & White Mug", 8.50M),
                new CatalogItem(5, "Prism White T-Shirt", "Prism White T-Shirt", 12),
                new CatalogItem(2, ".NET Foundation Sweatshirt", ".NET Foundation Sweatshirt", 12),
                new CatalogItem(5, "Roslyn Red Sheet", "Roslyn Red Sheet", 8.5M),
                new CatalogItem(2, ".NET Blue Sweatshirt", ".NET Blue Sweatshirt", 12),
                new CatalogItem(3, "Roslyn Red T-Shirt", "Roslyn Red T-Shirt",  12),
                new CatalogItem(5, "Kudu Purple Sweatshirt", "Kudu Purple Sweatshirt", 8.5M),
                new CatalogItem(5, "Cup<T> White Mug", "Cup<T> White Mug", 12),
                new CatalogItem(3, ".NET Foundation Sheet", ".NET Foundation Sheet", 12),
                new CatalogItem(2, "Cup<T> Sheet", "Cup<T> Sheet", 8.5M),
                new CatalogItem(5, "Prism White TShirt", "Prism White TShirt", 12)
            };
        }
    }
}
