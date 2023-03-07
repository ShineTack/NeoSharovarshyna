using AutoMapper;
using AutoMapper.Configuration.Annotations;

namespace NeoSharovarshyna.Services.ProductAPI.Models.Dtos
{
    [AutoMap(typeof(Product), ReverseMap = true)]
    public class ProductDto
    {
        [SourceMember(nameof(Product.ProductId))]
        public int ProductId { get; set; }
        [SourceMember(nameof(Product.Name))]
        public string? Name { get; set; }
        [SourceMember(nameof(Product.Price))]
        public double Price { get; set; }
        [SourceMember(nameof(Product.Description))]
        public string? Description { get; set; }
        [SourceMember(nameof(Product.CategoryName))]
        public string? CategoryName { get; set; }
        [SourceMember(nameof(Product.ImageUrl))]
        public string? ImageUrl { get; set; }
    }
}
