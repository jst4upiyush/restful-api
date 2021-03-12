using System.Collections.Generic;

namespace ProductCatalog.Api.Infrastructure.Data
{
    public class CatalogBrand : IEntity
    {
        public int Id { get; private set; }
        public string Brand { get; private set; }

        public IList<CatalogItem> CatalogItems { get; set; }

        public CatalogBrand(string brand) => Brand = brand;

        public void UpdateDetails(string brandName) => Brand = brandName;
    }
}
