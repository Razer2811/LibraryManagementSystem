
using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.WebApp.Controllers;

public class MemberTypeController(MemberTypeService _service) : Controller
{
    public async Task<IActionResult> Index()
    {
        var memberTypes = await _service.ListAsync();
        return View(memberTypes);
    }

    [HttpGet]
    public IActionResult CreateMemberType()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateMemberType(MemberType memberType)
    {
        if (!ModelState.IsValid)
            return View(memberType);

        try
        {
            await _service.AddAsync(memberType);

            TempData["SuccessMessage"] = "Thêm loại độc giả thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(memberType);
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditMemberType(int id)
    {
        try
        {
            var memberType = await _service.GetAsync(id);
            return View(memberType);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditMemberType(
        int id, MemberType memberType)
    {
        if (id != memberType.MemberTypeId)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(memberType);

        try
        {
            var result = await _service.UpdateAsync(memberType);

            if (result)
            {
                TempData["SuccessMessage"] = "Cập nhật loại độc giả thành công!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(
                string.Empty, "Không thể cập nhật loại độc giả.");

            return View(memberType);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(memberType);
        }
    }

    [HttpGet]
    public async Task<IActionResult> DeleteMemberType(int id)
    {
        try
        {
            var memberType = await _service.GetAsync(id);
            return View(memberType);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMemberTypeConfirmed(int id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);

            if (result)
                TempData["SuccessMessage"] = "Xóa loại độc giả thành công!";
            else
                TempData["ErrorMessage"] = "Không thể xóa loại độc giả.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
