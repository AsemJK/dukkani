using CatalogApi.Application.DTOs;
using CatalogApi.Application.DTOs.Conversions;
using CatalogApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProduct productInterface) : ControllerBase
    {
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> List()
        {
            var products = await productInterface.GetAllAsync();
            if (!products.Any())
                return NotFound("No products found");
            var (_, lov) = ProductConversion.FromEntity(null, products);
            return lov.Any() ? Ok(lov) : NotFound("No Products");
        }

    }
}
