using System;

namespace SLib23T1080033.Models.Entities
{
    /// <summary>
    /// Biểu diễn loại độc giả và các giới hạn mượn sách tương ứng.
    /// Ánh xạ tương ứng với bảng dbo.MemberTypes.
    /// </summary>
    public class MemberType
    {
        /// <summary>Mã định danh duy nhất của loại độc giả.</summary>
        public int MemberTypeId { get; set; }

        /// <summary>Tên loại độc giả.</summary>
        public string TypeName { get; set; } = string.Empty;

        /// <summary>Số lượng lượt mượn đang hoạt động tối đa mà loại độc giả được phép có.</summary>
        public int MaxActiveLoans { get; set; }

        /// <summary>Số ngày mượn tiêu chuẩn cho mỗi lượt mượn.</summary>
        public int LoanPeriodDays { get; set; }

        /// <summary>Số lần gia hạn tối đa được phép.</summary>
        public int MaxRenewals { get; set; }

        /// <summary>Cho biết loại độc giả có đang được kích hoạt hay không.</summary>
        public bool IsActive { get; set; }
    }
}
