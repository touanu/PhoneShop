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
        public IActionResult Index()
        {
            var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
            if (string.IsNullOrEmpty(token))
            {
                return Redirect("/Account/Login");
            }
            return View();
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
                var url = "api/Promotion/AddPromotion";
                

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 3 : gọi httpclient bên common để post lên api
                var result = await HttpHelper.HttpSenPostWithToken(baseurl, url, jsonData,token);

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
        public async Task<IActionResult> GetPromotions(PromotionsRequestData requestData)
        {
            var messageFromServer = string.Empty;
            var list = new List<Promotion>();
            try
            {
                var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
                if (string.IsNullOrEmpty(token))
                {
                    messageFromServer = "Vui lòng đăng nhập";
                    return PartialView(list);
                }
                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Promotion/GetPromotion";

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 3 : gọi httpclient bên common để post lên api
                var result = await HttpHelper.HttpSenPostWithToken(baseurl, url, jsonData,token);

                // Bước 4: nhận dữ liệu về 
                if (!string.IsNullOrEmpty(result))
                {
                    var response = JsonConvert.DeserializeObject<ReturnDataReturnpromotion>(result);
                    if (response != null)
                    {
                        if (response.ReturnCode < 0)
                        {
                            messageFromServer = response.ReturnMsg;
                            ViewBag.ErrorCode = response.ReturnCode;
                            ViewBag.ErrorMessage = messageFromServer;
                            return PartialView(list);
                        }
                        if (response?.listpromotion == null || response?.listpromotion.Count <= 0)
                        {
                            messageFromServer = "Không có dữ liệu.Vui lòng kiểm tra lại";
                            ViewBag.ErrorMessage = messageFromServer;
                            return PartialView(list);
                        }

                        foreach (var item in response?.listpromotion)
                        {
                            list.Add(new Promotion
                            {
                               PromotionID =item.PromotionID,
                               PercentageDiscount = item.PercentageDiscount,
                               ProductID = item.ProductID,
                               CustomerID = item.CustomerID,
                               PromotionName = item.PromotionName,
                               MinimumAmount = item.MinimumAmount,
                               EndDate = item.EndDate,
                               Quantity = item.Quantity,
                               QuantityOneDay = item.QuantityOneDay,
                               StartDate = item.StartDate,
                               TotalDiscount = item.TotalDiscount,
                            });

                        }
                    }
                }
                return PartialView(list);
            }
            catch (Exception ex)
            {
                return PartialView(list);
            }
        }
    }
}
