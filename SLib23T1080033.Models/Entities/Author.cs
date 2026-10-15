using System;

namespace SLib23T1080033.Models.Entities
{
    /// <summary>
    /// Biểu diễn tác giả của sách trong hệ thống thư viện.
    /// Ánh xạ tương ứng với bảng dbo.Authors.
    /// </summary>
    public class Author
    {
        /// <summary>Mã định danh duy nhất của tác giả.</summary>
        public int AuthorId { get; set; }

        /// <summary>Họ tên tác giả.</summary>
        public string AuthorName { get; set; } = string.Empty;

        /// <summary>Thông tin tiểu sử của tác giả; có thể không có.</summary>
        public string? Biography { get; set; }

        /// <summary>Ngày sinh của tác giả; có thể không có.</summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>Ngày mất của tác giả; có thể không có.</summary>
        public DateTime? DeathDate { get; set; }

        /// <summary>Cho biết tác giả có đang được kích hoạt hay không.</summary>
        public bool IsActive { get; set; }
    }
}
