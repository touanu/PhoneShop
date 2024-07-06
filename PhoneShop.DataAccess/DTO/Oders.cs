using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class Oders
    {
        [Key] public int OderID{ set; get; } 
        public int CustomerID{ set; get; } 
        public decimal TotalAmount{ set; get; }
        public DateTime CreatedDate{ set; get; }
        public string Status{ set; get; }
    }
}
