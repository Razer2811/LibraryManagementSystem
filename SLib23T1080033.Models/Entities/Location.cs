using System;

namespace SLib23T1080033.Models.Entities
{
    /// <summary>
    /// Biểu diễn vị trí lưu trữ sách trong thư viện.
    /// Ánh xạ tương ứng với bảng dbo.Locations.
    /// </summary>
    public class Location
    {
        /// <summary>Mã định danh duy nhất của vị trí.</summary>
        public int LocationId { get; set; }

        /// <summary>Mã vị trí dùng để nhận diện, không trùng lặp.</summary>
        public string LocationCode { get; set; } = string.Empty;

        /// <summary>Tên mô tả của vị trí lưu trữ.</summary>
        public string LocationName { get; set; } = string.Empty;

        /// <summary>Tầng lưu trữ; có thể không có.</summary>
        public string? Floor { get; set; }

        /// <summary>Phòng lưu trữ; có thể không có.</summary>
        public string? Room { get; set; }

        /// <summary>Kệ lưu trữ; có thể không có.</summary>
        public string? Shelf { get; set; }

        /// <summary>Mô tả bổ sung về vị trí; có thể không có.</summary>
        public string? Description { get; set; }

        /// <summary>Cho biết vị trí có đang được sử dụng hay không.</summary>
        public bool IsActive { get; set; }
    }
}
