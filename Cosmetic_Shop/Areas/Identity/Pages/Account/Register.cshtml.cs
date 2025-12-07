using CosmeticShop.Models;
using CosmeticShop.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cosmetic_Shop.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IRegisterService _registerService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<RegisterModel> _logger;
        public string ReturnUrl { get; set; }
        public RegisterModel(
            IRegisterService registerService,
            SignInManager<User> signInManager,
             UserManager<User> userManager,
            ILogger<RegisterModel> logger)
        {
            _registerService = registerService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            var (result, user) = await _registerService.RegisterUserAsync(Input);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
        
                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation", new { email = user.Email });
                }
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(ReturnUrl);
            }

            return Page();
        }
    }
}
public class InputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
}
