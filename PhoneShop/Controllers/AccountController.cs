using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public AccountController(IConfiguration configuration,IAccountServices accountServices)
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

        public async Task<JsonResult> AccountLogin(AccountRequestData requestData)
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
            }
            catch (Exception ex)
            {

                throw;
            }

            return Json(returnData);
        }
    }
}
