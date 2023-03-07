using AutoMapper;
using NeoSharovarshyna.Services.ProductAPI.DbContexts;
using NeoSharovarshyna.Services.ProductAPI.Models;
using NeoSharovarshyna.Services.ProductAPI.Models.Dtos;

namespace NeoSharovarshyna.Services.ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        private ApplicationDbContext _context;

        public ProductRepository(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            if (product?.ProductId > 0) _context.Products.Update(product);
            else await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.ProductId == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            return _mapper.Map<ProductDto>(_context.Products.FirstOrDefault(p => p.ProductId == id));
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            return _mapper.Map<List<ProductDto>>(_context.Products);
        }
    }
}
