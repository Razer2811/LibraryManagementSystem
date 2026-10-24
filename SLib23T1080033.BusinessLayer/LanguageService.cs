
using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.BusinessLayer;

public class LanguageService
{
    private readonly ILanguageRepository _languageRepository;

    public LanguageService(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository
            ?? throw new ArgumentNullException(
                nameof(languageRepository));
    }

    // Hiển thị danh sách ngôn ngữ
    public async Task<IEnumerable<Language>> ListAsync()
    {
        return await _languageRepository.ListAsync();
    }

    // Lấy thông tin ngôn ngữ theo ID
    public async Task<Language> GetAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID ngôn ngữ không hợp lệ.");

        var language = await _languageRepository.GetAsync(id);

        if (language == null)
            throw new Exception("Không tìm thấy ngôn ngữ.");

        return language;
    }

    // Thêm ngôn ngữ
    public async Task<int> AddAsync(Language language)
    {
        ValidateLanguage(language);

        language.LanguageName = language.LanguageName.Trim();

        return await _languageRepository.AddAsync(language);
    }

    // Cập nhật ngôn ngữ
    public async Task<bool> UpdateAsync(Language language)
    {
        ValidateLanguage(language);

        if (language.LanguageId <= 0)
            throw new Exception("ID ngôn ngữ không hợp lệ.");

        var existing = await _languageRepository.GetAsync(
            language.LanguageId);

        if (existing == null)
            throw new Exception(
                "Không tìm thấy ngôn ngữ cần cập nhật.");

        language.LanguageName = language.LanguageName.Trim();

        var result = await _languageRepository.UpdateAsync(language);

        return result > 0;
    }

    // Xóa ngôn ngữ
    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID ngôn ngữ không hợp lệ.");

        var language = await _languageRepository.GetAsync(id);

        if (language == null)
            throw new Exception("Không tìm thấy ngôn ngữ cần xóa.");

        if (await _languageRepository.InUsedAsync(id))
            throw new Exception(
                "Không thể xóa ngôn ngữ vì đang được sách sử dụng.");

        return await _languageRepository.DeleteAsync(id) > 0;
    }

    // Kiểm tra ngôn ngữ có đang được sử dụng không
    public async Task<bool> InUsedAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID ngôn ngữ không hợp lệ.");

        var language = await _languageRepository.GetAsync(id);

        if (language == null)
            throw new Exception("Không tìm thấy ngôn ngữ.");

        return await _languageRepository.InUsedAsync(id);
    }

    // Kiểm tra dữ liệu trước khi thêm hoặc sửa
    private void ValidateLanguage(Language language)
    {
        if (language == null)
            throw new Exception(
                "Thông tin ngôn ngữ không được để trống.");

        if (string.IsNullOrWhiteSpace(language.LanguageName))
            throw new Exception(
                "Tên ngôn ngữ không được để trống.");

        if (language.LanguageName.Trim().Length > 100)
            throw new Exception(
                "Tên ngôn ngữ không được vượt quá 100 ký tự.");
    }
}
