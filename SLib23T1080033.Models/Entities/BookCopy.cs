using System;

namespace SLib23T1080033.Models.Entities
{
    /// <summary>
    /// Biểu diễn một bản sao vật lý cụ thể của đầu sách trong thư viện.
    /// Ánh xạ tương ứng với bảng dbo.BookCopies.
    /// </summary>
    public class BookCopy
    {
        /// <summary>Mã định danh duy nhất của bản sao sách.</summary>
        public int BookCopyId { get; set; }

        /// <summary>Mã đầu sách mà bản sao này thuộc về.</summary>
        public int BookId { get; set; }

        /// <summary>Mã vạch duy nhất của bản sao sách.</summary>
        public string Barcode { get; set; } = string.Empty;

        /// <summary>Mã vị trí lưu trữ; null nếu chưa gán vị trí.</summary>
        public int? LocationId { get; set; }

        /// <summary>Ngày tiếp nhận hoặc mua bản sao; có thể không có.</summary>
        public DateTime? AcquisitionDate { get; set; }

        /// <summary>Giá tiếp nhận hoặc mua bản sao; có thể không có.</summary>
        public decimal? AcquisitionPrice { get; set; }

        /// <summary>Tình trạng vật lý, ví dụ New, Good, Fair hoặc Damaged.</summary>
        public string CopyCondition { get; set; } = "Good";

        /// <summary>Trạng thái bản sao, ví dụ Available, OnLoan, Reserved, Lost hoặc Withdrawn.</summary>
        public string CopyStatus { get; set; } = "Available";

        /// <summary>Ghi chú về bản sao sách; có thể không có.</summary>
        public string? Notes { get; set; }
    }
}
