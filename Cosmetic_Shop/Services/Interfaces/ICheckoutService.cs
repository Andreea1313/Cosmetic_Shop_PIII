using CosmeticShop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cosmetic_Shop.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<Order> SubmitOrderAsync(Order model, int userId);
    }
}
