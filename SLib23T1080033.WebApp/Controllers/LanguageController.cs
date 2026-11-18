using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.WebApp.Controllers;

public class LanguageController(LanguageService languageService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? keyword)
    {
        try
        {
            var languages = await languageService.ListAsync();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                languages = languages.Where(language =>
                    language.LanguageName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    language.LanguageCode.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            ViewBag.Keyword = keyword?.Trim();
            return View(languages);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return View(Enumerable.Empty<Language>());
        }
    }

    [HttpGet]
    public IActionResult CreateLanguage()
    {
        return View(new Language { IsActive = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateLanguage(Language language)
    {
        if (!ModelState.IsValid)
            return View(language);

        try
        {
            await languageService.AddAsync(language);
            TempData["SuccessMessage"] = "Thêm ngôn ngữ thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(language);
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditLanguage(int id)
    {
        try
        {
            return View(await languageService.GetAsync(id));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditLanguage(int id, Language language)
    {
        if (id != language.LanguageId)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(language);

        try
        {
            await languageService.UpdateAsync(language);
            TempData["SuccessMessage"] = "Cập nhật ngôn ngữ thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(language);
        }
    }

    [HttpGet]
    public async Task<IActionResult> DeleteLanguage(int id)
    {
        try
        {
            return View(await languageService.GetAsync(id));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteLanguageConfirmed(int id)
    {
        try
        {
            await languageService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Xóa ngôn ngữ thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
