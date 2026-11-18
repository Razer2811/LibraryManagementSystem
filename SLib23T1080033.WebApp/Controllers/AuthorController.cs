using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.WebApp.Controllers
{
    public class AuthorController(AuthorService authorService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(string? keyword)
        {
            try
            {
                var authors = await authorService.ListAsync();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    authors = authors.Where(author =>
                        (author.AuthorName?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (author.Biography?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false));
                }

                ViewBag.Keyword = keyword?.Trim();
                return View(authors);
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi
                // Chuyển hướng đến trang lỗi hoặc hiển thị thông báo lỗi
                return Content("Hệ thống đang quá tải. Vui lòng thử lại sau.");
            }
        }

        [HttpGet]
        public IActionResult CreateAuthor()
        {
            return View(new Author());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAuthor(Author author)
        {
            if (!ModelState.IsValid)
                return View(author);

            try
            {
                await authorService.AddAsync(author);
                TempData["SuccessMessage"] = "Thêm tác giả thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(author);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditAuthor(int id)
        {
            try
            {
                return View(await authorService.GetAsync(id));
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi
                // Chuyển hướng đến trang lỗi hoặc hiển thị thông báo lỗi
                return Content("Hệ thống đang quá tải. Vui lòng thử lại sau.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAuthor(int id, Author author)
        {
            if (id != author.AuthorId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(author);

            try
            {
                await authorService.UpdateAsync(author);
                TempData["SuccessMessage"] = "Cập nhật tác giả thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(author);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                return View(await authorService.GetAsync(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAuthorConfirmed(int id)
        {
            try
            {
                await authorService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Xóa tác giả thành công.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
