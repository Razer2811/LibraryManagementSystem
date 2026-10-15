using System;

namespace SLib23T1080033.Models.Entities
{
    /// <summary>
    /// Biểu diễn ngôn ngữ của sách.
    /// Ánh xạ tương ứng với bảng dbo.Languages.
    /// </summary>
    public class Language
    {
        /// <summary>Mã định danh duy nhất của ngôn ngữ.</summary>
        public int LanguageId { get; set; }

        /// <summary>Mã ngôn ngữ, chẳng hạn mã ISO hoặc mã quy ước của hệ thống.</summary>
        public string LanguageCode { get; set; } = string.Empty;

        /// <summary>Tên ngôn ngữ.</summary>
        public string LanguageName { get; set; } = string.Empty;

        /// <summary>Cho biết ngôn ngữ có đang được kích hoạt hay không.</summary>
        public bool IsActive { get; set; }
    }
}
