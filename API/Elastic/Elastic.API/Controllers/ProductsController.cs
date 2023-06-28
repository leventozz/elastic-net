using Elastic.API.DTOs;
using Elastic.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Elastic.API.Controllers
{
  
    public class ProductsController : BaseController
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("SaveProduct")]
        public async Task<IActionResult> Save(ProductCreateDto request)
        {
            return CreateActionResult(await _productService.SaveAsync(request));
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            return CreateActionResult(await _productService.GetAllAsync());
        }
    }
}
