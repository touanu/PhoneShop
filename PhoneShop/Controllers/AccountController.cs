using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using PhoneShop.DataAccess.Services;

namespace PhoneShop.Controllers
{
    public class AccountController : Controller
    {
        readonly IConfiguration _configuration;
        private readonly IAccountServices _accountServices;
        public AccountController(IConfiguration configuration, IAccountServices accountServices)
        {
            _accountServices = accountServices;
            _configuration = configuration;

        }

        public IActionResult Index()
        {
            return View();
        }
      
        public IActionResult Login()
        {
            return View();
        }
        public async Task<JsonResult>Logins(AccountRequestData requestData)
        {
            var returnData = new ReturnData();
            try
            {
                // Bước 1: Kiểm tra dữ liệu đầu vào 
                if (requestData == null || string.IsNullOrEmpty(requestData.UserName)
                    || string.IsNullOrEmpty(requestData.PassWord))
                {
                    returnData.ReturnCode = -2;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không được trống!";
                    return Json(returnData);
                }

                // Bước 2 : GỌI API ĐỂ LẤY TOKEN 
                // bƯỚC 2.1 kHAI BÁO URL CỦA API

                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Account/Login";

                //Bước 2.2 convert từ object requestData sang Json để đẩy lên API
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 2.3 dùng httpClient để đưa json lên URL của API
                var result = await PhoneShop.Commonlibs.HttpHelper.HttpSenPost(baseurl, url, jsonData);

                if (string.IsNullOrEmpty(result))
                {
                    returnData.ReturnCode = -2;
                    returnData.ReturnMsg = "Lỗi";
                    return Json(returnData);
                }

                // Bước 2.4 : Convert từ json nhận được thành object 

                var rs = JsonConvert.DeserializeObject<ReturnData>(result);
                if (rs.ReturnCode <= 0)
                {
                    returnData.ReturnCode = rs.ReturnCode;
                    returnData.ReturnMsg = rs.ReturnMsg;
                    return Json(returnData);
                }


                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "Đăng nhập thành công!";
                returnData.token = rs.token;

                //  Session
                return Json(returnData);
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = -969;
                returnData.ReturnMsg = ex.Message;
                return Json(returnData);
            }
        }
        
        public IActionResult Register()
        {
            return View();
        }
        public async Task<JsonResult> Registers(AccountRequestData user)
        {
            var returnData = new ReturnData();
            try
            {
                if (string.IsNullOrEmpty(user.UserName)
                    || string.IsNullOrWhiteSpace(user.UserName)
                    || string.IsNullOrEmpty(user.FristName)
                    || string.IsNullOrEmpty(user.LastName)
                    || string.IsNullOrEmpty(user.PassWord)
                    || string.IsNullOrEmpty(user.PhoneNumber)
                    || string.IsNullOrEmpty(user.Email))
                {
                    returnData.ReturnCode = -2;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không được trống!";
                    return Json(returnData);
                }
                var result = await _accountServices.SingInCustomer(user);
                returnData.ReturnCode = result.ReturnCode;
                returnData.ReturnMsg = result.ReturnMsg;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Json(returnData);
        }
        [HttpGet("/Account/RemoveCustomer")]
        public IActionResult RemoveCustomer()
        {
            return View();
        }
        [HttpDelete("/Account/RemoveCustomers")]
        public async Task<JsonResult> RemoveCustomers(AccountRequestData requestData)
        {
            ReturnData returnData = new ReturnData();
            try
            {
                if (requestData.ID <= 0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ!";
                    return Json(returnData);
                }
                var result = await _accountServices.RemoveCustomerByID(requestData);
                returnData.ReturnCode = result.ReturnCode;
                returnData.ReturnMsg = result.ReturnMsg;
                return Json(returnData);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
