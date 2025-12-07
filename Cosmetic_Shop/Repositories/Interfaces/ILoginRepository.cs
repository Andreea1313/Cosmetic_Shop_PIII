using CosmeticShop.Models;

namespace CosmeticShop.Repositories
{
    public interface ILoginRepository
    {
        Task<User> FindByEmailAsync(string email);
    }
}
