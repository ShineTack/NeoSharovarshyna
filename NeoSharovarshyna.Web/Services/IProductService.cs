using NeoSharovarshyna.Web.Models;

namespace NeoSharovarshyna.Web.Services
{
    public interface IProductService : IBaseService
    {
        Task<T> GetProducts<T>(string token);
        Task<T> GetProductById<T>(int productId, string token);
        Task<T> CreateProduct<T>(ProductDto product, string token);
        Task<T> UpdateProduct<T>(ProductDto product, string token);
        Task<T> DeleteProductById<T>(int productId, string token);
    }
}
