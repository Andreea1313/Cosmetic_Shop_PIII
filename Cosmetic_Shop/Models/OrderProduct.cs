namespace CosmeticShop.Models
{
    public class OrderProduct
    {
        public int OrderProductId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtOrder { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
