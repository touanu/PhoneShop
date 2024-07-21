using CommonLibs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using System.Text;

namespace PhoneShop.Controllers
{
    public class AttributesController : Controller
    {
        readonly IConfiguration _configuration;
        public AttributesController(IConfiguration configuration)
        {
            _configuration = configuration;

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
        public IActionResult AddAttribute()
        {
            var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
            if (string.IsNullOrEmpty(token))
            {
                return Redirect("/Account/Login");
            }
            return View();
        }
       
        public async Task<JsonResult>AddAttributes(AttributesRequestData attributesRequestData)
        {
            var returnData = new AttributesReturnData();
            var messageFromServer = string.Empty;
            try
            {
                attributesRequestData.AttributesName = "";
                attributesRequestData.AttributeValluesName = "";
                var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
                if (string.IsNullOrEmpty(token))
                {
                    messageFromServer = "Vui lòng đăng nhập";
                    return Json(messageFromServer);
                }
                // Bước 1 : khai báo API URL

                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Attribute/AddAttribute";

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(attributesRequestData);

                // Bước 3 : gọi httpclient bên common để post lên api
                var result = await HttpHelper.HttpSenPostWithToken(baseurl, url, jsonData,token);

                // Bước 4: nhận dữ liệu về 
                if (string.IsNullOrEmpty(result))
                {
                    returnData.ReturnCode = -2;
                    returnData.ReturnMsg = "Lỗi";
                    return Json(returnData);
                }
                var rs = JsonConvert.DeserializeObject<ReturnData>(result);
                if (rs.ReturnCode <= 0)
                {
                    returnData.ReturnCode = rs.ReturnCode;
                    returnData.ReturnMsg = rs.ReturnMsg;
                    return Json(returnData);
                }
                return Json(returnData);

            }
            catch (Exception ex)
            {
                return Json(returnData);
            }
        }
 
        public IActionResult DeleteAttributeValueByName()
        {
            return View();
        }
        
        public async Task<JsonResult> DeleteAttributeValueByNames(AttributesRequestData requestData)
        {
            var returnData = new ReturnData();
            try
            {
                if (requestData.AttributeValluesName == null)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu vào không hợp lệ!";
                    return Json(returnData);
                }
                return Json(returnData) ;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<IActionResult> GetAttributes(AttributesResponseData requestData)
        {
            var messageFromServer = string.Empty;
            var list = new List<ProductAttribute>();
            try
           
            {
                var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
                if (string.IsNullOrEmpty(token))
                {
                    messageFromServer = "Vui lòng đăng nhập";
                    return PartialView(list);
                }
                // Bước 1 : khai báo API URL

                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Attribute/GetAttribute";

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 3 : gọi httpclient bên common để post lên api
                var result = await HttpHelper.HttpSenPostWithToken(baseurl, url, jsonData,token);

                // Bước 4: nhận dữ liệu về 
                if (!string.IsNullOrEmpty(result))
                {
                    var response = JsonConvert.DeserializeObject<ReturnDataReturnAttributes>(result);
                    if (response!= null)
                    {
                        if(response.ReturnCode<0)
                        {
                            messageFromServer = response.ReturnMsg;
                            ViewBag.ErrorCode = response.ReturnCode;
                            ViewBag.ErrorMessage = messageFromServer;
                            return PartialView(list);
                        }
                        if (response?.list == null || response?.list.Count <= 0)
                        {
                            messageFromServer = "Không có dữ liệu.Vui lòng kiểm tra lại";
                            ViewBag.ErrorMessage = messageFromServer;
                            return PartialView(list);
                        }

                        foreach (var item in response?.list)
                        {
                            list.Add(new ProductAttribute
                            {
                                ProductAttributeID = item.ProductAttributeID,
                                ProductID = item.ProductID,
                                AttributesName = item.AttributesName
                            });

                        }
                    }
                }
                ViewBag.ErrorMessage = messageFromServer;
                return PartialView(list);

            }
            catch (Exception ex)
            {
                return PartialView(list);
            }
        }
    }
}
