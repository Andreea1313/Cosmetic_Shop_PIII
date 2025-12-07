using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using CosmeticShop.Models;

namespace CosmeticShop.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;

        public IndexModel(UserManager<User> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        [BindProperty]
        public InputModel Input { get; set; } = null!;

        public User AppUser { get; set; } = null!;

        public string? StatusMessage { get; set; }

        public class InputModel
        {
            public string? Name { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Address { get; set; }
            public string? Gender { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found");

            AppUser = user;
            Input = new InputModel
            {
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Gender = user.Gender
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            user.Name = Input.Name;
            user.PhoneNumber = Input.PhoneNumber;
            user.Address = Input.Address;
            user.Gender = Input.Gender;

            await _userManager.UpdateAsync(user);
            StatusMessage = "Profile updated successfully!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUploadPictureAsync(IFormFile ProfileImage)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || ProfileImage == null || ProfileImage.Length == 0)
                return RedirectToPage();

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(ProfileImage.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ProfileImage.CopyToAsync(stream);
            }

            user.ProfilePicturePath = "/uploads/" + fileName;
            await _userManager.UpdateAsync(user);

            return RedirectToPage();
        }
    }
}
