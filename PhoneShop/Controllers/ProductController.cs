using Microsoft.AspNetCore.Mvc;

namespace PhoneShop.Controllers
{
    [Route("admin/[controller]")]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
