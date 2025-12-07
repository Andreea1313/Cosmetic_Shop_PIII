using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Cosmetic_Shop.Services.Interfaces
{
    public interface IProductService
    {
        Task<IDictionary<string, object>> PrepareProductDataAsync(ClaimsPrincipal user, int productId);
    }
}
