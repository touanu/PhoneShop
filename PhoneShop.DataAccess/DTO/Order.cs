using System.ComponentModel.DataAnnotations;

namespace PhoneShop.DataAccess.DTO
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public string StatusAsString { get { return statusNames[Status].ToString(); } }
        private readonly Dictionary<int, string> statusNames = new()
        {
            { -99, "Không rõ" },
            { -1, "Đã huỷ hàng" },
            { -2, "Yêu cầu trả hàng"},
            { -3, "Đã trả hàng" },
            { 0, "Hoàn thành đơn hàng" },
            { 1, "Chờ lấy hàng" },
            { 2, "Đang vận chuyển" },
            { 3, "Đã giao hàng" },
        };
    }

    public enum OrderStatus
    {
        Unexpected = -99,
        Canceled = -1,
        Refunding = -2,
        Refunded = -3,

        Completed = 0,
        Unshipped = 1,
        Pending = 2,
        Shipped = 3,
    }


}
