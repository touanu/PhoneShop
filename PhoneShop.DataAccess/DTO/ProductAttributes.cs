using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class ProductAttributes
    {
        [Key] public int ProductAttributeID { get; set; }
        public int ProductID { get; set; }
        public string AttributesName { get; set; }
    }
}
