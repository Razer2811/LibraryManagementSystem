using System;

namespace SLib23T1080033.Models.Entities
{
    /// <summary>
    /// Biểu diễn nhà xuất bản trong hệ thống quản lý thư viện.
    /// Ánh xạ tương ứng với bảng dbo.Publishers trong cơ sở dữ liệu.
    /// </summary>
    public class Publisher
    {
        /// <summary>Mã định danh duy nhất của nhà xuất bản.</summary>
        public int PublisherId { get; set; }

        /// <summary>Tên nhà xuất bản.</summary>
        public string PublisherName { get; set; } = string.Empty;

        /// <summary>Địa chỉ của nhà xuất bản; có thể không có.</summary>
        public string? Address { get; set; }

        /// <summary>Số điện thoại liên hệ; có thể không có.</summary>
        public string? Phone { get; set; }

        /// <summary>Địa chỉ email liên hệ; có thể không có.</summary>
        public string? Email { get; set; }

        /// <summary>Địa chỉ trang web của nhà xuất bản; có thể không có.</summary>
        public string? Website { get; set; }

        /// <summary>Cho biết nhà xuất bản có đang được kích hoạt hay không.</summary>
        public bool IsActive { get; set; }

        /// <summary>Thời điểm tạo bản ghi nhà xuất bản.</summary>
        public DateTime CreatedAt { get; set; }
    }
}
