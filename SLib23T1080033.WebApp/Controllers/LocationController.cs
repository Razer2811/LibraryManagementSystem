using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.WebApp.Controllers;

public class LocationController(LocationService locationService) : Controller
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index(string? keyword)
    {
        try
        {
            var locations = await locationService.ListAsync();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                locations = locations.Where(location =>
                    location.LocationCode.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    location.LocationName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    (location.Room?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false));
            }

            ViewBag.Keyword = keyword?.Trim();
            return View(locations);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return View(Enumerable.Empty<Location>());
        }
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
            TempData["SuccessMessage"] = "Thêm vị trí thành công.";
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
            return View(await locationService.GetAsync(id));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditLocation(int id, Location location)
    {
        if (id != location.LocationId)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(location);

        try
        {
            await locationService.UpdateAsync(location);
            TempData["SuccessMessage"] = "Cập nhật vị trí thành công.";
            return RedirectToAction(nameof(Index));
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
            return View(await locationService.GetAsync(id));
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
            await locationService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Xóa vị trí thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
