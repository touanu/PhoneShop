using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class AttributesRequestData
    {
        public string AttributesNameValue { get; set; }
        public int ProductAttributeID { get; set; }
        public int ProductID { get; set; }
        public string AttributesName { get; set; }
        public int ProductAttreID { get; set; }
        public string AttributeValluesName { get; set; }
        public decimal Price { get; set; }
        public decimal PriceSale { get; set; }
        public int Quantity { get; set; }
        public string AttributeValuestring { get; set; }
    }
    public class AttributesResponseData
    {
        public int ProductID { get; set; }
        public string ?ProductName { get; set; }
        public string ?AttributesName { get; set; }
        public string ?AttributeValuestring { get; set; }
    }
    public class AttributesResponse
    {
        public int productAttributeID { get; set; }
        public int productID { get; set; }
        public string attributesName { get; set; }
    }
   
}
