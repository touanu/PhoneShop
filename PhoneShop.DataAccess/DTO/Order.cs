using System.ComponentModel.DataAnnotations;

namespace PhoneShop.DataAccess.DTO
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int Status { get; set; }
    }

    public enum OrderStatus
    {
        Completed = 0,
        Unshipped = 1,
        Pending = 2,
        Shipped = 3,
        Canceled = 4,
        Refunded = 5,
        Unexpected = 99
    }
}
