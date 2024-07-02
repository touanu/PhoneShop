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
        PhonShopDBcontext _dbcontext;
        public AccountServices(PhonShopDBcontext DBContext)
        {
            _dbcontext = DBContext;
        }
        public async Task<ReturnDataReturnAccount> AccountLogin(AccountRequestData requestData)
        {
            var returnData = new ReturnDataReturnAccount();
           try
           {
                var account = _dbcontext.accounts.Where(s => s.UserName == requestData.UserName
                && s.PassWord == requestData.PassWord).FirstOrDefault();
                var customer = _dbcontext.customers.Where(c=>c.UserName == requestData.UserName
                &&c.PassWord == requestData.PassWord).FirstOrDefault();
                if(account==null && customer == null)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "đăng nhập thất bại";
                    return returnData;
                }
            returnData.ReturnCode = 1;
            returnData.Accounts = account;
            returnData.ReturnMsg = "đăng nhập thành công";
            return returnData;
           }
           catch (Exception ex)
           {
              throw;
           }
        }

        public async Task<ReturnDataReturnAccount> AddCustomer(AccountRequestData requestData)
        {
            var returnData = new ReturnDataReturnAccount();
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
                //if(Validation.IsName(requestData.FristName)==false
                //||Validation.IsName(requestData.LastName)==false
                //||Validation.IsNumber(requestData.PhoneNumber)==false
                //||Validation.IsContainHTMLTags(requestData.UserName)==true
                //||Validation.IsContainSpecialCharacters(requestData.UserName)==true
                //||Validation.IsContainHTMLTags(requestData.PassWord)==true
                //||Validation.IsValidEmail(requestData.Email)==false)
                //{
                //    returnData.ReturnCode = -1;
                //    returnData.ReturnMsg = "dữ liệu vào không hợp lệ";
                //    return returnData;
                //}
                //check tài khoan ton tại hay không
                var customer = _dbcontext.customers.Where(c=>c.UserName == requestData.UserName).FirstOrDefault();
                if((customer!=null && customer.CustomerID>0))
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Tên đăng nhập đã tồn tại";
                    return returnData;
                }
                var customerreq = new Customers
                {
                    FirstName = requestData.FristName,
                    LastName = requestData.LastName,
                    UserName = requestData.UserName,
                    PassWord = requestData.PassWord,
                    PhoneNumber = requestData.PhoneNumber,
                    Birthday = requestData.Birthday,
                    Email = requestData.Email,
                    ProvineID = requestData.ProviceID,
                    DistrictID = requestData.DistrictID,
                    WardsID = requestData.WardsID,
                };
                _dbcontext.customers.Add(customerreq);
                _dbcontext.SaveChangesAsync();
                returnData.customers = customerreq;
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "Thêm tài khoản thành công! ";
                return returnData;
            }
            catch (Exception ex)
            {
              throw;
            }
        }

        public async Task<List<Customers>> GetCustomerByUserNameorLastName(AccountRequestData requestData)
        {
            try
            {
                var customers = new List<Customers>();
                if (requestData.UserName!=null
                &&requestData.LastName!=null)
                {
                   return null;
                }
                if (requestData.UserName!=null)
                {
                    foreach(var user in _dbcontext.customers)
                    {
                        if(user.LastName==requestData.UserName)
                        {
                            customers.Add(user);
                        }
                    }
                }
                if(requestData.LastName!=null){
                    var customer = _dbcontext.customers.Where(c=>c.UserName==requestData.UserName).FirstOrDefault();
                    customers.Add(customer);
                }
                return customers;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ReturnData> RemoveCustomerByID(AccountRequestData requestData)
        {
            var returnData = new ReturnData();
            try 
            {
                if(requestData==null
                ||requestData.ID<=0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "dữ liệu vào không hợp lệ";
                    return returnData; 
                }
                Customers customer=new Customers();
                foreach (var c in _dbcontext.customers)
                {
                    if (c.CustomerID==requestData.ID)
                    {
                        customer=c;
                    }
                }
                if(customer==null)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Không có khách hàng có ID bạn vừa nhập";
                    return returnData; 
                }
                _dbcontext.customers.Remove(customer);
                _dbcontext.SaveChangesAsync();
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "xóa khách hàng thành công";
                return returnData;
            }
            catch (Exception ex)
            {
                throw;
            }
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
                var customer = _dbcontext.customers.Where(c=>c.UserName == requestData.UserName).FirstOrDefault();
                if((customer!=null && customer.CustomerID>0))
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Tên đăng nhập đã tồn tại";
                    return returnData;
                }
                var customerreq = new Customers
                {
                    FirstName = requestData.FristName,
                    LastName = requestData.LastName,
                    UserName = requestData.UserName,
                    PassWord = requestData.PassWord,
                    PhoneNumber = requestData.PhoneNumber,
                    Birthday = requestData.Birthday,
                    Email = requestData.Email,
                    ProvineID = requestData.ProviceID,
                    DistrictID = requestData.DistrictID,
                    WardsID = requestData.WardsID,
                };
                _dbcontext.customers.Add(customerreq);
                _dbcontext.SaveChangesAsync();
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "Đăng ký tài khoản thành công! ";
                return returnData;
           }
           catch (Exception ex)
           {
              throw;
           }
        }

        public async Task<ReturnData> UpdateCustomer(AccountRequestData requestData)
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
                var customer = _dbcontext.customers.Where(c=>c.UserName == requestData.UserName).FirstOrDefault();
                if((customer==null && customer.CustomerID>0))
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Tên đăng nhập không tồn tại";
                    return returnData;
                }
                var customerreq = new Customers
                {
                    FirstName = requestData.FristName,
                    LastName = requestData.LastName,
                    UserName = requestData.UserName,
                    PassWord = requestData.PassWord,
                    PhoneNumber = requestData.PhoneNumber,
                    Birthday = requestData.Birthday,
                    Email = requestData.Email,
                    ProvineID = requestData.ProviceID,
                    DistrictID = requestData.DistrictID,
                    WardsID = requestData.WardsID,
                };
                _dbcontext.customers.Update(customerreq);
                _dbcontext.SaveChangesAsync();
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "Cập nhật tài khoản thành công! ";
                return returnData;
           }
           catch (Exception ex)
           {
              throw;
           }

        }



    }
}
