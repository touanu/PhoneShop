using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhoneShop.DataAccess.DTO;

namespace PhoneShop.Controllers
{
    public class ProductController(IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<IActionResult> Index(int? page, string? sortOrder)
        {
            var baseurl = _configuration["API_URL:URL"] ?? "";
            var url = "api/Product/Get";
            var requestData = JsonConvert.SerializeObject(
                new ProductRequestGetData
                {
                    PageNumber = page ?? 1,
                    PageSize = 5
                });

            var result = await Commonlibs.HttpHelper.HttpSendPost(baseurl, url, requestData);
            var products = JsonConvert.DeserializeObject<ProductGetReturnData>(result);
            // products.Products = [];
            return View(products);
        }
    }
}
