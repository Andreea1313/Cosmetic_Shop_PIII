using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using CosmeticShop.Models;

namespace CosmeticShop.Services.Auth
{
    public class LoginService : ILoginService
    {
        private readonly SignInManager<User> _signInManager;

        public LoginService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<SignInResult> PasswordSignInAsync(string email, string password)
        {
            return await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);
        }
    }
}
