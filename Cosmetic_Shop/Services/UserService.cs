using Cosmetic_Shop.Repositories.Interfaces;
using Cosmetic_Shop.Services.Interfaces;
using CosmeticShop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cosmetic_Shop.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IWebHostEnvironment _env;

        public UserService(IUserRepository userRepo, IWebHostEnvironment env)
        {
            _userRepo = userRepo;
            _env = env;
        }

        public async Task<User?> GetCurrentUserAsync(ClaimsPrincipal principal)
        {
            return await _userRepo.GetCurrentUserAsync(principal);
        }

        public async Task<bool> UpdateProfilePictureAsync(ClaimsPrincipal principal, IFormFile profileImage)
        {
            var user = await _userRepo.GetCurrentUserAsync(principal);
            if (user == null || profileImage == null || profileImage.Length == 0)
                return false;

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(profileImage.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(stream);
            }

            user.ProfilePicturePath = "/uploads/" + fileName;
            await _userRepo.UpdateUserAsync(user);

            return true;
        }
    }
}
