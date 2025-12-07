using Microsoft.AspNetCore.Mvc;
using Cosmetic_Shop.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Cosmetic_Shop.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(int productId)
        {
            var productData = await _productService.PrepareProductDataAsync(User, productId);

            if (productData.Count == 0)
                return NotFound();

            foreach (var entry in productData)
            {
                ViewData[entry.Key] = entry.Value;
            }

            return View();
        }
    }
}
