using System.Linq;
using CosmeticShop.Models;

namespace CosmeticShop.Repositories
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly CosmeticShopContext _context;

        public RegisterRepository(CosmeticShopContext context)
        {
            _context = context;
        }

        public int GetMaxUserId()
        {
            return _context.Users.Any() ? _context.Users.Max(u => u.UserId) : 0;
        }
    }
}
