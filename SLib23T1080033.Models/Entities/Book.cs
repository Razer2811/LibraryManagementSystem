using System;

namespace SLib23T1080033.Models.Entities
{
    /// <summary>
    /// Biểu diễn thông tin đầu sách, không phải một bản sao vật lý cụ thể.
    /// Ánh xạ tương ứng với bảng dbo.Books.
    /// </summary>
    public class Book
    {
        /// <summary>Mã định danh duy nhất của đầu sách.</summary>
        public int BookId { get; set; }

        /// <summary>Mã ISBN của đầu sách; có thể không có.</summary>
        public string? ISBN { get; set; }

        /// <summary>Tiêu đề chính của sách.</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>Tiêu đề phụ của sách; có thể không có.</summary>
        public string? Subtitle { get; set; }

        /// <summary>Mã nhà xuất bản; null nếu chưa xác định nhà xuất bản.</summary>
        public int? PublisherId { get; set; }

        /// <summary>Mã ngôn ngữ của sách; null nếu chưa xác định ngôn ngữ.</summary>
        public int? LanguageId { get; set; }

        /// <summary>Năm xuất bản; có thể không có.</summary>
        public short? PublicationYear { get; set; }

        /// <summary>Thông tin lần xuất bản hoặc ấn bản; có thể không có.</summary>
        public string? Edition { get; set; }

        /// <summary>Mô tả nội dung sách; có thể không có.</summary>
        public string? Description { get; set; }

        /// <summary>Số trang của sách; có thể không có.</summary>
        public int? PageCount { get; set; }

        /// <summary>Đường dẫn hoặc tên tệp ảnh bìa; có thể không có.</summary>
        public string? CoverImagePath { get; set; }

        /// <summary>Cho biết đầu sách có đang được kích hoạt hay không.</summary>
        public bool IsActive { get; set; }

        /// <summary>Thời điểm tạo bản ghi đầu sách.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>Thời điểm cập nhật bản ghi gần nhất; null nếu chưa cập nhật.</summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
