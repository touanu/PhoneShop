using CommonLibs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhoneShop.Commonlibs;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using PhoneShop.DataAccess.Services;

namespace PhoneShop.Controllers
{
    public class PromotionsController : Controller
    {
        readonly IConfiguration _configuration;
        public PromotionsController(IConfiguration config)
        {
            _configuration = config;
        }

        public IActionResult AddPromotion()
        {
            var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
            if (string.IsNullOrEmpty(token))
            {
                return Redirect("/Account/Login");
            }
            return View();
        }
        public async Task<JsonResult> AddPromotions(PromotionsRequestData requestData)
        {
            var returnData = new GetPromotionsReturnData();
            var messageFromServer = string.Empty;
            try
            {
                var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
                if (string.IsNullOrEmpty(token))
                {
                    messageFromServer = "Vui lòng đăng nhập";
                    return Json(messageFromServer);
                }
                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Promotions/AddPromotion";
                

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 3 : gọi httpclient bên common để post lên api
<<<<<<<<< Temporary merge branch 1
                var result = await HttpHelper.HttpSendPost(baseurl, url, jsonData);
=========
                var result = await PhoneShop.Commonlibs.HttpHelper.HttpSenPostWithToken(baseurl, url, jsonData,token);
>>>>>>>>> Temporary merge branch 2

                // Bước 4: nhận dữ liệu về 
                if (!string.IsNullOrEmpty(result))
                {
                    var response =  JsonConvert.DeserializeObject<GetPromotionsReturnData>(result);
                    returnData.ReturnMsg = response.ReturnMsg;
                    returnData.ReturnCode = response.ReturnCode;
                    return Json(returnData);
                }
                returnData.ReturnCode = -2;
                returnData.ReturnMsg = "Lỗi";
                return Json(returnData);
            }
            catch (Exception ex)
            {
                return Json(returnData);
            }


        }
        public IActionResult GetPromotion()
        {
            return View();
        }
        public async Task<JsonResult> GetPromotions(PromotionsRequestData requestData)
        {
            var list = new List<Promotion>();
            try
            {
                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Promotions/GetPromotion";

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 3 : gọi httpclient bên common để post lên api
                var result = await HttpHelper.HttpSendPost(baseurl, url, jsonData);

                // Bước 4: nhận dữ liệu về 
                if (!string.IsNullOrEmpty(result))
                {
                    var response = JsonConvert.DeserializeObject<PromotionsRequestData>(result);
                }
                return Json(list);
            }
            catch (Exception ex)
            {
                return Json(list);
            }
        }
    }
}
