using Microsoft.AspNetCore.Mvc;
using PhoneShop.DataAccess.DBContext;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices.IAccountServices;

namespace PhoneShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountServices _accountServices ;
        public AccountController(IAccountServices accountServices)
        {
            _accountServices = accountServices ;
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult>Login(LoginRequestData requestData)
        {
            var rs = new ReturnData();
            try
            {            
                if (requestData == null
                    || string.IsNullOrEmpty(requestData.UserName)
                    || string.IsNullOrEmpty(requestData.PassWord)
                    )
                {
                    rs.ReturnCode = -1;
                    rs.ReturnMsg = "Dữ liệu không được trống";
                    return Json(rs);
                }
                var model = await new PhoneShop.DataAccess.Services.AccountServices().LogIn(requestData);
                rs.ReturnCode = model.ReturnCode;
                rs.ReturnMsg = model.ReturnMsg;
            }   
            catch (Exception ex)
            {
                throw;
            }
            return Json(rs);
        }
        [HttpGet]
        public ActionResult SingInCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SingInCustomer(LoginRequestData requestData)
        {
            var rs = new ReturnData();
            try
            {
                if (requestData == null
                    || string.IsNullOrEmpty(requestData.UserName)
                    || string.IsNullOrEmpty(requestData.PassWord)
                    || string.IsNullOrEmpty(requestData.Email)
                    || string.IsNullOrEmpty(requestData.FristName)
                    || string.IsNullOrEmpty(requestData.LastName)
                    || string.IsNullOrEmpty(requestData.PhoneNumber)
                    )
                {
                    rs.ReturnCode = -1;
                    rs.ReturnMsg = "Dữ liệu không được trống";
                    return Json(rs);
                }
                var model = await new PhoneShop.DataAccess.Services.AccountServices().SingInCustomer(requestData);
                rs.ReturnCode = model.ReturnCode;
                rs.ReturnMsg = model.ReturnMsg;
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(rs);
        }
    }
}
