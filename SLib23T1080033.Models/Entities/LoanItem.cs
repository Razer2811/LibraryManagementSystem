using System;

namespace SLib23T1080033.Models.Entities
{
    /// <summary>
    /// Biểu diễn một dòng chi tiết trong phiếu mượn, gắn với một bản sao sách.
    /// Ánh xạ tương ứng với bảng dbo.LoanItems.
    /// </summary>
    public class LoanItem
    {
        /// <summary>Mã định danh duy nhất của dòng chi tiết phiếu mượn.</summary>
        public int LoanItemId { get; set; }

        /// <summary>Mã phiếu mượn chứa dòng chi tiết này.</summary>
        public int LoanId { get; set; }

        /// <summary>Mã bản sao sách được mượn.</summary>
        public int BookCopyId { get; set; }

        /// <summary>Thời điểm đến hạn trả bản sao sách.</summary>
        public DateTime DueAt { get; set; }

        /// <summary>Thời điểm trả sách hoặc ghi nhận mất sách; null khi chưa hoàn tất.</summary>
        public DateTime? ReturnedAt { get; set; }

        /// <summary>Trạng thái dòng mượn: Borrowed, Returned hoặc Lost.</summary>
        public string ItemStatus { get; set; } = "Borrowed";

        /// <summary>Tình trạng sách khi trả: New, Good, Fair hoặc Damaged; có thể không có.</summary>
        public string? ReturnCondition { get; set; }

        /// <summary>Ghi chú cho dòng chi tiết mượn; có thể không có.</summary>
        public string? Notes { get; set; }
    }
}
