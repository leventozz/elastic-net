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
        public async Task<IActionResult> SaveProduct(ProductCreateDto request)
        {
            return CreateActionResult(await _productService.SaveAsync(request));
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(ProductUpdateDto request)
        {
            return CreateActionResult(await _productService.UpdateAsync(request));
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            return CreateActionResult(await _productService.DeleteAsync(id));
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            return CreateActionResult(await _productService.GetAllAsync());
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return CreateActionResult(await _productService.GetByIdAsync(id));
        }
    }
}
