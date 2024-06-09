using PhoneShop.Models;

namespace PhoneShop.DataAccess.DTO
{
    public class ReturnData
    {
        public int ReturnCode { get; set; }
        public string ReturnMsg { get; set; }
    }
    public class ReturnDataReturnAccountsOrCustomers : ReturnData
    {
        public Accounts accounts { get; set; }
        public Customers customers { get; set; }
    }
}
