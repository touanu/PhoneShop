
using CommonLibs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using PhoneShop.DataAccess.Services;
using PhoneShop.DataAccess.UnitOfWork;
using PhoneShop.Models;
using System.Net.Http;

namespace PhoneShop.Controllers
{
    public class BrandController : Controller
    {
      
        private IConfiguration _configuration;

        public BrandController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BrandInsert()
        {
            var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
            if (string.IsNullOrEmpty(token))
            {
                return Redirect("/Account/Login");
            }
            return View();
        }
        public async Task<ActionResult> GetBrands(BrandInsertRequestData requestData)
        {
            var messageFromServer = string.Empty;
            var list = new List<Brand>();
            try
            {
                requestData.IconImages = "";
                if (requestData.BrandName==null) { requestData.BrandName = ""; }
                var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
                if (string.IsNullOrEmpty(token))
                {
                    messageFromServer = "Vui lòng đăng nhập";
                    return PartialView(list);
                }
                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Brand/BrandGetList";

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 3 : gọi httpclient bên common để post lên api
                var result = await HttpHelper.HttpSenPostWithToken(baseurl, url, jsonData, token);

                if (!string.IsNullOrEmpty(result))
                {
                    var response = JsonConvert.DeserializeObject<BrandListReturnData>(result);
                    if (response != null)
                    {
                        if (response.ReturnCode < 0)
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
                            list.Add(new Brand
                            {
                               BrandID =item.BrandID,
                               BrandName =item.BrandName,
                               IconImages = item.IconImages,
                            });

                        }
                    }

                }
                return PartialView(list);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<JsonResult> BrandInserts(BrandInsertRequestData requetsData) 
        {

            var returnData = new BrandInsertReturnData();
            var messageFromServer = string.Empty;
            try

            {
                var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
                if (string.IsNullOrEmpty(token))
                {
                    messageFromServer = "Vui lòng đăng nhập";
                    return Json(messageFromServer);
                }
                // Bước 1 : khai báo API URL

                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Brand/BrandInsert";

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requetsData);

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
                returnData.ReturnCode = rs.ReturnCode;
                returnData.ReturnMsg =rs.ReturnMsg;

                return Json(returnData);

            }
            catch (Exception ex)
            {
                return Json(returnData);
            }
            return Json(returnData);

        }

        public async Task<JsonResult> DeleteBrand(BrandInsertRequestData requestData)
        {
            ReturnData returnData = new ReturnData();
            var messageFromServer = string.Empty;
            try
            {
                var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
                if (string.IsNullOrEmpty(token))
                {
                    messageFromServer = "Vui lòng đăng nhập";
                    return Json(messageFromServer);
                }
                // Bước 1: Kiểm tra dữ liệu đầu vào 
                if (requestData.BrandID <= 0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ!";
                    return Json(returnData);
                }
                requestData.BrandName = "";
                requestData.IconImages = "";
                // Bước 2 : GỌI API ĐỂ LẤY TOKEN 
                // bƯỚC 2.1 kHAI BÁO URL CỦA API

                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Brand/RemoveBrand";

                //Bước 2.2 convert từ object requestData sang Json để đẩy lên API
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 2.3 dùng httpClient để đưa json lên URL của API
                var result = await HttpHelper.HttpSenPostWithToken(baseurl, url, jsonData, token);

                if (string.IsNullOrEmpty(result))
                {
                    returnData.ReturnCode = -2;
                    returnData.ReturnMsg = "Lỗi";
                    return Json(returnData);
                }

                // Bước 2.4 : Convert từ json nhận được thành object 

                var rs = JsonConvert.DeserializeObject<ReturnData>(result);
                if (rs.ReturnCode <= 0)
                {
                    returnData.ReturnCode = rs.ReturnCode;
                    returnData.ReturnMsg = rs.ReturnMsg;
                    return Json(returnData);
                }
                returnData.ReturnMsg = rs.ReturnMsg;
                returnData.ReturnCode = rs.ReturnCode;
                return Json(returnData);
            }
            catch (Exception ex)
            {

                returnData.ReturnCode = -969;
                returnData.ReturnMsg = ex.Message;
                return Json(returnData);
            }
        }
    }
}
