using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class CategoryRequestData
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string IconImages { get; set; }
        public int DisplayStatus { get; set; }
    }
}
