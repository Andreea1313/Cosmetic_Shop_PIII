using Cosmetic_Shop.Repositories.Interfaces;
using CosmeticShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cosmetic_Shop.Repositories
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly CosmeticShopContext _context;

        public CheckoutRepository(CosmeticShopContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task AddOrderProductsAsync(List<OrderProduct> orderProducts)
        {
            await _context.OrderProducts.AddRangeAsync(orderProducts);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
