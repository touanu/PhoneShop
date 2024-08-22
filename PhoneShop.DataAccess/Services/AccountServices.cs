using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Commonlibs;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PhoneShop.DataAccess.Services
{
    public class AccountServices : IAccountServices
    {
        PhoneShopDBcontext _dbcontext;
        public AccountServices(PhoneShopDBcontext DBContext)
        {
            _dbcontext = DBContext;
        }
        public async Task<ReturnDataReturnAccount> AccountLogin(AccountRequestData requestData)
        {
            var returnData = new ReturnDataReturnAccount();
           try
           {
                var account = _dbcontext.Account.Where(s => s.UserName == requestData.UserName
                && s.PassWord == requestData.PassWord).FirstOrDefault();
                
                if(account==null)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "đăng nhập thất bại";
                    return returnData;
                }
            returnData.ReturnCode = 1;
            returnData.Account = account;
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
                
                //check tài khoan ton tại hay không
                var customer = _dbcontext.Customer.Where(c=>c.UserName == requestData.UserName).FirstOrDefault();
                if((customer!=null && customer.CustomerID>0))
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Tên đăng nhập đã tồn tại";
                    return returnData;
                }
                var customerreq = new Customer  
                {
                    FirstName = requestData.FristName,
                    LastName = requestData.LastName,
                    UserName = requestData.UserName,
                    PassWord = requestData.PassWord,
                    PhoneNumber = requestData.PhoneNumber,
                    Birthday = requestData.Birthday,
                    Email = requestData.Email,
                    ProvinceID = requestData.ProviceID,
                    DistrictID = requestData.DistrictID,
                    WardID = requestData.WardsID,
                };
                _dbcontext.Customer.Add(customerreq);
                returnData.customer = customerreq;
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "Thêm tài khoản thành công! ";
                return returnData;
            }
            catch (Exception ex)
            {
              throw;
            }
        }

        public async Task<Customer?>  GetCustomerbyID(int id)
        {
            return _dbcontext.Customer.FirstOrDefault(x => x.CustomerID == id);
        }

        public async Task<List<Customer>> GetCustomerByUserNameorLastName(AccountRequestData requestData)
        {
            try
            {
                var customers = new List<Customer>();
                if (requestData.UserName!=null
                &&requestData.LastName!=null)
                {
                   return null;
                }
                if (requestData.UserName!=null)
                {
                    foreach(var user in _dbcontext.Customer)
                    {
                        if(user.LastName==requestData.UserName)
                        {
                            customers.Add(user);
                        }
                    }
                }
                if(requestData.LastName!=null){
                    var customer = _dbcontext.Customer.Where(c=>c.UserName==requestData.UserName).FirstOrDefault();
                    customers.Add(customer);
                }
                return customers;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async  Task<DTO.Functions> GetFunction(string functionCode)
        {
            var model = new DTO.Functions();
            try
            {
            
                model = _dbcontext.Functions.Where(s => s.FunctionCode == functionCode).FirstOrDefault();
                
            }
            catch (Exception)
            {

                throw;
            }
            return model;
        }

        public async Task<GetCustomerReturnData> GetlistCustomer(AccountRequestData requestData)
        {
            var list = new List<Customer>();
            var returnData = new GetCustomerReturnData();
            try
            {
                list = _dbcontext.Customer.ToList();

               
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "lấy dữ liệu thành công!";
                returnData.listcustomer = list;
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = -969;
                returnData.ReturnMsg = "Hệ thống đang bận!" + ex;
                return returnData;
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
                Customer customer=new Customer();
                foreach (var c in _dbcontext.Customer)
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
                _dbcontext.Customer.Remove(customer);
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
                var customer = _dbcontext.Customer.Where(c=>c.UserName == requestData.UserName).FirstOrDefault();
                if((customer!=null && customer.CustomerID>0))
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Tên đăng nhập đã tồn tại";
                    return returnData;
                }
                var customerreq = new Customer
                {
                    FirstName = requestData.FristName,
                    LastName = requestData.LastName,
                    UserName = requestData.UserName,
                    PassWord = requestData.PassWord,
                    PhoneNumber = requestData.PhoneNumber,
                    Birthday = requestData.Birthday,
                    Email = requestData.Email,
                    ProvinceID = requestData.ProviceID,
                    DistrictID = requestData.DistrictID,
                    WardID = requestData.WardsID,
                };
                _dbcontext.Customer.Add(customerreq);
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
                var customer = _dbcontext.Customer.Where(c=>c.UserName == requestData.UserName).FirstOrDefault();
                if((customer==null && customer.CustomerID>0))
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Tên đăng nhập không tồn tại";
                    return returnData;
                }
                var customerreq = new Customer
                {
                    FirstName = requestData.FristName,
                    LastName = requestData.LastName,
                    UserName = requestData.UserName,
                    PassWord = requestData.PassWord,
                    PhoneNumber = requestData.PhoneNumber,
                    Birthday = requestData.Birthday,
                    Email = requestData.Email,
                    ProvinceID = requestData.ProviceID,
                    DistrictID = requestData.DistrictID,
                    WardID = requestData.WardsID,
                };
                _dbcontext.Customer.Update(customerreq);
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "Cập nhật tài khoản thành công! ";
                return returnData;
           }
           catch (Exception ex)
           {
              throw;
           }

        }

        public async Task<User_Permissions> User_PermissionById(int functionId, int AccountID)
        {
            var model = new User_Permissions();

            try
            {
                model = _dbcontext.User_Permissions.Where(s => s.FunctionID == functionId && s.AccountID == AccountID).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }

            return model;
        }
    }
}
