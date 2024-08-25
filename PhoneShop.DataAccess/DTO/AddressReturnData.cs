using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class AddressReturnData : ReturnData
    {
        public string? District { get; set; }
        public string? Province { get; set; }
        public string? Ward { get; set; }
    }
}
