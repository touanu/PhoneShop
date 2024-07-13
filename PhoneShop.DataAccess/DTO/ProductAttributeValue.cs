using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class ProductAttributeValue
    {
        [Key] public int AttributeValueID { get; set; }
        public int ProductAttributeID { get; set; }
        public string AttributeValuesName { get; set; }
        public decimal Price { get; set; }
        public decimal PriceSale {  get; set; }
        public int Quantity { get; set; }
    }
}
