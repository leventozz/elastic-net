using Elastic.API.DTOs;
using Elastic.API.Model;
using Elastic.API.Repositories;
using Elastic.Clients.Elasticsearch;
using Nest;
using System.Collections.Immutable;
using System.Net;

namespace Elastic.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
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
            var deleteResponse = await _productRepository.DeleteAsync(id);

            if (!deleteResponse.IsValid &&
                deleteResponse.Result is Nest.Result.NotFound)
            {
                return ResponseDto<bool>.Fail("product is not exist", HttpStatusCode.NotFound);
            }

            if (!deleteResponse.IsValid)
            {
                _logger.LogError(deleteResponse.OriginalException, deleteResponse.ServerError.Error.ToString());
                return ResponseDto<bool>.Fail("an error occured", HttpStatusCode.InternalServerError);
            }

            return ResponseDto<bool>.Success(true, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<bool>> DeleteNewAsync(string id)
        {
            var deleteResponse = await _productRepository.DeleteNewAsync(id);

            if (!deleteResponse.IsValidResponse &&
                deleteResponse.Result is Clients.Elasticsearch.Result.NotFound)
            {
                return ResponseDto<bool>.Fail("product is not exist", HttpStatusCode.NotFound);
            }

            if (!deleteResponse.IsValidResponse)
            {
                deleteResponse.TryGetOriginalException(out Exception ex);
                _logger.LogError(ex, deleteResponse.ElasticsearchServerError.Error.ToString());
                return ResponseDto<bool>.Fail("an error occured", HttpStatusCode.InternalServerError);
            }

            return ResponseDto<bool>.Success(true, HttpStatusCode.OK);
        }
    }
}
