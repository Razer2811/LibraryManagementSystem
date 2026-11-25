using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.DTOs;

namespace SLib23T1080033.WebApp.Controllers;

public class EmployeeController(EmployeeService employeeService) : Controller
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index(
        string? name,
        bool? isWorking,
        int page = 1,
        int pageSize = 10)
    {
        try
        {
            var employees = await employeeService.GetPageAsync(
                name, isWorking, page, pageSize);

            ViewBag.Name = name?.Trim();
            ViewBag.IsWorking = isWorking;
            return View(employees);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            ViewBag.Name = name?.Trim();
            ViewBag.IsWorking = isWorking;
            return View(new EmployeePageDTO
            {
                Page = Math.Max(page, 1),
                PageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 100)
            });
        }
    }

    [HttpGet]
    public IActionResult CreateEmployee()
    {
        return View(new EmployeeDTO());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEmployee(EmployeeDTO employee)
    {
        if (!ModelState.IsValid)
            return View(employee);

        try
        {
            await employeeService.AddAsync(employee);
            TempData["SuccessMessage"] = "Thêm nhân viên thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(employee);
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditEmployee(int id)
    {
        try
        {
            return View(await employeeService.GetAsync(id));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEmployee(int id, EmployeeDTO employee)
    {
        if (id != employee.EmployeeId)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(employee);

        try
        {
            await employeeService.UpdateAsync(id, employee);
            TempData["SuccessMessage"] = "Cập nhật nhân viên thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(employee);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetWorking(int id, bool working)
    {
        try
        {
            await employeeService.SetWorkingAsync(id, working);
            TempData["SuccessMessage"] = working
                ? "Đã khôi phục trạng thái làm việc cho nhân viên."
                : "Đã chuyển nhân viên sang trạng thái ngừng làm việc.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        try
        {
            return View(await employeeService.GetAsync(id));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEmployeeConfirmed(int id)
    {
        try
        {
            await employeeService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Xóa nhân viên thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
