using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.BusinessLayer;

public class AuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository
            ?? throw new ArgumentNullException(nameof(authorRepository));
    }

    // Lấy danh sách tác giả
    public async Task<IEnumerable<Author>> ListAsync()
    {
        return await _authorRepository.ListAsync();
    }

    // Lấy tác giả theo ID
    public async Task<Author> GetAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID tác giả không hợp lệ.");

        var author = await _authorRepository.GetAsync(id);

        if (author == null)
            throw new Exception("Không tìm thấy tác giả.");

        return author;
    }

    // Thêm tác giả
    public async Task<int> AddAsync(Author author)
    {
        ValidateAuthor(author);

        author.AuthorName = author.AuthorName.Trim();

        return await _authorRepository.AddAsync(author);
    }

    // Cập nhật tác giả
    public async Task<bool> UpdateAsync(Author author)
    {
        ValidateAuthor(author);

        if (author.AuthorId <= 0)
            throw new Exception("ID tác giả không hợp lệ.");

        var existingAuthor = await _authorRepository.GetAsync(author.AuthorId);

        if (existingAuthor == null)
            throw new Exception("Không tìm thấy tác giả cần cập nhật.");

        author.AuthorName = author.AuthorName.Trim();

        var affectedRows = await _authorRepository.UpdateAsync(author);

        return affectedRows > 0;
    }

    // Xóa tác giả
    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID tác giả không hợp lệ.");

        var author = await _authorRepository.GetAsync(id);

        if (author == null)
            throw new Exception("Không tìm thấy tác giả cần xóa.");

        if (await _authorRepository.InUsedAsync(id))
            throw new Exception(
                "Không thể xóa tác giả vì tác giả đang được sử dụng.");

        var affectedRows = await _authorRepository.DeleteAsync(id);

        return affectedRows > 0;
    }

    // Kiểm tra tác giả đang được sử dụng
    public async Task<bool> InUsedAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID tác giả không hợp lệ.");

        var author = await _authorRepository.GetAsync(id);

        if (author == null)
            throw new Exception("Không tìm thấy tác giả.");

        return await _authorRepository.InUsedAsync(id);
    }

    // Kiểm tra dữ liệu tác giả
    private static void ValidateAuthor(Author author)
    {
        if (author == null)
            throw new Exception("Thông tin tác giả không được để trống.");

        if (string.IsNullOrWhiteSpace(author.AuthorName))
            throw new Exception("Tên tác giả không được để trống.");

        if (author.AuthorName.Trim().Length > 100)
            throw new Exception("Tên tác giả không được vượt quá 100 ký tự.");

        if (author.BirthDate.HasValue &&
            author.BirthDate.Value > DateTime.Now)
        {
            throw new Exception("Ngày sinh không được lớn hơn ngày hiện tại.");
        }

        if (author.BirthDate.HasValue &&
            author.DeathDate.HasValue &&
            author.DeathDate.Value < author.BirthDate.Value)
        {
            throw new Exception("Ngày mất không được trước ngày sinh.");
        }
    }
}