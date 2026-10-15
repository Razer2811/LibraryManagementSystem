using System;

namespace SLib23T1080033.Models.Entities
{
    /// <summary>
    /// Biểu diễn nhân viên thư viện.
    /// Ánh xạ tương ứng với bảng dbo.Employees.
    /// </summary>
    public class Employee
    {
        /// <summary>Mã định danh duy nhất của nhân viên.</summary>
        public int EmployeeId { get; set; }

        /// <summary>Tên của nhân viên.</summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>Họ của nhân viên.</summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>Ngày sinh của nhân viên; có thể không có.</summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>Địa chỉ email của nhân viên.</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>Số điện thoại của nhân viên; có thể không có.</summary>
        public string? Phone { get; set; }

        /// <summary>Địa chỉ của nhân viên; có thể không có.</summary>
        public string? Address { get; set; }

        /// <summary>Mật khẩu theo cấu trúc hiện tại của cơ sở dữ liệu; có thể không có.</summary>
        public string? Password { get; set; }

        /// <summary>Cho biết nhân viên hiện còn làm việc hay không.</summary>
        public bool IsWorking { get; set; }
    }
}
