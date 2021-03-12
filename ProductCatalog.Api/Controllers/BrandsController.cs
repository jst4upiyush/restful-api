using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ProductCatalog.Api.Infrastructure;
using ProductCatalog.Api.Infrastructure.Data;
using ProductCatalog.Api.Models;
using ProductCatalog.Api.OpenApiHelpers;
using ProductCatalog.Api.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Api.Controllers
{
    [Route("api/brands")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IAsyncRepository<CatalogBrand> _repository;
        private readonly IMapper _mapper;

        public BrandsController(
            IAsyncRepository<CatalogBrand> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get All the Brands
        /// </summary>
        /// <returns>List of Brands</returns>
        [HttpGet(Name = "GetBrands")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AlsoProduces(StatusCodes.Status200OK, "application/vnd.productcatalog.catalogbrand.hateoas+json", typeof(BrandsCollectionDto))]
        [Produces("application/json", "application/vnd.productcatalog.catalogbrand+json", Type = typeof(IEnumerable<BrandDto>))]
        public async Task<ActionResult<BrandsCollectionDto>> Get()
        {
            var brandsFromDb = await _repository.ListAllAsync();
            dynamic brandsDto = GetMediaSubTypeWithoutSuffix() switch
            {
                "vnd.productcatalog.catalogbrand.hateoas" => MapBrandCollectionDtoWithLinks(brandsFromDb),
                _ => _mapper.Map<IEnumerable<BrandDto>>(brandsFromDb)
            };

            return Ok(brandsDto);
        }

        /// <summary>
        /// Get Brand by ID
        /// </summary>
        /// <returns>List of Brands</returns>
        [HttpGet("{brandId}", Name = "GetBrandById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AlsoProduces(StatusCodes.Status200OK, "application/vnd.productcatalog.catalogbrand.hateoas+json", typeof(BrandDtoWithLinks))]
        [Produces("application/json", "application/vnd.productcatalog.catalogbrand+json", Type = typeof(BrandDto))]
        public async Task<IActionResult> Get(int brandId)
        {
            var brandEntity = await _repository.GetByIdAsync(brandId);
            if (brandEntity == null) return NotFound();

            dynamic brandDto = GetMediaSubTypeWithoutSuffix() switch
            {
                "vnd.productcatalog.catalogbrand.hateoas" => MapBrandDtoWithLinks(brandEntity),
                _ => _mapper.Map<BrandDto>(brandEntity)
            };

            return Ok(brandDto);
        }

        /// <summary>
        /// Add a new Brand
        /// </summary>
        /// <returns>Newly created Brand</returns>
        [HttpPost(Name = "CreateBrand")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        [AlsoProduces(StatusCodes.Status200OK, "application/vnd.productcatalog.catalogbrand.hateoas+json", typeof(BrandDtoWithLinks))]
        [Produces("application/json", "application/vnd.productcatalog.catalogbrand+json", Type = typeof(BrandDto))]
        public async Task<ActionResult> Post([FromBody] BrandForCreationDto value)
        {
            var brandEntity = await _repository.AddAsync(new CatalogBrand(value.Name));

            dynamic brandCreationResponse = GetMediaSubTypeWithoutSuffix() switch
            {
                "vnd.productcatalog.catalogbrand.hateoas" => MapBrandDtoWithLinks(brandEntity),
                _ => _mapper.Map<BrandDto>(brandEntity)
            };

            return CreatedAtRoute(
                "GetBrandById",
                new { brandId = brandEntity.Id },
                brandCreationResponse);
        }

        /// <summary>
        /// Update the Brand
        /// </summary>
        /// <param name="brandId">brandId </param>
        /// <param name="value">target brand state</param>
        [HttpPut("{brandId}", Name = "UpdateBrand")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> UpdateBrand(int brandId, [FromBody] BrandForUpdateDto value)
        {
            var existingItem = await _repository.GetByIdAsync(brandId);
            if (existingItem == null) return NotFound();

            existingItem.UpdateDetails(value.Name);
            await _repository.UpdateAsync(existingItem);

            return NoContent();
        }

        /// <summary>
        /// Delete the given Brand along with all its Catalog
        /// </summary>
        /// <param name="brandId">brand Id</param>
        [HttpDelete("{brandId}", Name = "DeleteBrand")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBrand(int brandId)
        {
            var itemToDelete = await _repository.GetByIdAsync(brandId);
            if (itemToDelete == null) return NotFound();

            await _repository.DeleteAsync(itemToDelete);
            return NoContent();
        }

        private List<ResourceLink> CreateLinksForBrand(int brandId)
            => new List<ResourceLink> {
                new ResourceLink(Url.Link("GetBrandById", new { brandId }), "self", "GET"),
                new ResourceLink(Url.Link("GetCatalogForBrand", new { brandId }), "get_brand_catalog", "GET"),
                new ResourceLink(Url.Link("AddCatalogItemForBrand", new { brandId }), "add_brand_catalog_item", "POST"),
                new ResourceLink(Url.Link("UpdateBrand", new { brandId }), "update_brand", "PUT"),
                new ResourceLink(Url.Link("DeleteBrand", new { brandId }), "delete_brand", "DELETE")
            };
        private List<ResourceLink> CreateLinksForBrandCollection()
            => new List<ResourceLink> {
                new ResourceLink(Url.Link("GetBrands", new { }), "self", "GET"),
                new ResourceLink(Url.Link("CreateBrand", new { }), "create_brand", "POST")
            };

        private BrandDtoWithLinks MapBrandDtoWithLinks(CatalogBrand brandEntity)
        {
            var brandDtoWithLinks = _mapper.Map<BrandDtoWithLinks>(brandEntity);
            brandDtoWithLinks.Links = CreateLinksForBrand(brandEntity.Id);
            return brandDtoWithLinks;
        }

        private BrandsCollectionDto MapBrandCollectionDtoWithLinks(IEnumerable<CatalogBrand> brandEntities)
        {
            var brandsDtos = _mapper.Map<List<BrandDtoWithLinks>>(brandEntities);
            brandsDtos.ForEach(b => b.Links = CreateLinksForBrand(b.Id));
            return new BrandsCollectionDto { Data = brandsDtos, Links = CreateLinksForBrandCollection() };
        }


        private string GetMediaSubTypeWithoutSuffix()
        {
            var acceptHeader = Request.Headers["Accept"].FirstOrDefault();
            MediaTypeHeaderValue.TryParse(acceptHeader, out var parsedMediaType);
            return parsedMediaType.SubTypeWithoutSuffix.ToString();
        }
    }
}
