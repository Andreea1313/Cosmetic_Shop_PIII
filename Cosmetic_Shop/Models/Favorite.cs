namespace CosmeticShop.Models
{
    public class Favorite
    {
        public int FavoriteId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public bool Pressed { get; set; }

        public User User { get; set; }
        public Product Product { get; set; }
    }
}
