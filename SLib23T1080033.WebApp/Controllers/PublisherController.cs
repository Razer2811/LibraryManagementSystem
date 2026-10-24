using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.WebApp.Controllers
{
    public class PublisherController(PublisherService _publisherService)
        : Controller
    {
        // GET: Hiển thị danh sách nhà xuất bản
        public async Task<IActionResult> Index()
        {
            var publishers = await _publisherService.ListAsync();
            return View(publishers);
        }

        // GET: Hiển thị form thêm nhà xuất bản
        [HttpGet]
        public IActionResult CreatePublisher()
        {
            return View();
        }

        // POST: Xử lý thêm nhà xuất bản
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePublisher(Publisher publisher)
        {
            if (!ModelState.IsValid)
            {
                return View(publisher);
            }

            try
            {
                await _publisherService.AddAsync(publisher);

                TempData["SuccessMessage"] =
                    "Thêm nhà xuất bản thành công!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(publisher);
            }
        }

        // GET: Hiển thị form chỉnh sửa nhà xuất bản
        [HttpGet]
        public async Task<IActionResult> EditPublisher(int id)
        {
            try
            {
                var publisher = await _publisherService.GetAsync(id);
                return View(publisher);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Xử lý cập nhật nhà xuất bản
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPublisher(
            int id, Publisher publisher)
        {
            if (id != publisher.PublisherId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(publisher);
            }

            try
            {
                var result = await _publisherService.UpdateAsync(publisher);

                if (result)
                {
                    TempData["SuccessMessage"] =
                        "Cập nhật nhà xuất bản thành công!";

                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(
                    string.Empty, "Không thể cập nhật nhà xuất bản.");

                return View(publisher);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(publisher);
            }
        }

        // GET: Hiển thị trang xác nhận xóa nhà xuất bản
        [HttpGet]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            try
            {
                var publisher = await _publisherService.GetAsync(id);
                return View(publisher);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Xử lý xóa nhà xuất bản
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePublisherConfirmed(int id)
        {
            try
            {
                var result = await _publisherService.DeleteAsync(id);

                if (result)
                {
                    TempData["SuccessMessage"] =
                        "Xóa nhà xuất bản thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] =
                        "Không thể xóa nhà xuất bản.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
