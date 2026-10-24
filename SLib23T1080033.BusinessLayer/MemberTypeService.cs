
using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.BusinessLayer;

public class MemberTypeService
{
    private readonly IMemberTypeRepository _repository;

    public MemberTypeService(IMemberTypeRepository repository)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<MemberType>> ListAsync()
    {
        return await _repository.ListAsync();
    }

    public async Task<MemberType> GetAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID loại độc giả không hợp lệ.");

        var memberType = await _repository.GetAsync(id);

        if (memberType == null)
            throw new Exception("Không tìm thấy loại độc giả.");

        return memberType;
    }

    public async Task<int> AddAsync(MemberType memberType)
    {
        ValidateMemberType(memberType);

        memberType.TypeName = memberType.TypeName.Trim();

        return await _repository.AddAsync(memberType);
    }

    public async Task<bool> UpdateAsync(MemberType memberType)
    {
        ValidateMemberType(memberType);

        if (memberType.MemberTypeId <= 0)
            throw new Exception("ID loại độc giả không hợp lệ.");

        var existing = await _repository.GetAsync(
            memberType.MemberTypeId);

        if (existing == null)
            throw new Exception("Không tìm thấy loại độc giả cần cập nhật.");

        memberType.TypeName = memberType.TypeName.Trim();

        return await _repository.UpdateAsync(memberType) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID loại độc giả không hợp lệ.");

        var memberType = await _repository.GetAsync(id);

        if (memberType == null)
            throw new Exception("Không tìm thấy loại độc giả cần xóa.");

        if (await _repository.InUsedAsync(id))
            throw new Exception(
                "Không thể xóa loại độc giả vì đang được độc giả sử dụng.");

        return await _repository.DeleteAsync(id) > 0;
    }

    public async Task<bool> InUsedAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID loại độc giả không hợp lệ.");

        return await _repository.InUsedAsync(id);
    }

    private static void ValidateMemberType(MemberType memberType)
    {
        if (memberType == null)
            throw new Exception("Thông tin loại độc giả không được để trống.");

        if (string.IsNullOrWhiteSpace(memberType.TypeName))
            throw new Exception("Tên loại độc giả không được để trống.");

        if (memberType.TypeName.Trim().Length > 100)
            throw new Exception("Tên loại độc giả không được vượt quá 100 ký tự.");

        if (memberType.MaxActiveLoans < 0)
            throw new Exception("Số sách mượn tối đa không được âm.");

        if (memberType.LoanPeriodDays <= 0)
            throw new Exception("Thời hạn mượn phải lớn hơn 0.");

        if (memberType.MaxRenewals < 0)
            throw new Exception("Số lần gia hạn không được âm.");
    }
}
