using CommonLibs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhoneShop.Commonlibs;
using PhoneShop.DataAccess.DTO;

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
