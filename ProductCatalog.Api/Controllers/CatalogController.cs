using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Infrastructure;
using ProductCatalog.Api.Infrastructure.Data;
using ProductCatalog.Api.Models;
using ProductCatalog.Api.Repositories;

namespace ProductCatalog.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogItemRepository _repository;
        private readonly IMapper _mapper;

        public CatalogController(
            ICatalogItemRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the full Catalog
        /// </summary>
        /// <returns>List of Catalog Items</returns>
        [HttpGet(Name = "GetCatalog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CatalogItemDto>>> Get()
        {
            var catalogItems = await _repository.ListAllAsync();
            return Ok(_mapper.Map<IEnumerable<CatalogItemDto>>(catalogItems));
        }

        /// <summary>
        /// Get Catalog item by id
        /// </summary>
        /// <param name="catalogItemId">Catalog Item ID</param>
        /// <returns>The Catalog Items for the given ID</returns>
        [HttpGet("{catalogItemId}", Name = "GetCatalogItem")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CatalogItemDto>> Get(int catalogItemId)
        {
            var catalogItem = await _repository.GetByIdAsync(catalogItemId);
            var brand = catalogItem.CatalogBrand;
            if (catalogItem == null) return NotFound();

            return _mapper.Map<CatalogItemDto>(catalogItem);
        }

        /// <summary>
        /// Update the Catalog item
        /// </summary>
        /// <param name="catalogItemId">id of CatalogItem to up updated</param>
        /// <param name="value">catalog item with updated values</param>
        [HttpPut("{catalogItemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> Put(int catalogItemId, [FromBody] CatalogItemForUpdatesDto value)
        {
            var existingItem = await _repository.GetByIdAsync(catalogItemId);
            if (existingItem == null) return NotFound();

            existingItem.UpdateDetails(value.Name, value.Description, value.Price);
            await _repository.UpdateAsync(existingItem);
            return NoContent();
        }

        /// <summary>
        /// Delete the Catalog Item
        /// </summary>
        /// <param name="catalogItemId">id of CatalogItem to up deleted</param>
        [HttpDelete("{catalogItemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int catalogItemId)
        {
            var itemToDelete = await _repository.GetByIdAsync(catalogItemId);
            if (itemToDelete == null) return NotFound();

            await _repository.DeleteAsync(itemToDelete);

            return NoContent();
        }

        //[HttpPut]
        //[HttpDelete]
        //[ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        //public IActionResult NotAllowed() => StatusCode(StatusCodes.Status405MethodNotAllowed);

        //[HttpPost("{id}")]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> PostToCatalogItem(int id)
        //{
        //    var itemExists = await _repository.ExistsAsync(id);
        //    return itemExists ? Conflict() : NotFound();
        //}
    }
}
