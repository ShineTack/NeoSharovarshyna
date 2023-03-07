using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeoSharovarshyna.Services.ProductAPI.Migrations
{
    public partial class AddProductsToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryName", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Soup", "Ukrainian traditional soup", "https://upload.wikimedia.org/wikipedia/commons/a/a7/Borscht_served.jpg", "Borsch", 20.0 },
                    { 2, "First cours", "Ukrainian traditional food", "https://f.authenticukraine.com.ua/photo/5410/qNL1g.jpg", "Varenyky", 15.0 },
                    { 3, "Baking", "Ukrainian traditional bake", "https://tasteofartisan.com/wp-content/uploads/2019/09/Ukrainian-Pampushki-Recipe-1.jpg", "Pampushky", 3.0 },
                    { 4, "Sauce", "Ukrainian taditional sauce", "https://petersfoodadventures.com/wp-content/uploads/2018/08/homemade-sour-cream.jpg", "Smetana", 3.0 },
                    { 5, "Snack", "Ukrainian traditional snack", "https://ukrainefood.info/uploads/img/x22_52a5f2f6ce6bb.jpg", "Salo", 5.0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5);
        }
    }
}
