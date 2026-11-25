using SLib23T1080033.Models.DTOs;

namespace SLib23T1080033.DataLayer.Interfaces;

public interface IEmployeeRepository
{
    Task<EmployeePageDTO> GetPageAsync(
        string? name,
        bool? isWorking,
        int page,
        int pageSize);

    Task<EmployeeDTO?> GetAsync(int id);
    Task<int> AddAsync(EmployeeDTO employee);
    Task<int> UpdateAsync(EmployeeDTO employee);
    Task<int> UpdateWorkingStatusAsync(int id, bool currentValue, bool newValue);
    Task<int> DeleteAsync(int id);
    Task<bool> IsInUseAsync(int id);
}
