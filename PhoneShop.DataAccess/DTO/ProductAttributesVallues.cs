using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class ProductAttributesVallues
    {
        [Key] public int ProductAttreID { get; set; }
        public int ProductAttributeID { get; set; }
        public string AttributeValluesName { get; set; }
        public decimal Price { get; set; }
        public decimal PriceSale {  get; set; }
        public int Quantity { get; set; }
    }
}
