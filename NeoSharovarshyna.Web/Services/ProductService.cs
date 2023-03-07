using NeoSharovarshyna.Web.Models;
using NeoSharovarshyna.Web.Tools;

namespace NeoSharovarshyna.Web.Services
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IHttpClientFactory factory) : base(factory)
        {

        }

        public async Task<T> CreateProduct<T>(ProductDto product, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.POST,
                ApiData = product,
                ApiUrl = "api/products",
                AccessToken = token
            });
        }

        public async Task<T> DeleteProductById<T>(int productId, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.DELETE,
                ApiUrl = $"api/products/{productId}",
                AccessToken = token
            });
        }

        public async Task<T> GetProductById<T>(int productId, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                ApiUrl = $"api/products/{productId}",
                AccessToken= token
            });
        }

        public async Task<T> GetProducts<T>(string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                ApiUrl = "api/products",
                AccessToken= token
            });
        }

        public async Task<T> UpdateProduct<T>(ProductDto product, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.PUT,
                ApiUrl = "api/products",
                ApiData = product,
                AccessToken = token
            });
        }
    }
}
