using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CosmeticShop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CosmeticShop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ManageProductsController : Controller
    {
        private readonly CosmeticShopContext _context;

        public ManageProductsController(CosmeticShopContext context)
        {
            _context = context;
        }

        private static readonly List<string> Categories = new()
        {
            "hair",
            "skincare",
            "makeup",
            "bodycare",
            "nails"
        };

        private void LoadCategories()
        {
            ViewBag.Categories = Categories;
        }

        public async Task<IActionResult> Index()
        {
            LoadCategories();
            var products = await _context.Products.ToListAsync();
            return View("~/Views/Home/ManageProducts.cshtml", products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Product product)
        {
            if (!ModelState.IsValid)
            {
                LoadCategories();
                var all = await _context.Products.ToListAsync();
                return View("~/Views/Home/ManageProducts.cshtml", all);
            }

            if (product.ProductId == 0)
                _context.Products.Add(product);
            else
                _context.Products.Update(product);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var prod = await _context.Products.FindAsync(id);
            if (prod != null)
            {
                _context.Products.Remove(prod);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
