using CommonLibs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using OfficeOpenXml;
using PhoneShop.Commonlibs;
using PhoneShop.DataAccess.DTO;
using System.Web;

namespace PhoneShop.Controllers
{
    public class OrderController(IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpGet]
        public IActionResult Index()
        {
            var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
            if (string.IsNullOrEmpty(token))
            {
                return Redirect("/Account/Login");
            }
            return View();
        }

        [HttpGet("/Order/Detail")]
        public IActionResult OrderDetail()
        {
            var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
            if (string.IsNullOrEmpty(token))
            {
                return Redirect("/Account/Login");
            }

            return View();
        }

        [HttpGet("/Order/Print")]
        public IActionResult PrintOrder()
        {
            var token = Request.Cookies["MY_JWT_TOKEN"] != null ? Request.Cookies["MY_JWT_TOKEN"].ToString() : "";
            if (string.IsNullOrEmpty(token))
            {
                return Redirect("/Account/Login");
            }
            return View();
        }

        //[HttpPost("/Order/Export/{id}")]
        //public async Task<IActionResult> ExportOrder(int id)
        //{
        //    // Lấy dữ liệu
        //    var baseurl = _configuration["API_URL:URL"] ?? "";
        //    var url = "api/Order/GetDetail";
        //    var requestData = new OrderDetailGetRequestData
        //    {
        //        OrderID = id
        //    };

        //    var result = await HttpHelper.HttpSendPost(baseurl, url,
        //        JsonConvert.SerializeObject(requestData));
        //    var returnData = JsonConvert.DeserializeObject<OrderGetReturnData>(result);

        //    if (returnData == null || returnData.ReturnCode < 0)
        //    {
        //        return Ok();
        //    }

        //    // Tạo tệp excel
        //    using var ms = new MemoryStream();
        //    using var p = new ExcelPackage(ms);

        //    var sheet = p.Workbook.Worksheets.Add("Sheet1");
        //    sheet.Cells["A1"].Value = "";
        //    p.Save();
        //    using var p2 = new ExcelPackage(ms);
        //    var helloWorld = p2.Workbook.Worksheets[0].Cells["A1"].Value;

        //    // Tạo thông tin cho header và trả về tệp tin
        //    string fileName = $"Order{returnData.Order.OrderID}_{returnData.Customer.UserName}_{returnData.Order.CreatedDate}_export.xlsx";
        //    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    var cd = new System.Net.Mime.ContentDisposition
        //    {
        //        FileName = fileName,
        //        Inline = true,
        //    };

        //    Response.Headers.Append("Content-Disposition", cd.ToString());
        //    return File(filedata, contentType);
        //}

        [HttpPost]
        public async Task<IActionResult> GetOrders(OrderGetRequestData requestData)
        {
            try
            {
                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Order/Get";

                var result = await HttpHelper.HttpSendPost(baseurl, url,
                    JsonConvert.SerializeObject(requestData));
                var returnData = JsonConvert.DeserializeObject<OrderGetViewReturnData>(result);

                return PartialView(returnData);
            }
            catch (Exception)
            {
                return PartialView(null);
            }
        }

        [HttpPost("/Order/GetOrderDetail/{id}")]
        public async Task<IActionResult> GetOrderDetail(int id)
        {
            try
            {
                var baseurl = _configuration["API_URL:URL"] ?? "";
                var url = "api/Order/GetDetail";
                var requestData = new OrderDetailGetRequestData
                {
                    OrderID = id
                };

                var result = await HttpHelper.HttpSendPost(baseurl, url,
                    JsonConvert.SerializeObject(requestData));
                var returnData = JsonConvert.DeserializeObject<OrderGetReturnData>(result);

                if (returnData == null || returnData.ReturnCode < 0)
                {
                    return PartialView(null);
                }

                return PartialView(returnData);
            }
            catch (Exception)
            {
                return PartialView(null);
            }
        }
    }
}
