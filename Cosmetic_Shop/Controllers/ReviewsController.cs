using Cosmetic_Shop.Services.Interfaces;
using CosmeticShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmetic_Shop.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IReviewService _service;
        private readonly UserManager<User> _userManager;

        public ReviewsController(IReviewService service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = await _service.GetAllAsync();
            return View(reviews);
        }

        public async Task<IActionResult> Details(int id)
        {
            var review = await _service.GetByIdAsync(id);
            if (review == null) return NotFound();

            return View(review);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Review review)
        {
            if (review == null || review.UserId == 0 || string.IsNullOrWhiteSpace(review.Content))
                return BadRequest(new { success = false, error = "Invalid data" });

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.UserId != review.UserId)
                return Forbid();

            var result = await _service.CreateAsync(review);
            return Json(new { success = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews(int productId)
        {
            var reviews = await _service.GetByProductIdAsync(productId);
            return Json(reviews.Select(r => new {
                r.ReviewId,
                r.Content,
                r.UserId,
                Name = r.User?.Name ?? "Unknown user",
                ProfilePicture = r.User?.ProfilePicturePath ?? "/images/user.png"
            }));
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] Review review)
        {
            if (review == null || string.IsNullOrWhiteSpace(review.Content))
                return Json(new { success = false });

            var existing = await _service.GetByIdAsync(id);
            var currentUser = await _userManager.GetUserAsync(User);

            if (existing == null || currentUser == null || existing.UserId != currentUser.UserId)
                return Forbid();

            var success = await _service.EditAsync(id, review.Content);
            return Json(new { success });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _service.GetByIdAsync(id);
            var currentUser = await _userManager.GetUserAsync(User);

            if (review == null || currentUser == null || review.UserId != currentUser.UserId)
                return Forbid();

            var success = await _service.DeleteAsync(id);
            return Json(new { success });
        }
    }
}
