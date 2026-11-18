using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.WebApp.Controllers;

public class CategoryController(CategoryService categoryService)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? keyword)
    {
        var categories = await categoryService.ListAsync();
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            categories = categories.Where(c =>
                c.CategoryName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                (c.Description?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false));
        }
        ViewBag.Keyword = keyword?.Trim();
        return View(categories);
    }

    // Chuẩn bị danh sách danh mục cha cho form
    private async Task LoadParentCategoriesAsync(int? currentCategoryId = null)
    {
        var categories = await categoryService.ListAsync();

        ViewBag.ParentCategories = categories
            .Where(c => c.CategoryId != currentCategoryId)
            .ToList();
    }

    // Hiển thị form thêm
    [HttpGet]
    public async Task<IActionResult> CreateCategory()
    {
        await LoadParentCategoriesAsync();
        return View();
    }

    // Xử lý thêm
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

            TempData["SuccessMessage"] = "Thêm danh mục thành công!";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await LoadParentCategoriesAsync();
            return View(category);
        }
    }

    // Hiển thị form sửa
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

    // Xử lý sửa
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
            var result = await categoryService.UpdateAsync(category);

            if (result)
            {
                TempData["SuccessMessage"] = "Cập nhật danh mục thành công!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(
                string.Empty, "Không thể cập nhật danh mục.");

            await LoadParentCategoriesAsync(category.CategoryId);
            return View(category);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await LoadParentCategoriesAsync(category.CategoryId);
            return View(category);
        }
    }

    // Hiển thị trang xác nhận xóa
    [HttpGet]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            var category = await categoryService.GetAsync(id);
            return View(category);
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
    public async Task<IActionResult> DeleteCategoryConfirmed(int id)
    {
        try
        {
            var result = await categoryService.DeleteAsync(id);

            if (result)
                TempData["SuccessMessage"] = "Xóa danh mục thành công!";
            else
                TempData["ErrorMessage"] = "Không thể xóa danh mục.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}