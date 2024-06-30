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
    [ApiController]
    [Route("api/[controller]")]
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
      
        [HttpGet("/Account/Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("/Account/Logins")]
        public async Task<JsonResult>Logins(AccountRequestData requestData)
        {
            var returnData = new ReturnData();
            try
            {
                // Bước 1 : khai báo API URL

                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Account/Logins";

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 3 : gọi httpclient bên common để post lên api
                var result = await PhoneShop.Commonlibs.HttpHelper.HttpSenPost(baseurl, url, jsonData);
                var AccReq = new AccountRequestData();
                // Bước 4: nhận dữ liệu về 
                if (!string.IsNullOrEmpty(result))
                {
                    
                    var response = JsonConvert.DeserializeObject<AccountResponseData>(result);
                    AccReq.UserName = response.UserName;
                    AccReq.PassWord = response.PassWord;
                }
                if (requestData == null || string.IsNullOrEmpty(requestData.UserName)
                    || string.IsNullOrEmpty(requestData.PassWord))
                {
                    returnData.ReturnCode = -2;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không được trống!";
                    return Json(returnData);
                }

                // chuyển mật khẩu ở dạng plantext -> mã hóa 
                // 12345 -> SSMG5a92ylYwR3dTvcnMjEn6gU90X1Ob
                var salt = _configuration["Sercurity:SALT_KEY"] ?? "";
                var passwordHash = PhoneShop.Commonlibs.Sercuritys.EncryptPassword(AccReq.PassWord, salt);
                AccReq.PassWord = passwordHash;


                var Req = await _accountServices.AccountLogin(AccReq);

                returnData.ReturnCode = Req.ReturnCode;
                returnData.ReturnMsg = Req.ReturnMsg;
                return Json(returnData);
            }
            catch (Exception ex)
            {

                return Json(ex) ;
            }

            return Json(returnData);
        }
        [HttpGet("/Account/Register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("/Account/Registers")]
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
