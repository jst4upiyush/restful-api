using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Infrastructure.Data;
using ProductCatalog.Api.Models;
using ProductCatalog.Api.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCatalog.Api.Controllers
{
    [Route("api/products/{brandId}/catalog")]
    [ApiController]
    public class BrandCatalogController : ControllerBase
    {
        private readonly ICatalogItemRepository _repository;
        private readonly IMapper _mapper;

        public BrandCatalogController(ICatalogItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all the Catalog Items for the Brand
        /// </summary>
        /// <returns>List of Catalog Items</returns>
        [HttpGet(Name = "GetCatalogForBrand")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CatalogItemDto>>> Get(int brandId)
        {
            var catalogItems = await _repository.ListCatalogItemsForBrandAsync(brandId);
            return Ok(_mapper.Map<IEnumerable<CatalogItemDto>>(catalogItems));
        }

        /// <summary>
        /// Add a new Product to the Catalog for the Brand
        /// </summary>
        /// <param name="brandId">Id of Brand for which we want to add a new product</param>
        /// <param name="value">New CatalogItem data</param>
        /// <returns>Newly created Catalog Items</returns>
        [HttpPost(Name = "AddCatalogItemForBrand")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        public async Task<ActionResult<CatalogItemDto>> Post(int brandId, [FromBody] CatalogItemForCreationDto value)
        {
            var catalogEntity = await _repository.AddAsync(new CatalogItem(brandId, value.Name, value.Description, value.Price));

            return CreatedAtRoute(
                "GetCatalogItem",
                new { id = catalogEntity.Id },
                _mapper.Map<CatalogItemDto>(catalogEntity));
        }
    }
}
