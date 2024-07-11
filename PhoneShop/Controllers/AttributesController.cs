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
       
        public async Task<JsonResult>AddAtrributes(AttributesRequestData attributesRequestData)
        {
            var returnData = new AttributesReturnData();
            try

            {
                var messageFromServer = string.Empty;
                var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
                if (string.IsNullOrEmpty(token))
                {
                    messageFromServer = "Vui lòng đăng nhập";
                    return Json(messageFromServer);
                }
                // Bước 1 : khai báo API URL

                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Attributes/AddAttribute";

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(attributesRequestData);

                // Bước 3 : gọi httpclient bên common để post lên api
                var result = await PhoneShop.Commonlibs.HttpHelper.HttpSenPostWithToken(baseurl, url, jsonData,token);

                // Bước 4: nhận dữ liệu về 
                if (!string.IsNullOrEmpty(result))
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
            return Json(returnData);
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
        public async Task<IActionResult> GetAttribute(AttributesResponseData requestData)
        {
            var list = new List<ProductAttributes>();
            try

            {
                // Bước 1 : khai báo API URL

                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Attributes/GetAttribute";

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 3 : gọi httpclient bên common để post lên api
                var result = await PhoneShop.Commonlibs.HttpHelper.HttpSenPost(baseurl, url, jsonData);

                // Bước 4: nhận dữ liệu về 
                if (!string.IsNullOrEmpty(result))
                {
                    var response = JsonConvert.DeserializeObject<AttributesResponseData>(result);
                    if (response!= null)
                    {
                        foreach (var item in response.AttributesName)
                        {
                            list.Add(new ProductAttributes
                            {
                                AttributesName = item.ToString(),
                            }) ;

                        }
                    }
                }

                return PartialView(list);

            }
            catch (Exception ex)
            {
                return PartialView(list);
            }
            return PartialView(list);
        }
    }
}
