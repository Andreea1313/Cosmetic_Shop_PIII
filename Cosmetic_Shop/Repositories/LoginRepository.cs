using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using CosmeticShop.Models;

namespace CosmeticShop.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly UserManager<User> _userManager;

        public LoginRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
    }
}
