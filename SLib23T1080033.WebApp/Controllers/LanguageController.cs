
using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.WebApp.Controllers;

public class LanguageController(LanguageService languageService)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? keyword)
    {
        var languages = await languageService.ListAsync();
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            languages = languages.Where(l =>
                l.LanguageName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                l.LanguageCode.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }
        ViewBag.Keyword = keyword?.Trim();
        return View(languages);
    }

    // Hiển thị form thêm
    [HttpGet]
    public IActionResult CreateLanguage()
    {
        return View();
    }

    // Xử lý thêm
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateLanguage(Language language)
    {
        if (!ModelState.IsValid)
            return View(language);

        try
        {
            await languageService.AddAsync(language);

            TempData["SuccessMessage"] = "Thêm ngôn ngữ thành công!";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(language);
        }
    }

    // Hiển thị form sửa
    [HttpGet]
    public async Task<IActionResult> EditLanguage(int id)
    {
        try
        {
            var language = await languageService.GetAsync(id);
            return View(language);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // Xử lý sửa
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditLanguage(
        int id, Language language)
    {
        if (id != language.LanguageId)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(language);

        try
        {
            var result = await languageService.UpdateAsync(language);

            if (result)
            {
                TempData["SuccessMessage"] =
                    "Cập nhật ngôn ngữ thành công!";

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(
                string.Empty, "Không thể cập nhật ngôn ngữ.");

            return View(language);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(language);
        }
    }

    // Hiển thị trang xác nhận xóa
    [HttpGet]
    public async Task<IActionResult> DeleteLanguage(int id)
    {
        try
        {
            var language = await languageService.GetAsync(id);
            return View(language);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // Xử lý xóa
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteLanguageConfirmed(int id)
    {
        try
        {
            var result = await languageService.DeleteAsync(id);

            if (result)
                TempData["SuccessMessage"] = "Xóa ngôn ngữ thành công!";
            else
                TempData["ErrorMessage"] = "Không thể xóa ngôn ngữ.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
