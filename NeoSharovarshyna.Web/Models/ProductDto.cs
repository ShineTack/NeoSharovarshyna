using System.ComponentModel.DataAnnotations;

namespace NeoSharovarshyna.Web.Models;

public class ProductDto
{
    public ProductDto()
    {
        Count = 1;
    }

    [Required]
    public int ProductId { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    [Range(1, 1000)]
    public double Price { get; set; }
    [Required]
    public string? Description { get; set; }
    [Required]
    public string? CategoryName { get; set; }
    [Required]
    [Url]
    public string? ImageUrl { get; set; }
    [Range(1, 100)]
    public int Count { get; set; }
}
