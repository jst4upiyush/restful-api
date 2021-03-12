using AutoMapper;
using ProductCatalog.Api.Infrastructure.Data;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.MappingProfiles
{
    public class CatalogItemMappingProfile : Profile
    {
        public CatalogItemMappingProfile()
        {
            CreateMap<CatalogItem, CatalogItemDto>()
                .ForMember(
                    dest => dest.CatalogBrandName,
                    opt => opt.MapFrom(src => src.CatalogBrand.Brand));
        }
    }
}
