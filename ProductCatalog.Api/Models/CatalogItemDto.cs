namespace ProductCatalog.Api.Models
{
    public class CatalogItemDto
    {
        /// <summary>
        /// Unique Identifier for the Catalog Item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the Catalog Item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of Catalog Item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Price of the Catalog Item
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Unique identifier of Catalog Brand
        /// </summary>
        public int CatalogBrandId { get; set; }

        /// <summary>
        /// Catalog item's Brand name
        /// </summary>
        public string CatalogBrandName { get; set; }
    }
}
