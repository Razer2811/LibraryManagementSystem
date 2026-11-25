using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.WebApp.Controllers;

public class CategoryController(CategoryService categoryService) : Controller
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index(string? keyword)
    {
        try
        {
            var categories = await categoryService.ListAsync();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                categories = categories.Where(category =>
                    category.CategoryName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    (category.Description?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false));
            }

            ViewBag.Keyword = keyword?.Trim();
            return View(categories);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return View(Enumerable.Empty<Category>());
        }
    }

    private async Task LoadParentCategoriesAsync(int? currentCategoryId = null)
    {
        var categories = await categoryService.ListAsync();
        ViewBag.ParentCategories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            categories.Where(category => category.CategoryId != currentCategoryId),
            nameof(Category.CategoryId),
            nameof(Category.CategoryName),
            null);
    }

    [HttpGet]
    public async Task<IActionResult> CreateCategory()
    {
        await LoadParentCategoriesAsync();
        return View(new Category { IsActive = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(Category category)
    {
        if (!ModelState.IsValid)
        {
            await LoadParentCategoriesAsync();
            return View(category);
        }

        try
        {
            await categoryService.AddAsync(category);
            TempData["SuccessMessage"] = "Thêm danh mục thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await LoadParentCategoriesAsync();
            return View(category);
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditCategory(int id)
    {
        try
        {
            var category = await categoryService.GetAsync(id);
            await LoadParentCategoriesAsync(category.CategoryId);
            return View(category);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCategory(int id, Category category)
    {
        if (id != category.CategoryId)
            return BadRequest();

        if (!ModelState.IsValid)
        {
            await LoadParentCategoriesAsync(category.CategoryId);
            return View(category);
        }

        try
        {
            await categoryService.UpdateAsync(category);
            TempData["SuccessMessage"] = "Cập nhật danh mục thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await LoadParentCategoriesAsync(category.CategoryId);
            return View(category);
        }
    }

    [HttpGet]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            return View(await categoryService.GetAsync(id));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategoryConfirmed(int id)
    {
        try
        {
            await categoryService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Xóa danh mục thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
