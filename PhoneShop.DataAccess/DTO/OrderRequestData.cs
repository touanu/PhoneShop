using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class OrderRequestData
    {
        public required Orders Order { get; set; }
        public required List<OrderDetails> Details { get; set; }
    }
}
