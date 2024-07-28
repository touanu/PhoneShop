using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class ProductRequestData
    {
        public required Product Product { get; set; }
        public required List<ProductAttribute> Attributes { get; set; }
        public required List<ProductAttributeValue> AttributeValues { get; set; }
    }

    public class ProductRequestGetData
    {
        public int PageNumber { get; set; }
        public string? CreatedDate { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
    }
    public class ProductRequestAddUpdateData
    {
        public int ProductID { get; set; }
        public int? BrandID { get; set; }
        public int? CategoryID { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? Images { get; set; }
        public string? Attributes { get; set; }
    }
}
