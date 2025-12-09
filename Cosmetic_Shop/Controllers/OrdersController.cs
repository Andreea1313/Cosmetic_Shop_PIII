using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CosmeticShop.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CosmeticShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly CosmeticShopContext _context;

        public OrdersController(CosmeticShopContext context)
        {
            _context = context;
        
        public async Task<IActionResult> ViewOrders()
        {
            
            var orders = await _context.Orders
                .Include(o => o.PaymentType)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

        
            System.Console.WriteLine($"[DEBUG] Orders loaded: {orders.Count}");
            foreach (var o in orders)
                System.Console.WriteLine($"Order #{o.OrderId} - UserId={o.UserId} - Total={o.TotalAmount}");

            return View("~/Views/Orders/ViewOrders.cshtml", orders);
        }

        
        public async Task<IActionResult> UserOrders(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.PaymentType)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            System.Console.WriteLine($"[DEBUG] Orders for user {userId}: {orders.Count}");
            return View("~/Views/Orders/ViewOrders.cshtml", orders);
        }
    }
}
