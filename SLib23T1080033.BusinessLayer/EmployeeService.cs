using System.Net.Mail;
using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.DTOs;

namespace SLib23T1080033.BusinessLayer;

public class EmployeeService
{
    private const int DefaultPageSize = 10;
    private const int MaximumPageSize = 100;

    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<EmployeePageDTO> GetPageAsync(
        string? name,
        bool? isWorking,
        int page,
        int pageSize)
    {
        page = Math.Max(page, 1);
        pageSize = pageSize <= 0 ? DefaultPageSize : Math.Min(pageSize, MaximumPageSize);

        return await _repository.GetPageAsync(
            string.IsNullOrWhiteSpace(name) ? null : name.Trim(),
            isWorking,
            page,
            pageSize);
    }

    public async Task<EmployeeDTO> GetAsync(int id)
    {
        ValidateId(id);

        return await _repository.GetAsync(id)
            ?? throw new KeyNotFoundException("Không tìm thấy nhân viên.");
    }

    public async Task AddAsync(EmployeeDTO employee)
    {
        ValidateEmployee(employee);
        NormalizeEmployee(employee);
        employee.IsWorking = true;

        if (await _repository.AddAsync(employee) == 0)
            throw new InvalidOperationException("Không thể thêm nhân viên.");
    }

    public async Task UpdateAsync(int id, EmployeeDTO submittedEmployee)
    {
        ValidateId(id);
        ValidateEmployee(submittedEmployee);
        NormalizeEmployee(submittedEmployee);

        var existing = await _repository.GetAsync(id)
            ?? throw new KeyNotFoundException("Không tìm thấy nhân viên cần cập nhật.");

        existing.FirstName = submittedEmployee.FirstName;
        existing.LastName = submittedEmployee.LastName;
        existing.DateOfBirth = submittedEmployee.DateOfBirth;
        existing.Email = submittedEmployee.Email;
        existing.Phone = submittedEmployee.Phone;
        existing.Address = submittedEmployee.Address;

        if (await _repository.UpdateAsync(existing) == 0)
            throw new InvalidOperationException("Không thể cập nhật nhân viên.");
    }

    public async Task SetWorkingAsync(int id, bool working)
    {
        ValidateId(id);
        var employee = await GetAsync(id);

        if (employee.IsWorking == working)
            throw new InvalidOperationException(
                working
                    ? "Nhân viên hiện đang làm việc."
                    : "Nhân viên hiện đã ngừng làm việc.");

        if (await _repository.UpdateWorkingStatusAsync(
            id, employee.IsWorking, working) == 0)
            throw new InvalidOperationException(
                "Trạng thái nhân viên đã thay đổi. Vui lòng tải lại trang.");
    }

    public async Task DeleteAsync(int id)
    {
        ValidateId(id);
        _ = await GetAsync(id);

        if (await _repository.IsInUseAsync(id))
            throw new InvalidOperationException(
                "Không thể xóa nhân viên đã lập phiếu mượn sách.");

        if (await _repository.DeleteAsync(id) == 0)
            throw new InvalidOperationException("Không thể xóa nhân viên.");
    }

    private static void ValidateEmployee(EmployeeDTO employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        if (string.IsNullOrWhiteSpace(employee.FirstName))
            throw new ArgumentException("Tên nhân viên không được để trống.");
        if (employee.FirstName.Trim().Length > 50)
            throw new ArgumentException("Tên nhân viên không được vượt quá 50 ký tự.");

        if (string.IsNullOrWhiteSpace(employee.LastName))
            throw new ArgumentException("Họ nhân viên không được để trống.");
        if (employee.LastName.Trim().Length > 100)
            throw new ArgumentException("Họ nhân viên không được vượt quá 100 ký tự.");

        if (employee.DateOfBirth > DateTime.Today)
            throw new ArgumentException("Ngày sinh không được ở tương lai.");

        if (string.IsNullOrWhiteSpace(employee.Email))
            throw new ArgumentException("Email nhân viên không được để trống.");
        if (employee.Email.Trim().Length > 50 ||
            !MailAddress.TryCreate(employee.Email.Trim(), out var address) ||
            !string.Equals(
                address.Address,
                employee.Email.Trim(),
                StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Email nhân viên không hợp lệ.");

        if (employee.Phone?.Trim().Length > 50)
            throw new ArgumentException("Số điện thoại không được vượt quá 50 ký tự.");
        if (employee.Address?.Trim().Length > 255)
            throw new ArgumentException("Địa chỉ không được vượt quá 255 ký tự.");
    }

    private static void NormalizeEmployee(EmployeeDTO employee)
    {
        employee.FirstName = employee.FirstName.Trim();
        employee.LastName = employee.LastName.Trim();
        employee.Email = employee.Email.Trim();
        employee.Phone = NormalizeOptional(employee.Phone);
        employee.Address = NormalizeOptional(employee.Address);
    }

    private static string? NormalizeOptional(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static void ValidateId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Mã nhân viên không hợp lệ.");
    }
}
