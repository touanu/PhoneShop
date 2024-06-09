using PhoneShop.DataAccess.DBContext;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices.IAccountServices;
using PhoneShop.Models;

namespace PhoneShop.DataAccess.Services
{
    public class AccountServices : IAccountServices
    {
        PhoneShopDBcontext db = new PhoneShopDBcontext();
        public async Task<ReturnData> LogIn(LoginRequestData RequestData)
        {
           
            ReturnData result = new ReturnData();
            try
            {
                //Check null
                if (RequestData == null
                    ||RequestData.UserName==null
                    ||RequestData.PassWord==null) 
                {
                    result.ReturnCode = -1;
                    result.ReturnMsg = "Dữ liệu vào không hợp lệ!";
                    return result;
                }
                //check co tai khoan hay khong
                var Account = db.accounts.FirstOrDefault(a => a.UserName == RequestData.UserName);
                var Customer = db.customers.FirstOrDefault(c => c.UserName == RequestData.UserName);
                if (Customer == null
                    ||Account==null) 
                {
                    result.ReturnCode = -2;
                    result.ReturnMsg = "Tài Khoản KHông tồn tại!";
                    return result;
                }
                var user_Admin = db.accounts.FirstOrDefault(u => u.UserName == RequestData.UserName && u.PassWord == RequestData.PassWord);
                var user_Customer = db.customers.FirstOrDefault(u => u.UserName== RequestData.UserName && u.PassWord== RequestData.PassWord);
                if(user_Customer == null
                    || user_Admin == null)
                {
                    result.ReturnCode = -3;
                    result.ReturnMsg = "Sai mật khẩu đăng nhập!";
                    return result;
                }
                result.ReturnCode = 1;
                result.ReturnMsg = "Đăng nhập thành công!";
                return result;
            }
            catch (Exception ex)
            {
                result.ReturnCode = -1;
                result.ReturnMsg = "Hệ thống đang bận:" + ex.Message;
                return result;
            }
        }

        public Task<ReturnData> LogOut(LoginRequestData RequestData)
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnData> SingInCustomer(LoginRequestData RequestData)
        {
            ReturnData result = new ReturnData();
            try
            {
                //check du lieu
                if (RequestData == null
                    || RequestData.UserName == null
                    || RequestData.PassWord == null
                    || RequestData.PhoneNumber == null
                    ||RequestData.FristName==null
                    ||RequestData.LastName==null
                    ||RequestData.ProviceID<=0
                    ||RequestData.DistrictID<=0
                    ||RequestData.WardsID<=0
                    ||RequestData.Email==null
                    ||RequestData.Birthday==null)
                {
                    result.ReturnCode = -1;
                    result.ReturnMsg = "Dữ liệu vào không hợp lệ!";
                    return result;
                }
                //check trung
                var Customer = db.customers.FirstOrDefault(c => c.UserName == RequestData.UserName);
                if (Customer != null)
                {
                    result.ReturnCode = -2;
                    result.ReturnMsg = "Tài Khoản đã tồn tại!";
                    return result;
                }
                var customerReq = new Customers
                {
                    FristName = RequestData.FristName,
                    LastName = RequestData.LastName,
                    UserName = RequestData.UserName,
                    PassWord = RequestData.PassWord,
                    PhoneNumber = RequestData.PhoneNumber,
                    Birthday = RequestData.Birthday,
                    Email = RequestData.Email,
                    ProviceID = RequestData.ProviceID,
                    DistrictID = RequestData.DistrictID,
                    WardsID = RequestData.WardsID,
                };
                db.customers.Add(customerReq);
                db.SaveChangesAsync();
                result.ReturnCode = 1;
                result.ReturnMsg = "Đăng ký thành công!";
                return result;
            }
            catch (Exception ex)
            {
                result.ReturnCode = -1;
                result.ReturnMsg = "Hệ thống đang bận:" + ex.Message;
                return result;
            }
        }
    }
}
