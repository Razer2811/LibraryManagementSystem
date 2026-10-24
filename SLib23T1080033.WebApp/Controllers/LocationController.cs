
using Microsoft.AspNetCore.Mvc;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.WebApp.Controllers;

public class LocationController(LocationService _service) : Controller
{
    public async Task<IActionResult> Index()
    {
        var locations = await _service.ListAsync();
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
            await _service.AddAsync(location);

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
            var location = await _service.GetAsync(id);
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
            var result = await _service.UpdateAsync(location);

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
            var location = await _service.GetAsync(id);
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
            var result = await _service.DeleteAsync(id);

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
