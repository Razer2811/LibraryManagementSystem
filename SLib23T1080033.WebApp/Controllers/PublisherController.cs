using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.WebApp.Controllers
{
    public class PublisherController(PublisherService publisherService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(string? keyword)
        {
            try
            {
                var publishers = await publisherService.ListAsync();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    publishers = publishers.Where(publisher =>
                        publisher.PublisherName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                        (publisher.Address?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (publisher.Email?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false));
                }

                ViewBag.Keyword = keyword?.Trim();
                return View(publishers);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(Enumerable.Empty<Publisher>());
            }
        }

        [HttpGet]
        public IActionResult CreatePublisher()
        {
            return View(new Publisher { IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePublisher(Publisher publisher)
        {
            if (!ModelState.IsValid)
                return View(publisher);

            try
            {
                await publisherService.AddAsync(publisher);
                TempData["SuccessMessage"] = "Thêm nhà xuất bản thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(publisher);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditPublisher(int id)
        {
            try
            {
                return View(await publisherService.GetAsync(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPublisher(int id, Publisher publisher)
        {
            if (id != publisher.PublisherId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(publisher);

            try
            {
                await publisherService.UpdateAsync(publisher);
                TempData["SuccessMessage"] = "Cập nhật nhà xuất bản thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(publisher);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            try
            {
                return View(await publisherService.GetAsync(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePublisherConfirmed(int id)
        {
            try
            {
                await publisherService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Xóa nhà xuất bản thành công.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
