using System;

namespace SLib23T1080033.Models.Entities
{
    /// <summary>
    /// Biểu diễn độc giả đăng ký sử dụng dịch vụ thư viện.
    /// Ánh xạ tương ứng với bảng dbo.Members.
    /// </summary>
    public class Member
    {
        /// <summary>Mã định danh duy nhất của độc giả.</summary>
        public int MemberId { get; set; }

        /// <summary>Mã độc giả dùng để nhận diện trong hệ thống.</summary>
        public string MemberCode { get; set; } = string.Empty;

        /// <summary>Mã loại độc giả áp dụng cho độc giả này.</summary>
        public int MemberTypeId { get; set; }

        /// <summary>Họ và tên đầy đủ của độc giả.</summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>Ngày sinh của độc giả; có thể không có.</summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>Giới tính theo cơ sở dữ liệu: Male, Female hoặc Other; có thể không có.</summary>
        public string? Gender { get; set; }

        /// <summary>Địa chỉ email của độc giả; có thể không có.</summary>
        public string? Email { get; set; }

        /// <summary>Số điện thoại của độc giả; có thể không có.</summary>
        public string? Phone { get; set; }

        /// <summary>Địa chỉ liên hệ của độc giả; có thể không có.</summary>
        public string? Address { get; set; }

        /// <summary>Cơ quan, trường học hoặc tổ chức của độc giả; có thể không có.</summary>
        public string? Organization { get; set; }

        /// <summary>Thời điểm đăng ký độc giả.</summary>
        public DateTime RegisteredAt { get; set; }

        /// <summary>Ngày hết hạn thẻ độc giả; null nếu chưa xác định ngày hết hạn.</summary>
        public DateTime? ExpiredAt { get; set; }

        /// <summary>Trạng thái độc giả: Active, Suspended, Expired hoặc Closed.</summary>
        public string MemberStatus { get; set; } = "Active";

        /// <summary>Ghi chú về độc giả; có thể không có.</summary>
        public string? Notes { get; set; }
    }
}
