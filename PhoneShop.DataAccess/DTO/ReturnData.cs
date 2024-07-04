using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Models;

namespace PhoneShop.DataAccess.DTO
{
    public class ReturnData
    {
        public int ReturnCode { get; set; }
        public string ReturnMsg { get; set;}
        public string token { get; set; }
    }
    public class ReturnDataReturnAccount : ReturnData
    {
        public Accounts Accounts { get; set; }
        public Customers customers{ get; set; }
    }
    public class ReturnDataReturnAttributes: ReturnData
    {
        public ProductAttributes attribute { get; set; }
        public ProductAttributesVallues ProductAttributesVallues { get; set; }
    }
    public class BrandInsertReturnData : ReturnData
    {
        public Brands brands { get; set; }
    }

    public class Brand_DeleteReturnData : ReturnData
    {
    }
    public class AttributesReturnData : ReturnData 
    {
       public ProductAttributesVallues vallues { get; set; }
       public ProductAttributes attributes {  get; set; } 
    }

}