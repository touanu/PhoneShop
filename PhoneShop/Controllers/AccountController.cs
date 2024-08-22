using CommonLibs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Protocol;
using PhoneShop.Commonlibs;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using PhoneShop.DataAccess.Services;
using PhoneShop.DataAccess.UnitOfWork;
using System.Collections.Generic;

namespace PhoneShop.Controllers
{
    public class AccountController : Controller
    {
        readonly IConfiguration _configuration;
        
        public AccountController(IConfiguration configuration)
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
        public async Task<ActionResult> Logouts()
        {
            var returnData = new ReturnData();
            try
            {
                //lấy token từ server
                var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
                if (string.IsNullOrEmpty(token))
                {
                    return Redirect("/");
                }
                // Xóa token tại local
                Response.Cookies.Delete("MY_JWT_TOKEN");
                return Redirect("/");
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = -969;
                returnData.ReturnMsg = ex.Message;
                return Json(returnData);
            }
        }
        public IActionResult Login()
        {
            return View();
        }

        public async Task<JsonResult>Logins(AccountRequestData requestData)
        {
            var returnData = new ReturnData();
            try
            {
                // Bước 1: Kiểm tra dữ liệu đầu vào 
                if (requestData == null || string.IsNullOrEmpty(requestData.UserName)
                    || string.IsNullOrEmpty(requestData.PassWord))
                {
                    returnData.ReturnCode = -2;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không được trống!";
                    return Json(returnData);
                }
                if (requestData.Birthday == DateTime.MinValue)
                {
                    requestData.FristName = "";
                    requestData.LastName = "";
                    requestData.PhoneNumber = "";
                    requestData.Email = "";
                    requestData.Birthday = DateTime.Now;
                }
                // Bước 2 : GỌI API ĐỂ LẤY TOKEN 
                // bƯỚC 2.1 kHAI BÁO URL CỦA API

                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Account/Login";

                //Bước 2.2 convert từ object requestData sang Json để đẩy lên API
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 2.3 dùng httpClient để đưa json lên URL của API
                var result = await HttpHelper.HttpSendPost(baseurl, url, jsonData);

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


                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "Đăng nhập thành công!";
                returnData.Token = rs.Token;

                //  Session
                return Json(returnData);
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = -969;
                returnData.ReturnMsg = ex.Message;
                return Json(returnData);
            }
        }
        
        public IActionResult Register()
        {
            return View();
        }
        public async Task<JsonResult> Registers(AccountRequestData user)
        {
            var returnData = new ReturnData();
            try
            {
                // Bước 1: Kiểm tra dữ liệu đầu vào 
                if (user == null || string.IsNullOrEmpty(user.UserName)
                    || string.IsNullOrEmpty(user.PassWord)
                    ||string.IsNullOrEmpty(user.FristName)
                    ||string.IsNullOrEmpty(user.LastName))
                {
                    returnData.ReturnCode = -2;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không được trống!";
                    return Json(returnData);
                }

                // Bước 2 : GỌI API ĐỂ LẤY TOKEN 
                // bƯỚC 2.1 kHAI BÁO URL CỦA API

                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Account/Register";

                //Bước 2.2 convert từ object requestData sang Json để đẩy lên API
                var jsonData = JsonConvert.SerializeObject(user);

                // Bước 2.3 dùng httpClient để đưa json lên URL của API
                var result = await HttpHelper.HttpSendPost(baseurl, url, jsonData);

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
                return Json(returnData);
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = -969;
                returnData.ReturnMsg = ex.Message;
                return Json(returnData);
            }
        }
        public IActionResult RemoveCustomer()
        {
            var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
            if (string.IsNullOrEmpty(token))
            {
                return Redirect("/Account/Login");
            }
           
            return View();
        }
        public async Task<JsonResult> RemoveCustomers(AccountRequestData requestData)
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
                if (requestData.ID <= 0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Dữ liệu đầu vào không hợp lệ!";
                    return Json(returnData);
                }
                requestData.Email = "";
                requestData.FristName = "";
                requestData.LastName = "";
                requestData.PhoneNumber = "";
                requestData.UserName = "";
                requestData.PassWord = "";
                // Bước 2 : GỌI API ĐỂ LẤY TOKEN 
                // bƯỚC 2.1 kHAI BÁO URL CỦA API

                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Account/RemoveCustomer";

                //Bước 2.2 convert từ object requestData sang Json để đẩy lên API
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 2.3 dùng httpClient để đưa json lên URL của API
                var result = await HttpHelper.HttpSenPostWithToken(baseurl, url, jsonData,token);
                
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
                returnData.ReturnMsg=rs.ReturnMsg;
                returnData.ReturnCode=rs.ReturnCode;
                return Json(returnData);
            }
            catch (Exception ex)
            {

                returnData.ReturnCode = -969;
                returnData.ReturnMsg = ex.Message;
                return Json(returnData);
            }
        }
        public async Task<ActionResult> GetCustomers(AccountRequestData requestData)
        {
            var messageFromServer = string.Empty;
            var list = new List<Customer>();
            try
            {                
                var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
                if (string.IsNullOrEmpty(token))
                {
                    messageFromServer = "Vui lòng đăng nhập";
                    return View(list);
                }
                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Account/GetCustomer";

                // bƯỚC 2: tạo json data ( object sang JSON)
                var jsonData = JsonConvert.SerializeObject(requestData);

                // Bước 3 : gọi httpclient bên common để post lên api
                var result = await HttpHelper.HttpSenPostWithToken(baseurl, url, jsonData, token);

                if (!string.IsNullOrEmpty(result))
                {
                    var response = JsonConvert.DeserializeObject<GetCustomerReturnData>(result);
                    if (response != null)
                    {
                        if (response.ReturnCode < 0)
                        {
                            messageFromServer = response.ReturnMsg;
                            ViewBag.ErrorCode = response.ReturnCode;
                            ViewBag.ErrorMessage = messageFromServer;
                            return View(list);
                        }
                        if (response?.listcustomer == null || response?.listcustomer.Count <= 0)
                        {
                            messageFromServer = "Không có dữ liệu.Vui lòng kiểm tra lại";
                            ViewBag.ErrorMessage = messageFromServer;
                            return View(list);
                        }

                        foreach (var item in response?.listcustomer)
                        {
                            list.Add(new Customer
                            {
                                CustomerID = item.CustomerID,
                                Birthday = item.Birthday,
                                FirstName = item.FirstName,
                                LastName = item.LastName,
                                UserName = item.UserName,
                                PassWord = item.PassWord,
                                PhoneNumber = item.PhoneNumber,
                                Email = item.Email,
                                WardID = item.WardID,
                                ProvinceID = item.ProvinceID,
                                DistrictID =item.DistrictID,
                            });

                        }
                    }

                }
                return View(list);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
