using System;

namespace SLib23T1080033.Models.Entities
{
    /// <summary>
    /// Biểu diễn quan hệ giữa đầu sách và tác giả, bao gồm thứ tự tác giả
    /// và vai trò đóng góp. Ánh xạ bảng liên kết dbo.BookAuthors.
    /// Khóa chính của bảng gồm BookId và AuthorId.
    /// </summary>
    public class BookAuthor
    {
        /// <summary>Mã đầu sách trong quan hệ liên kết.</summary>
        public int BookId { get; set; }

        /// <summary>Mã tác giả trong quan hệ liên kết.</summary>
        public int AuthorId { get; set; }

        /// <summary>Thứ tự hiển thị của tác giả trong đầu sách, bắt đầu từ 1.</summary>
        public short AuthorOrder { get; set; }

        /// <summary>Vai trò hoặc phần đóng góp của tác giả; có thể không có.</summary>
        public string? Contribution { get; set; }
    }
}
