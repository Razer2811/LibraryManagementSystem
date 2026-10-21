using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using System.Threading.Tasks;

namespace SLib23T1080033.WebApp.Controllers
{
    public class AuthorController(AuthorService _authorService) : Controller
    {
        //private readonly AuthorService _authorService;

        //public AuthorController(AuthorService authorService)
        //{
        //    _authorService = authorService;
        //}

        public async Task<IActionResult> Index()
        {
            var authors =await _authorService.ListAsync();
            return View(authors);
        }
    }
}
