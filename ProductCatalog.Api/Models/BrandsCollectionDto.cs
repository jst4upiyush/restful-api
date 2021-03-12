using System.Collections.Generic;

namespace ProductCatalog.Api.Models
{
    public class BrandsCollectionDto
    {
        public IEnumerable<BrandDtoWithLinks> Data { get; set; }
        public List<ResourceLink> Links { get; set; } = new List<ResourceLink>();
    }

    public class ResourceLink
    {
        public ResourceLink() { }

        public ResourceLink(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }

        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
    }
}
