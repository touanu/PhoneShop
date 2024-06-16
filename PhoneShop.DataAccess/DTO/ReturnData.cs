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
    }
    public class ReturnDataReturnCustomer : ReturnData
    {
        public Customers customers{ get; set; }
    }
}