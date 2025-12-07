using Microsoft.AspNetCore.Identity;

namespace CosmeticShop.Models
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? ProfilePicturePath { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
