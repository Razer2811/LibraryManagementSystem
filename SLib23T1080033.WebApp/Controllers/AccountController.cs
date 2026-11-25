using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.WebApp;
using System.Threading.Tasks;

namespace SLib23T1080033.WebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            //// Kiểm tra thông tin đăng nhập có đúng hay không
            //if (username != "admin")
            //{

            //    ModelState.AddModelError("Error", "Đăng nhập thất bại!");
            //    return View();
            //}

            WebUserData userData = new WebUserData()
            {
                UserId = username,
                UserName = username,
                DisplayName = username,
                Email = "admin@example.com",
                Roles = new List<string>()
                {
                    WebUserRoles.Administrator,
                    WebUserRoles.Employee
                }

            };

            // tạo giấy chứng chỉ

            var principal = userData.CreatePrincipal();

            // đăng nhập (cáp giấy chứng nhận)

            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
