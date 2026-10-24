
using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.BusinessLayer;

public class LocationService
{
    private readonly ILocationRepository _repository;

    public LocationService(ILocationRepository repository)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<Location>> ListAsync()
    {
        return await _repository.ListAsync();
    }

    public async Task<Location> GetAsync(int id)
    {
        if (id <= 0)
            throw new Exception("Mã vị trí không hợp lệ.");

        var location = await _repository.GetAsync(id);

        if (location == null)
            throw new Exception("Không tìm thấy vị trí.");

        return location;
    }

    public async Task<int> AddAsync(Location location)
    {
        ValidateLocation(location);

        NormalizeLocation(location);

        return await _repository.AddAsync(location);
    }

    public async Task<bool> UpdateAsync(Location location)
    {
        ValidateLocation(location);

        if (location.LocationId <= 0)
            throw new Exception("Mã vị trí không hợp lệ.");

        var existing = await _repository.GetAsync(location.LocationId);

        if (existing == null)
            throw new Exception("Không tìm thấy vị trí cần cập nhật.");

        NormalizeLocation(location);

        var result = await _repository.UpdateAsync(location);

        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            throw new Exception("Mã vị trí không hợp lệ.");

        var location = await _repository.GetAsync(id);

        if (location == null)
            throw new Exception("Không tìm thấy vị trí cần xóa.");

        if (await _repository.InUsedAsync(id))
        {
            throw new Exception(
                "Không thể xóa vị trí vì đang được bản sao sách sử dụng.");
        }

        return await _repository.DeleteAsync(id) > 0;
    }

    public async Task<bool> InUsedAsync(int id)
    {
        if (id <= 0)
            throw new Exception("Mã vị trí không hợp lệ.");

        return await _repository.InUsedAsync(id);
    }

    private static void ValidateLocation(Location location)
    {
        if (location == null)
            throw new Exception("Thông tin vị trí không được để trống.");

        if (string.IsNullOrWhiteSpace(location.LocationCode))
            throw new Exception("Mã vị trí không được để trống.");

        if (string.IsNullOrWhiteSpace(location.LocationName))
            throw new Exception("Tên vị trí không được để trống.");
    }

    private static void NormalizeLocation(Location location)
    {
        location.LocationCode = location.LocationCode.Trim();
        location.LocationName = location.LocationName.Trim();

        location.Floor = NormalizeOptionalText(location.Floor);
        location.Room = NormalizeOptionalText(location.Room);
        location.Shelf = NormalizeOptionalText(location.Shelf);
        location.Description = NormalizeOptionalText(location.Description);
    }

    private static string? NormalizeOptionalText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
