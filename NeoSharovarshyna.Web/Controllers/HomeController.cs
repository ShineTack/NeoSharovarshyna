using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeoSharovarshyna.Web.Models;
using NeoSharovarshyna.Web.Services;
using NeoSharovarshyna.Web.Tools;
using Newtonsoft.Json;
using System.Diagnostics;

namespace NeoSharovarshyna.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _productService.GetProducts<ResponseDto>("");
            return View(JsonConvert.DeserializeObject<List<ProductDto>>(response?.Data?.ToString() ?? ""));
        }

        [Authorize]
        public async Task<IActionResult> ProductDetail(int productId)
        {
            var response = await _productService.GetProductById<ResponseDto>(productId, "");
            var productDto = JsonConvert.DeserializeObject<ProductDto>(response?.Data?.ToString() ?? "");
            if(productDto == default(ProductDto)) return RedirectToAction("Index");
            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction("Index");
        }
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}