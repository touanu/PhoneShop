using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LENOVO.source.repos.PhoneShop.PhoneShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _accountServices ;
        public AccountController(IAccountServices accountServices){
            _accountServices = accountServices ;
        }
         public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<JsonRsult> AccountLogin(AccountRequestData requestData)
        {
            var returnData = new ReturnData();
            try
            {
                if (requestData == null || string.IsNullOrEmpty(requestData.email)
                    || string.IsNullOrEmpty(requestData.password))
                {
                    returnData.ReturnCode = -2;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không được trống!";
                    return Json(returnData);
                }

                // chuyển mật khẩu ở dạng plantext -> mã hóa 
                // 12345 -> SSMG5a92ylYwR3dTvcnMjEn6gU90X1Ob
                var salt = _configuration["Sercurity:SALT_KEY"] ?? "";
                var passwordHash = PhoneShop.CommonLibs.Sercurity.EncryptPassword(requestData.password, salt);

                requestData.password = passwordHash;

               var result = await new AccountServices().Account_Login(requestData);
                
               var result = await _accountServices.Account_Login(requestData);

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