using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.BusinessLayer;

public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository
            ?? throw new ArgumentNullException(
                nameof(categoryRepository));
    }

    // Lấy danh sách danh mục
    public async Task<IEnumerable<Category>> ListAsync()
    {
        return await _categoryRepository.ListAsync();
    }

    // Lấy một danh mục theo ID
    public async Task<Category> GetAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID danh mục không hợp lệ.");

        var category = await _categoryRepository.GetAsync(id);

        if (category == null)
            throw new Exception("Không tìm thấy danh mục.");

        return category;
    }

    // Thêm danh mục
    public async Task<int> AddAsync(Category category)
    {
        ValidateCategory(category);

        await ValidateParentCategoryAsync(
            category.ParentCategoryId, null);

        category.CategoryName = category.CategoryName.Trim();

        return await _categoryRepository.AddAsync(category);
    }

    // Cập nhật danh mục
    public async Task<bool> UpdateAsync(Category category)
    {
        ValidateCategory(category);

        if (category.CategoryId <= 0)
            throw new Exception("ID danh mục không hợp lệ.");

        var existing = await _categoryRepository.GetAsync(
            category.CategoryId);

        if (existing == null)
            throw new Exception(
                "Không tìm thấy danh mục cần cập nhật.");

        await ValidateParentCategoryAsync(
            category.ParentCategoryId, category.CategoryId);

        category.CategoryName = category.CategoryName.Trim();

        var result = await _categoryRepository.UpdateAsync(category);

        return result > 0;
    }

    // Xóa danh mục
    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID danh mục không hợp lệ.");

        var category = await _categoryRepository.GetAsync(id);

        if (category == null)
            throw new Exception("Không tìm thấy danh mục cần xóa.");

        if (await _categoryRepository.InUsedAsync(id))
            throw new Exception(
                "Không thể xóa danh mục vì đang được sách sử dụng " +
                "hoặc còn danh mục con.");

        return await _categoryRepository.DeleteAsync(id) > 0;
    }

    // Kiểm tra danh mục có đang được sử dụng không
    public async Task<bool> InUsedAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID danh mục không hợp lệ.");

        var category = await _categoryRepository.GetAsync(id);

        if (category == null)
            throw new Exception("Không tìm thấy danh mục.");

        return await _categoryRepository.InUsedAsync(id);
    }

    // Kiểm tra dữ liệu danh mục
    private void ValidateCategory(Category category)
    {
        if (category == null)
            throw new Exception(
                "Thông tin danh mục không được để trống.");

        if (string.IsNullOrWhiteSpace(category.CategoryName))
            throw new Exception(
                "Tên danh mục không được để trống.");

        if (category.CategoryName.Trim().Length > 100)
            throw new Exception(
                "Tên danh mục không được vượt quá 100 ký tự.");
    }

    // Kiểm tra danh mục cha có hợp lệ không
    private async Task ValidateParentCategoryAsync(
        int? parentCategoryId, int? currentCategoryId)
    {
        // Không chọn danh mục cha: đây là danh mục cấp cao nhất
        if (parentCategoryId == null)
            return;

        if (parentCategoryId <= 0)
            throw new Exception("ID danh mục cha không hợp lệ.");

        // Không cho danh mục làm cha của chính nó
        if (parentCategoryId == currentCategoryId)
            throw new Exception(
                "Danh mục không thể là danh mục cha của chính nó.");

        var parent = await _categoryRepository.GetAsync(
            parentCategoryId.Value);

        if (parent == null)
            throw new Exception("Danh mục cha không tồn tại.");

        // Không cho chọn một danh mục con/cháu làm danh mục cha,
        // vì điều này sẽ tạo ra vòng lặp trong cây danh mục.
        if (currentCategoryId.HasValue)
        {
            var categories = (
                await _categoryRepository.ListAsync()).ToList();

            var currentParentId = parentCategoryId;

            while (currentParentId.HasValue)
            {
                if (currentParentId.Value == currentCategoryId.Value)
                    throw new Exception(
                        "Không thể chọn danh mục con làm danh mục cha.");

                var currentParent = categories.FirstOrDefault(
                    c => c.CategoryId == currentParentId.Value);

                if (currentParent == null)
                    break;

                currentParentId = currentParent.ParentCategoryId;
            }
        }
    }
}