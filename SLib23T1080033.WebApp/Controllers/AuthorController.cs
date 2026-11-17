using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.Entities;
using System.Threading.Tasks;

namespace SLib23T1080033.WebApp.Controllers
{
    public class AuthorController(AuthorService _authorService) : Controller
    {
        // Hiển thị danh sách tác giả
        public async Task<IActionResult> Index()
        {
            try
            {

                var model = await _authorService.ListAsync();
                return View(model);

            }
            catch (Exception ex)
            {
                // Ghi lại lỗi
                // Chuyển hướng đến trang lỗi hoặc hiển thị thông báo lỗi
                return Content("Hệ thống đang quá tải. Vui lòng thử lại sau.");

            }
        }
        // Hiển thị form tạo tác giả mới
        public async Task<IActionResult> CreateAuthor(int id)
        {
            try
            {
                var model = new Author();
                return View(model);

            }
            catch (Exception ex)
            {
                // Ghi lại lỗi
                // Chuyển hướng đến trang lỗi hoặc hiển thị thông báo lỗi
                return Content("Hệ thống đang quá tải. Vui lòng thử lại sau.");

            }
        }
        //Hiển thị giao diện cập nhật thông tin tác giả
        public async Task<IActionResult> EditAuthor(int id)
        {
            try
            {

                var model = await _authorService.GetAsync(id);
                if(model == null)
                {
                    return RedirectToAction("Index");
                }
                return View(model);

            }
            catch (Exception ex)
            {
                // Ghi lại lỗi
                // Chuyển hướng đến trang lỗi hoặc hiển thị thông báo lỗi
                return Content("Hệ thống đang quá tải. Vui lòng thử lại sau.");

            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveData(Author data)
        {
            // TODO : Phải kiểm tả dữ liệu đầu vào trước khi lưu vào CSDL
            try
            {
                if (data.AuthorId == 0)
                {
                    await _authorService.AddAsync(data);
                }
                else
                {
                    await _authorService.UpdateAsync(data);
                }

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                // Ghi lại lỗi
                // Chuyển hướng đến trang lỗi hoặc hiển thị thông báo lỗi
                return Content("Hệ thống đang quá tải. Vui lòng thử lại sau.");

            }
        }
        //Hiển thị giao diện Xóa tác giả
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                if(Request.Method == "POST")
                {
                    await _authorService.DeleteAsync(id);
                    return RedirectToAction("Index");
                }

                var model = await _authorService.GetAsync(id);
                if (model == null)
                {
                    return RedirectToAction("Index");
                }
                return View(model);

            }
            catch (Exception ex)
            {
                // Ghi lại lỗi
                // Chuyển hướng đến trang lỗi hoặc hiển thị thông báo lỗi
                return Content("Hệ thống đang quá tải. Vui lòng thử lại sau.");

            }
        }

        //private readonly AuthorService _authorService;

        //public AuthorController(AuthorService authorService)
        //{
        //    _authorService = authorService;
        //}

        //public async Task<IActionResult> Index()
        //{
        //    var authors =await _authorService.ListAsync();
        //    return View(authors);

        //}
        //// GET: Hiển thị form thêm tác giả
        //[HttpGet]
        //public IActionResult CreateAuthor()
        //{
        //    return View();
        //}

        //// POST: Xử lý thêm tác giả
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateAuthor(Author author)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(author);
        //    }

        //    try
        //    {
        //        await _authorService.AddAsync(author);

        //        TempData["SuccessMessage"] = "Thêm tác giả thành công!";

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, ex.Message);
        //        return View(author);
        //    }
        //}
        //// GET: Hiển thị form chỉnh sửa tác giả
        //[HttpGet]
        //public async Task<IActionResult> EditAuthor(int id)
        //{
        //    try
        //    {
        //        var author = await _authorService.GetAsync(id);
        //        return View(author);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = ex.Message;
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        //// POST: Xử lý cập nhật tác giả
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditAuthor(int id, Author author)
        //{
        //    if (id != author.AuthorId)
        //    {
        //        return BadRequest();
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return View(author);
        //    }

        //    try
        //    {
        //        var result = await _authorService.UpdateAsync(author);

        //        if (result)
        //        {
        //            TempData["SuccessMessage"] = "Cập nhật tác giả thành công!";
        //            return RedirectToAction(nameof(Index));
        //        }

        //        ModelState.AddModelError(
        //            string.Empty, "Không thể cập nhật tác giả.");

        //        return View(author);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, ex.Message);
        //        return View(author);
        //    }
        //}
        //// GET: Hiển thị trang xác nhận xóa tác giả
        //[HttpGet]
        //public async Task<IActionResult> DeleteAuthor(int id)
        //{
        //    try
        //    {
        //        var author = await _authorService.GetAsync(id);
        //        return View(author);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = ex.Message;
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        //// POST: Xử lý xóa tác giả
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteAuthorConfirmed(int id)
        //{
        //    try
        //    {
        //        var result = await _authorService.DeleteAsync(id);

        //        if (result)
        //        {
        //            TempData["SuccessMessage"] = "Xóa tác giả thành công!";
        //            return RedirectToAction(nameof(Index));
        //        }

        //        TempData["ErrorMessage"] = "Không thể xóa tác giả.";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = ex.Message;
        //        return RedirectToAction(nameof(Index));
        //    }
        //}
    }
}
