using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public  class Category
    {
        [Key]public int CategoryID {get;set;}
        public string CategoryName {get;set;}
        public string IconImages {get;set;}
        public int DisplayStatus {get;set;}
    }
}
