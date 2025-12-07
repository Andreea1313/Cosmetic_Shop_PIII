using Cosmetic_Shop.Repositories.Interfaces;
using Cosmetic_Shop.Services.Interfaces;
using CosmeticShop.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cosmetic_Shop.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly UserManager<User> _userManager;

        public ProductService(IProductRepository productRepo, UserManager<User> userManager)
        {
            _productRepo = productRepo;
            _userManager = userManager;
        }

        public async Task<IDictionary<string, object>> PrepareProductDataAsync(ClaimsPrincipal userPrincipal, int productId)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            var product = await _productRepo.GetProductByIdAsync(productId);

            if (product == null)
                return new Dictionary<string, object>(); 

            return new Dictionary<string, object>
            {
                { "UserId", user?.UserId ?? 0 },
                { "ProductId", product.ProductId },
                { "Image", product.ImageUrl },
                { "Name", product.Name },
                { "Description", product.Description },
                { "Price", product.Price }
            };
        }
    }
}
