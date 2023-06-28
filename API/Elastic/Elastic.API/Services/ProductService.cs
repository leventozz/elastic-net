using Elastic.API.DTOs;
using Elastic.API.Model;
using Elastic.API.Repositories;
using System.Collections.Immutable;
using System.Net;

namespace Elastic.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
        {
            var response = await _productRepository.SaveAsync(request.CreateProduct());

            if (response is null)
                return ResponseDto<ProductDto>.Fail("an exception occured", HttpStatusCode.InternalServerError);

            return ResponseDto<ProductDto>.Success(response.CreateDto(), HttpStatusCode.Created);
        }

        public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            var productListDto = products.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.Stock,
                new ProductFeatureDto(x.Feature.Width, x.Feature.Height, x.Feature.Color.ToString()))).ToList();

            return ResponseDto<List<ProductDto>>.Success(productListDto, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
                return ResponseDto<ProductDto>.Fail("an exception occured", HttpStatusCode.NotFound);

            return ResponseDto<ProductDto>.Success(product.CreateDto(), HttpStatusCode.OK);
        }

        public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto updateProduct)
        {
            var result = await _productRepository.UpdateAsync(updateProduct);

            if(result is false)
                return ResponseDto<bool>.Fail("an exception occured", HttpStatusCode.NotFound);

            return ResponseDto<bool>.Success(true, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string id)
        {
            var result = await _productRepository.DeleteAsync(id);

            if (result is false)
                return ResponseDto<bool>.Fail("an exception occured", HttpStatusCode.NotFound);

            return ResponseDto<bool>.Success(true, HttpStatusCode.OK);
        }
    }
}
