using Microsoft.AspNetCore.Mvc;

namespace SLib23T1080033.WebApp.Controllers
{
    public class ReturnController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
