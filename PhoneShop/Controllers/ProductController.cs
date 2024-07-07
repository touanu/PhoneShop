using Microsoft.AspNetCore.Mvc;

namespace PhoneShop.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
