using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class Products
    {
        public int ProductID { get; set; }
        public int BrandID { get; set; }
        public int CategoryID { get; set; }
        public required string ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public required string Images { get; set; }
    }
}
