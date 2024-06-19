using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
   public class BrandRequetsData
    {

        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public string IconImages { get; set; }
    }

    public class Brand_DeleteRequestData
    {
        public int BrandID { get; set; }
    }

}
