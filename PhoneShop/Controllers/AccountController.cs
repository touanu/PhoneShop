using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<JsonResult> Logins(AccountRequestData requestData)
        {
            var returnData = new ReturnData();
            try
            {
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
                var passwordHash = PhoneShop.Commonlibs.Sercuritys.EncryptPassword(requestData.PassWord, salt);
                requestData.PassWord = passwordHash;


                var result = await _accountServices.AccountLogin(requestData);

                returnData.ReturnCode = result.ReturnCode;
                returnData.ReturnMsg = result.ReturnMsg;
                return Json(returnData);
            }
            catch (Exception ex)
            {

                throw;
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
