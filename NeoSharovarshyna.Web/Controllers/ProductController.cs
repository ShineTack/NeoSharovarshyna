using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NeoSharovarshyna.Web.Models;
using NeoSharovarshyna.Web.Services;
using NeoSharovarshyna.Web.Tools;
using Newtonsoft.Json;

namespace NeoSharovarshyna.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetProducts<ResponseDto>(token);
            var products = JsonConvert.DeserializeObject<List<ProductDto>>(response?.Data?.ToString() ?? string.Empty);
            return View(products);
        }

        public async Task<IActionResult> CreateProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)
        {
            if(!ModelState.IsValid) return View(productDto);
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.CreateProduct<ResponseDto>(productDto, token);
            if(response != null && response.IsSuccess) {
                return RedirectToAction(nameof(ProductIndex));
            }
            return View(response);
        }

        public async Task<IActionResult> EditProduct(int productId)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetProductById<ResponseDto>(productId, token);
            if (response != null && response.IsSuccess) 
                return View(JsonConvert.DeserializeObject<ProductDto>(response?.Data?.ToString() ?? string.Empty));
            return RedirectToAction(nameof(ProductIndex));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid) return View(productDto);
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.UpdateProduct<ResponseDto>(productDto, token);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
            return View(response);
        }

        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetProductById<ResponseDto>(productId, token);
            if (response != null && response.IsSuccess)
                return View(JsonConvert.DeserializeObject<ProductDto>(response?.Data?.ToString() ?? string.Empty));
            return RedirectToAction(nameof(ProductIndex));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid) return View(productDto);
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.DeleteProductById<ResponseDto>(productDto.ProductId, token);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
            return View(response);
        }
    }
}