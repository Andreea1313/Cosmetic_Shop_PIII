using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticShop.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string Content { get; set; } = string.Empty;

        public int ProductId { get; set; }
        public int UserId { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }

    }
}
