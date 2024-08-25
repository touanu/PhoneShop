using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class District
    {
        public int DistrictID { get; set; }
        public string? DistrictName { get; set; }
        public string? FullName { get; set; }
        public string? CodeName { get; set; }
        public int ProvinceID { get; set; }
    }
}
