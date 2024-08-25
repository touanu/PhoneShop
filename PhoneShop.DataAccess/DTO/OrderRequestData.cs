using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class OrderRequestData
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int TotalAmount { get; set; }
        public int Status { get; set; }
        public string? Details { get; set; }
    }
    public class OrderGetRequestData
    {
        public int? OrderID { get; set; }
        public int? CustomerID { get; set; }
        public int? Status { get; set; }
        public int PageNumber { get; set; }
        public string? CreatedDate { get; set; }
    }
    public class OrderDetailGetRequestData
    {
        public int? OrderID {  set; get; }
    }
}
