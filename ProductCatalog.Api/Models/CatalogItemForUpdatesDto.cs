using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Api.Models
{
    public class CatalogItemForUpdatesDto
    {
        /// <summary>
        /// Name of the Catalog Item
        /// </summary>
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Description of Catalog Item
        /// </summary>
        [MaxLength(300)]
        public string Description { get; set; }

        /// <summary>
        /// Price of the Catalog Item
        /// </summary>
        [Range(0.1, 100)]
        public decimal Price { get; set; }
    }
}
