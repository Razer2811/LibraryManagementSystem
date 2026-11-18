
using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.WebApp.Controllers;

public class LocationController(LocationService locationService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? keyword)
    {
        var locations = await locationService.ListAsync();
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            locations = locations.Where(l =>
                l.LocationCode.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                l.LocationName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                (l.Room?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false));
        }
        ViewBag.Keyword = keyword?.Trim();
        return View(locations);
    }

    [HttpGet]
    public IActionResult CreateLocation()
    {
        return View(new Location { IsActive = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateLocation(Location location)
    {
        if (!ModelState.IsValid)
            return View(location);

        try
        {
            await locationService.AddAsync(location);

            TempData["SuccessMessage"] = "Thêm vị trí thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(location);
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditLocation(int id)
    {
        try
        {
            var location = await locationService.GetAsync(id);
            return View(location);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditLocation(
        int id, Location location)
    {
        if (id != location.LocationId)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(location);

        try
        {
            var result = await locationService.UpdateAsync(location);

            if (result)
            {
                TempData["SuccessMessage"] = "Cập nhật vị trí thành công!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(
                string.Empty, "Không thể cập nhật vị trí.");

            return View(location);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(location);
        }
    }

    [HttpGet]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        try
        {
            var location = await locationService.GetAsync(id);
            return View(location);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteLocationConfirmed(int id)
    {
        try
        {
            var result = await locationService.DeleteAsync(id);

            if (result)
                TempData["SuccessMessage"] = "Xóa vị trí thành công!";
            else
                TempData["ErrorMessage"] = "Không thể xóa vị trí.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
