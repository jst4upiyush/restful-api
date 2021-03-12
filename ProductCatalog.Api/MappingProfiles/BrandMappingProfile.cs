using AutoMapper;
using ProductCatalog.Api.Infrastructure.Data;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.MappingProfiles
{
    public class BrandMappingProfile : Profile
    {
        public BrandMappingProfile()
        {
            CreateMap<CatalogBrand, BrandDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Brand));

            CreateMap<CatalogBrand, BrandDtoWithLinks>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Brand));
        }
    }
}
