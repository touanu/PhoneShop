using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class Province
    {
        public int ProvinceID { get; set; }
        public string? ProvinceName { get; set; }
        public string? FullName { get; set; }
        public string? CodeName { get; set; }
    }
}
