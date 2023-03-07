using Microsoft.EntityFrameworkCore;
using NeoSharovarshyna.Services.ProductAPI.Models;

namespace NeoSharovarshyna.Services.ProductAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                    new Product()
                    {
                        ProductId = 1,
                        Name = "Borsch",
                        Price = 20,
                        CategoryName = "Soup",
                        Description = "Ukrainian traditional soup",
                        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/a7/Borscht_served.jpg",
                    },
                    new Product()
                    {
                        ProductId = 2,
                        Name = "Varenyky",
                        Price = 15,
                        CategoryName = "First cours",
                        Description = "Ukrainian traditional food",
                        ImageUrl = "https://f.authenticukraine.com.ua/photo/5410/qNL1g.jpg",
                    },
                    new Product()
                    {
                        ProductId = 3,
                        Name = "Pampushky",
                        Price = 3,
                        CategoryName = "Baking",
                        Description = "Ukrainian traditional bake",
                        ImageUrl = "https://tasteofartisan.com/wp-content/uploads/2019/09/Ukrainian-Pampushki-Recipe-1.jpg",
                    },
                    new Product()
                    {
                        ProductId = 4,
                        Name = "Smetana",
                        Price = 3,
                        CategoryName = "Sauce",
                        Description = "Ukrainian taditional sauce",
                        ImageUrl = "https://petersfoodadventures.com/wp-content/uploads/2018/08/homemade-sour-cream.jpg",
                    },
                    new Product()
                    {
                        ProductId = 5,
                        Name = "Salo",
                        Price = 5,
                        CategoryName = "Snack",
                        Description = "Ukrainian traditional snack",
                        ImageUrl = "https://ukrainefood.info/uploads/img/x22_52a5f2f6ce6bb.jpg",
                    }
                );
        }

        public DbSet<Product> Products { get; set; }
    }
}
