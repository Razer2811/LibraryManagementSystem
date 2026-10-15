using System;

namespace SLib23T1080033.Models.Entities
{
    /// <summary>
    /// Biểu diễn thể loại sách, hỗ trợ cấu trúc thể loại cha - con.
    /// Ánh xạ tương ứng với bảng dbo.Categories.
    /// </summary>
    public class Category
    {
        /// <summary>Mã định danh duy nhất của thể loại.</summary>
        public int CategoryId { get; set; }

        /// <summary>Tên thể loại sách.</summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>Mã thể loại cha; null nếu đây là thể loại cấp cao nhất.</summary>
        public int? ParentCategoryId { get; set; }

        /// <summary>Mô tả thể loại; có thể không có.</summary>
        public string? Description { get; set; }

        /// <summary>Cho biết thể loại có đang được kích hoạt hay không.</summary>
        public bool IsActive { get; set; }
    }
}
