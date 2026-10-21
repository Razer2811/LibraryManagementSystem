using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.Entities;
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
        // GET: Hiển thị form thêm tác giả
        [HttpGet]
        public IActionResult CreateAuthor()
        {
            return View();
        }

        // POST: Xử lý thêm tác giả
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAuthor(Author author)
        {
            if (!ModelState.IsValid)
            {
                return View(author);
            }

            try
            {
                await _authorService.AddAsync(author);

                TempData["SuccessMessage"] = "Thêm tác giả thành công!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(author);
            }
        }
    }
}
