using CommonLibs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhoneShop.DataAccess.DTO;

namespace PhoneShop.Controllers
{
    public class CategoryController : Controller
    {
        readonly IConfiguration _configuration;
        public CategoryController(IConfiguration configuration)
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
        public IActionResult AddCategory()
        {
            var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
            if (string.IsNullOrEmpty(token))
            {
                return Redirect("/Account/Login");
            }
            return View();
        }
        public async Task<JsonResult> AddCategorys(CategoryRequestData requestData)
        {
            var model = new GetCategoryReturnData();
            var messageFromServer = string.Empty;
            try
            {
                var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
                if (string.IsNullOrEmpty(token))
                {
                    messageFromServer = "Vui lòng đăng nhập";
                    return Json(messageFromServer);
                }
                if (requestData == null
                    || string.IsNullOrEmpty(requestData.CategoryName))
                {
                    model.ReturnCode = -1;
                    model.ReturnMsg = "Dữ liệu không được trống";
                    return Json(model);
                }


                // Bước 1.1 : khai báo API URL

                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Category/AddCategory";

                // bƯỚC 1.2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 1.3 : gọi httpclient bên common để post lên api
                var result = await HttpHelper.HttpSenPostWithToken(baseurl, url, jsonData,token);

                // Bước 1.4: nhận dữ liệu về 
                if (string.IsNullOrEmpty(result))
                {
                    model.ReturnCode = -2;
                    model.ReturnMsg = "Lỗi";
                    return Json(model);
                }
                var rs = JsonConvert.DeserializeObject<ReturnData>(result);
                model.ReturnCode = rs.ReturnCode;
                model.ReturnMsg = rs.ReturnMsg;
                return Json(model);
            }
            catch (Exception ex)
            {

                throw;
            }

            return Json(model);
        }
        public async Task<ActionResult> GetCategorys(CategoryRequestData requestData)
        {
            var messageFromServer = string.Empty;
            var list = new List<Category>();
            try
            {
                requestData.DisplayStatus = 0;
                requestData.IconImages = "";
                
                var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
                if (string.IsNullOrEmpty(token))
                {
                    messageFromServer = "Vui lòng đăng nhập";
                    return PartialView(list);
                }
                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Category/GetCategory";

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 3 : gọi httpclient bên common để post lên api
                var result = await HttpHelper.HttpSenPostWithToken(baseurl, url, jsonData, token);

                if (!string.IsNullOrEmpty(result))
                {
                    var response = JsonConvert.DeserializeObject<GetCategoryReturnData>(result);
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
                            list.Add(new Category
                            {
                                CategoryID = item.CategoryID,
                                CategoryName = item.CategoryName,
                                IconImages = item.IconImages,
                                DisplayStatus = item.DisplayStatus,
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
        public async Task<JsonResult> DeleteCategory(CategoryRequestData requestData)
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
                if (requestData.CategoryID <= 0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ!";
                    return Json(returnData);
                }
                requestData.CategoryName = "";
                requestData.IconImages = "";
                // Bước 2 : GỌI API ĐỂ LẤY TOKEN 
                // bƯỚC 2.1 kHAI BÁO URL CỦA API

                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Category/RemoveCategory";

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
