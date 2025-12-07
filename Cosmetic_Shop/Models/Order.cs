namespace CosmeticShop.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZIPCode { get; set; }
        public string PhoneNumber { get; set; }
        public int PaymentTypeId { get; set; }

        public User User { get; set; }
        public PaymentType PaymentType { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    }
}
