using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.BusinessLayer;

public class LocationService
{
    private readonly ILocationRepository _locationRepository;

    public LocationService(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository
            ?? throw new ArgumentNullException(
                nameof(locationRepository));
    }

    // Lấy danh sách vị trí
    public async Task<IEnumerable<Location>> ListAsync()
    {
        return await _locationRepository.ListAsync();
    }

    // Lấy thông tin vị trí theo ID
    public async Task<Location> GetAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID vị trí không hợp lệ.");

        var location = await _locationRepository.GetAsync(id);

        if (location == null)
            throw new Exception("Không tìm thấy vị trí.");

        return location;
    }

    // Thêm vị trí
    public async Task<int> AddAsync(Location location)
    {
        ValidateLocation(location);

        location.LocationName = location.LocationName.Trim();

        return await _locationRepository.AddAsync(location);
    }

    // Cập nhật vị trí
    public async Task<bool> UpdateAsync(Location location)
    {
        ValidateLocation(location);

        if (location.LocationId <= 0)
            throw new Exception("ID vị trí không hợp lệ.");

        var existing = await _locationRepository.GetAsync(
            location.LocationId);

        if (existing == null)
            throw new Exception("Không tìm thấy vị trí cần cập nhật.");

        location.LocationName = location.LocationName.Trim();

        var result = await _locationRepository.UpdateAsync(location);

        return result > 0;
    }

    // Xóa vị trí
    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID vị trí không hợp lệ.");

        var location = await _locationRepository.GetAsync(id);

        if (location == null)
            throw new Exception("Không tìm thấy vị trí cần xóa.");

        if (await _locationRepository.InUsedAsync(id))
            throw new Exception(
                "Không thể xóa vị trí vì đang được bản sao sách sử dụng.");

        return await _locationRepository.DeleteAsync(id) > 0;
    }

    // Kiểm tra vị trí có đang được sử dụng không
    public async Task<bool> InUsedAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID vị trí không hợp lệ.");

        var location = await _locationRepository.GetAsync(id);

        if (location == null)
            throw new Exception("Không tìm thấy vị trí.");

        return await _locationRepository.InUsedAsync(id);
    }

    // Kiểm tra dữ liệu vị trí
    private void ValidateLocation(Location location)
    {
        if (location == null)
            throw new Exception(
                "Thông tin vị trí không được để trống.");

        if (string.IsNullOrWhiteSpace(location.LocationName))
            throw new Exception(
                "Tên vị trí không được để trống.");

        if (location.LocationName.Trim().Length > 100)
            throw new Exception(
                "Tên vị trí không được vượt quá 100 ký tự.");
    }
}