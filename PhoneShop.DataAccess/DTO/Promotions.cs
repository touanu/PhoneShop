using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class Promotions
    {
        public int PromotionID { get; set; }
        public string? PromotionName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quantity { get; set; }
        public int ProductID { get; set; }
        public int CustomerID { get; set; }
        public float PercentageDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
        public int QuantityOneDay { get; set; }
        public decimal MinimumAmount { get; set; }
    }
}
