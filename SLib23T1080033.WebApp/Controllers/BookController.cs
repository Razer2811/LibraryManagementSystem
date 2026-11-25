using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SLib23T1080033.WebApp.Controllers
{
    public class BookController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
