using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.WebApp.Controllers;

public class MemberTypeController(MemberTypeService memberTypeService) : Controller
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index(string? keyword)
    {
        try
        {
            var memberTypes = await memberTypeService.ListAsync();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                memberTypes = memberTypes.Where(memberType =>
                    memberType.TypeName.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            ViewBag.Keyword = keyword?.Trim();
            return View(memberTypes);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return View(Enumerable.Empty<MemberType>());
        }
    }

    [HttpGet]
    public IActionResult CreateMemberType()
    {
        return View(new MemberType { IsActive = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateMemberType(MemberType memberType)
    {
        if (!ModelState.IsValid)
            return View(memberType);

        try
        {
            await memberTypeService.AddAsync(memberType);
            TempData["SuccessMessage"] = "Thêm loại độc giả thành công.";
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
            return View(await memberTypeService.GetAsync(id));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditMemberType(int id, MemberType memberType)
    {
        if (id != memberType.MemberTypeId)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(memberType);

        try
        {
            await memberTypeService.UpdateAsync(memberType);
            TempData["SuccessMessage"] = "Cập nhật loại độc giả thành công.";
            return RedirectToAction(nameof(Index));
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
            return View(await memberTypeService.GetAsync(id));
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
            await memberTypeService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Xóa loại độc giả thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
