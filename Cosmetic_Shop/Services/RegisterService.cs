using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CosmeticShop.Models;
using CosmeticShop.Repositories;
using Microsoft.AspNetCore.Identity;

namespace CosmeticShop.Services.Auth
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly IRegisterRepository _userRepository;

        public RegisterService(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            IRegisterRepository userRepository)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<User>)userStore;
            _userRepository = userRepository;
        }

        public async Task<(IdentityResult Result, User User)> RegisterUserAsync(InputModel input)
        {
            var user = new User();

            await _userStore.SetUserNameAsync(user, input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, input.Email, CancellationToken.None);

            var maxId = _userRepository.GetMaxUserId();
            user.UserId = maxId + 1;

            var result = await _userManager.CreateAsync(user, input.Password);
            return (result, user);
        }
    }
}
