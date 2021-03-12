using System.Collections.Generic;

namespace ProductCatalog.Api.Models
{
    public class BrandDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BrandDtoWithLinks: BrandDto
    {
        public List<ResourceLink> Links { get; set; } = new List<ResourceLink>();
    }

    public class BrandForCreationDto
    {
        public string Name { get; set; }
    }

    public class BrandForUpdateDto
    {
        public string Name { get; set; }
    }
}
