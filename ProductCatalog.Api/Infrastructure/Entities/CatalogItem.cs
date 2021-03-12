using System;

namespace ProductCatalog.Api.Infrastructure.Data
{
    /// <summary>
    /// Product catalog item with Id, Name, Description and Price
    /// </summary>
    public class CatalogItem : IEntity
    {
        public CatalogItem(
            int catalogBrandId,
            string name,
            string description,
            decimal price)
        {
            CatalogBrandId = catalogBrandId;
            Name = name;
            Description = description;
            Price = price;

            CreatedOn = DateTime.Now;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public DateTime? CreatedOn { get; private set; }
        public DateTime? ModifiedOn { get; private set; }

        public int CatalogBrandId { get; private set; }
        public CatalogBrand CatalogBrand { get; private set; }

        public void UpdateDetails(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;

            ModifiedOn = DateTime.Now;
        }
    }
}
