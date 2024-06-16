using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Commonlibs;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using PhoneShop.Models;

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

        public Task<ReturnData> AddCustomer(AccountRequestData requestData)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnData> GetCustomer(AccountRequestData requestData)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnData> RemoveCustomer(AccountRequestData requestData)
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnData> SingInCustomer(AccountRequestData requestData)
        {
           var returnData = new ReturnData();
           try
           {
                // check du lieu vao
                if(requestData==null
                ||requestData.FristName==null
                ||requestData.LastName==null
                ||requestData.UserName==null
                ||requestData.PassWord==null
                ||requestData.PhoneNumber==null
                ||requestData.Email==null
                ||requestData.Birthday==null
                ||requestData.ProviceID<=0
                ||requestData.DistrictID<=0
                ||requestData.WardsID<=0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg="dữ liệu vào không hợp lệ";
                    return returnData;    
                }
                //check ten dung dinh dang
                if(Validation.IsName(requestData.FristName)==false
                ||Validation.IsName(requestData.LastName)==false
                ||Validation.IsNumber(requestData.PhoneNumber)==false
                ||Validation.IsContainHTMLTags(requestData.UserName)==true
                ||Validation.IsContainSpecialCharacters(requestData.UserName)==true
                ||Validation.IsContainHTMLTags(requestData.PassWord)==true
                ||Validation.IsValidEmail(requestData.Email)==false)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "dữ liệu vào không hợp lệ";
                    return returnData;
                }
                //check tài khoan ton tại hay không
                var customer = dbcontext.customers.Where(c=>c.UserName == requestData.UserName).FirstOrDefault();
                if((customer!=null && customer.CustomerID>0))
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Tên đăng nhập đã tồn tại";
                    return returnData;
                }
                var customerreq = new Customers
                {
                    FristName = requestData.FristName,
                    LastName = requestData.LastName,
                    UserName = requestData.UserName,
                    PassWord = requestData.PassWord,
                    PhoneNumber = requestData.PhoneNumber,
                    Birthday = requestData.Birthday,
                    Email = requestData.Email,
                    ProviceID = requestData.ProviceID,
                    DistrictID = requestData.DistrictID,
                    WardsID = requestData.WardsID,
                };
                dbcontext.customers.Add(customerreq);
                dbcontext.SaveChangesAsync();
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "Đăng ký tài khoản thành công! ";
                return returnData;
           }
           catch (Exception ex)
           {
              throw;
           }
        }

        public Task<ReturnData> UpdateCustomer(AccountRequestData requestData)
        {
            throw new NotImplementedException();
        }
    }
}
