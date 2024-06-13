using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;

namespace PhoneShop.DataAccess.Services
{
    public class AccountServices : IAccountServices
    {
        PhonShopDBcontext dbcontext;
        public async Task<ReturnData> AccountLogin(AccountRequestData requestData)
        {
            var returnData = new ReturnData();
           try
           {
                var account = dbcontext.accounts.Where(s => s.Email == requestData.UserName
                && s.PassWord == requestData.PassWord).FirstOrDefault();
                var customer = dbcontext.customers.Where(c=>c.UserName == requestData.UserName
                &&c.PassWord == requestData.PassWord).FirstOrDefault();
                if((account==null && account.AccountID <= 0)
                ||(customer==null && customer.CustomerID<=0))
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "đăng nhập thất bại";
                    return returnData;
                }
            returnData.ReturnCode = 1;
            returnData.ReturnMsg = "đăng nhập thành công";
            return returnData;
           }
           catch (Exception ex)
           {
              throw;
           }
        }
    }
}
