using System;

namespace SLib23T1080033.Models.Entities
{
    /// <summary>
    /// Biểu diễn một phiếu mượn sách của độc giả.
    /// Ánh xạ tương ứng với bảng dbo.Loans.
    /// </summary>
    public class Loan
    {
        /// <summary>Mã định danh duy nhất của phiếu mượn.</summary>
        public int LoanId { get; set; }

        /// <summary>Mã phiếu mượn dùng để nhận diện, không trùng lặp.</summary>
        public string LoanCode { get; set; } = string.Empty;

        /// <summary>Mã độc giả thực hiện lượt mượn.</summary>
        public int MemberId { get; set; }

        /// <summary>Mã nhân viên xử lý phiếu mượn; có thể không có.</summary>
        public int? EmployeeId { get; set; }

        /// <summary>Thời điểm lập hoặc bắt đầu lượt mượn.</summary>
        public DateTime BorrowedAt { get; set; }

        /// <summary>Trạng thái phiếu mượn: Active, Completed hoặc Cancelled.</summary>
        public string LoanStatus { get; set; } = "Active";

        /// <summary>Ghi chú về phiếu mượn; có thể không có.</summary>
        public string? Notes { get; set; }
    }
}
