
namespace SLib23T1080033.DataLayer.Interfaces;

/// <summary>
/// Định nghĩa các phép xử lý dữ liệu CRUD dùng chung cho các bảng.
/// </summary>
/// <typeparam name="T">Kiểu Entity cần xử lý.</typeparam>
public interface ICommonRepository<T> where T : class
{
    /// <summary>
    /// Lấy danh sách toàn bộ bản ghi.
    /// </summary>
    Task<IEnumerable<T>> ListAsync();

    /// <summary>
    /// Lấy một bản ghi theo ID.
    /// </summary>
    /// <param name="id">ID của bản ghi.</param>
    Task<T?> GetAsync(int id);

    /// <summary>
    /// Thêm một bản ghi mới.
    /// </summary>
    /// <param name="entity">Bản ghi cần thêm.</param>
    Task<int> AddAsync(T entity);

    /// <summary>
    /// Cập nhật một bản ghi.
    /// </summary>
    /// <param name="entity">Bản ghi cần cập nhật.</param>
    Task<int> UpdateAsync(T entity);

    /// <summary>
    /// Xóa một bản ghi theo ID.
    /// </summary>
    /// <param name="id">ID của bản ghi cần xóa.</param>
    Task<int> DeleteAsync(int id);

    /// <summary>
    /// Kiểm tra bản ghi có đang được dữ liệu khác tham chiếu hay không.
    /// </summary>
    /// <param name="id">ID của bản ghi cần kiểm tra.</param>
    Task<bool> InUsedAsync(int id);
}