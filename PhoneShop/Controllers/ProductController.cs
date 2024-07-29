using CommonLibs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhoneShop.DataAccess.DTO;

namespace PhoneShop.Controllers
{
    public class ProductController(IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
            if (string.IsNullOrEmpty(token))
            {
                return Redirect("/Account/Login");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
            if (string.IsNullOrEmpty(token))
            {
                return Redirect("/Account/Login");
            }

            var baseurl = _configuration["API_URL:URL"] ?? "";
            var url = "api/Product/AddViewData";

            var result = await HttpHelper.HttpSendPost(baseurl, url, null);
            var returnData = JsonConvert.DeserializeObject<ProductAddViewReturnData>(result);

            return View(returnData);
        }

        [HttpPost]
        public async Task<IActionResult> GetProducts(ProductRequestGetData requestData)
        {
            try
            {
                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Product/Get";

                var result = await HttpHelper.HttpSendPost(baseurl, url,
                    JsonConvert.SerializeObject(requestData));
                var returnData = JsonConvert.DeserializeObject<ProductGetReturnData>(result);

                return PartialView(returnData);
            }
            catch (Exception)
            {
                return PartialView(null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProducts(ProductRequestAddUpdateData requestData)
        {
            var returnData = new ProductAddReturnData();
            if (requestData == null)
            {
                returnData.ReturnCode = (int)ReturnCode.Invalid;
                returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ";
                return Json(returnData);
            }
            
            var baseurl = _configuration["API_URL:URL"] ?? "";
            var url = "api/Product/Add";

            var result = await HttpHelper.HttpSendPost(baseurl, url,
                JsonConvert.SerializeObject(requestData));
            if (result == null)
            {
                returnData.ReturnCode = (int)ReturnCode.Failure;
                returnData.ReturnMsg = "Thất bại khi gửi dữ liệu về máy chủ.";
                return Json(returnData);
            }
            var rs = JsonConvert.DeserializeObject<ReturnData>(result);
            if (rs == null)
            {
                returnData.ReturnCode = (int)ReturnCode.EqualNull;
                returnData.ReturnMsg = "Không thể phân tích dữ liệu từ máy chủ.";
                return Json(returnData);
            }

            returnData.ReturnCode = rs.ReturnCode;
            returnData.ReturnMsg = rs.ReturnMsg;
            return Json(returnData);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct (ProductRequestDeleteData requestData)
        {
            var returnData = new ReturnData();
            if (requestData == null
                || requestData.ProductID == null
                || requestData.ProductID < 0
                )
            {
                returnData.ReturnCode = (int)ReturnCode.Invalid;
                returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ";
                return Json(returnData);
            }

            var baseurl = _configuration["API_URL:URL"] ?? "";
            var url = "api/Product/Delete";

            var result = await HttpHelper.HttpSendPost(baseurl, url,
                JsonConvert.SerializeObject(requestData));
            if (result == null)
            {
                returnData.ReturnCode = (int)ReturnCode.Failure;
                returnData.ReturnMsg = "Thất bại khi gửi dữ liệu về máy chủ.";
                return Json(returnData);
            }
            var rs = JsonConvert.DeserializeObject<ReturnData>(result);
            if (rs == null)
            {
                returnData.ReturnCode = (int)ReturnCode.EqualNull;
                returnData.ReturnMsg = "Không thể phân tích dữ liệu từ máy chủ.";
                return Json(returnData);
            }

            returnData.ReturnCode = rs.ReturnCode;
            returnData.ReturnMsg = rs.ReturnMsg;
            return Json(returnData);
        }
    }
}
