using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class Ward
    {
        public int WardID { get; set; }
        public string? WardName { get; set; }
        public string? FullName { get; set; }
        public string? CodeName { get; set; }
        public int DistrictID { get; set; }
    }
}
