using System;

namespace SLib23T1080033.Models.Entities
{
    /// <summary>
    /// Biểu diễn quan hệ phân loại giữa đầu sách và thể loại.
    /// Ánh xạ bảng liên kết dbo.BookCategories.
    /// Khóa chính của bảng gồm BookId và CategoryId.
    /// </summary>
    public class BookCategory
    {
        /// <summary>Mã đầu sách được gán thể loại.</summary>
        public int BookId { get; set; }

        /// <summary>Mã thể loại được gán cho đầu sách.</summary>
        public int CategoryId { get; set; }
    }
}
