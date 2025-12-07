using CosmeticShop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cosmetic_Shop.Repositories.Interfaces
{
    public interface ICheckoutRepository
    {
        Task AddOrderAsync(Order order);
        Task AddOrderProductsAsync(List<OrderProduct> orderProducts);
        Task SaveChangesAsync();
    }
}
