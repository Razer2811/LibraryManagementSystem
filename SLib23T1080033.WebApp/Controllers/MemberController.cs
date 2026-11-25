using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.DTOs;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.WebApp.Controllers;

public class MemberController(MemberService memberService, MemberTypeService memberTypeService) : Controller
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index(
        int? memberTypeId,
        string? status,
        string? name,
        int page = 1,
        int pageSize = 10)
    {
        try
        {
            var members = await memberService.GetPageAsync(
                memberTypeId, status, name, page, pageSize);

            ViewBag.MemberTypeId = memberTypeId;
            ViewBag.Status = status;
            ViewBag.Name = name?.Trim();
            ViewBag.MemberTypes = new SelectList(
                await memberTypeService.ListAsync(),
                nameof(MemberType.MemberTypeId),
                nameof(MemberType.TypeName),
                memberTypeId);

            return View(members);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            ViewBag.MemberTypeId = memberTypeId;
            ViewBag.Status = status;
            ViewBag.Name = name?.Trim();
            ViewBag.MemberTypes = new SelectList(
                await memberTypeService.ListAsync(),
                nameof(MemberType.MemberTypeId),
                nameof(MemberType.TypeName),
                memberTypeId);

            return View(new MemberPageDTO
            {
                Page = Math.Max(page, 1),
                PageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 100)
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> CreateMember()
    {
        await LoadMemberTypesAsync();
        return View(new MemberDTO());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateMember(MemberDTO member)
    {
        if (!ModelState.IsValid)
        {
            await LoadMemberTypesAsync(member.MemberTypeId);
            return View(member);
        }

        try
        {
            await memberService.AddAsync(member);
            TempData["SuccessMessage"] = "Thêm độc giả thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await LoadMemberTypesAsync(member.MemberTypeId);
            return View(member);
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditMember(int id)
    {
        try
        {
            var member = await memberService.GetAsync(id);
            await LoadMemberTypesAsync(member.MemberTypeId);
            return View(member);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditMember(int id, MemberDTO member)
    {
        if (id != member.MemberId)
            return BadRequest();

        if (!ModelState.IsValid)
        {
            await LoadMemberTypesAsync(member.MemberTypeId);
            return View(member);
        }

        try
        {
            await memberService.UpdateAsync(id, member);
            TempData["SuccessMessage"] = "Cập nhật độc giả thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await LoadMemberTypesAsync(member.MemberTypeId);
            return View(member);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetSuspended(int id, bool suspended)
    {
        try
        {
            await memberService.SetSuspendedAsync(id, suspended);
            TempData["SuccessMessage"] = suspended
                ? "Đã khóa độc giả."
                : "Đã mở khóa độc giả.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RenewCard(int id, DateTime expiredAt)
    {
        try
        {
            await memberService.RenewCardAsync(id, expiredAt);
            TempData["SuccessMessage"] = "Gia hạn thẻ độc giả thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> DeleteMember(int id)
    {
        try
        {
            return View(await memberService.GetAsync(id));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMemberConfirmed(int id)
    {
        try
        {
            await memberService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Xóa độc giả thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadMemberTypesAsync(int? selectedMemberTypeId = null)
    {
        var memberTypes = await memberTypeService.ListAsync();
        ViewBag.MemberTypes = new SelectList(
            memberTypes,
            nameof(MemberType.MemberTypeId),
            nameof(MemberType.TypeName),
            selectedMemberTypeId);
    }
}
