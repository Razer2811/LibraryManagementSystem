using System.Net.Mail;
using System.Text.RegularExpressions;
using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Constant;
using SLib23T1080033.Models.DTOs;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.BusinessLayer;

public class MemberService
{
    private const int DefaultPageSize = 10;
    private const int MaximumPageSize = 100;

    private readonly IMemberRepository _memberRepository;
    private readonly IMemberTypeRepository _memberTypeRepository;

    public MemberService(
        IMemberRepository memberRepository,
        IMemberTypeRepository memberTypeRepository)
    {
        _memberRepository = memberRepository
            ?? throw new ArgumentNullException(nameof(memberRepository));
        _memberTypeRepository = memberTypeRepository
            ?? throw new ArgumentNullException(nameof(memberTypeRepository));
    }

    public async Task<MemberPageDTO> GetPageAsync(
        int? memberTypeId,
        string? status,
        string? name,
        int page,
        int pageSize)
    {
        if (memberTypeId.HasValue && memberTypeId <= 0)
            throw new ArgumentException("Loại độc giả không hợp lệ.");

        if (!string.IsNullOrWhiteSpace(status) && !IsKnownStatus(status))
            throw new ArgumentException("Trạng thái độc giả không hợp lệ.");

        if (memberTypeId.HasValue &&
            await _memberTypeRepository.GetAsync(memberTypeId.Value) == null)
            throw new ArgumentException("Không tìm thấy loại độc giả.");

        page = Math.Max(page, 1);
        pageSize = pageSize <= 0 ? DefaultPageSize : Math.Min(pageSize, MaximumPageSize);

        return await _memberRepository.GetPageAsync(
            memberTypeId,
            string.IsNullOrWhiteSpace(status) ? null : status.Trim(),
            string.IsNullOrWhiteSpace(name) ? null : name.Trim(),
            page,
            pageSize);
    }

    public async Task<MemberDTO> GetAsync(int id)
    {
        ValidateId(id);

        return await _memberRepository.GetAsync(id)
            ?? throw new KeyNotFoundException("Không tìm thấy độc giả.");
    }

    public async Task AddAsync(MemberDTO member)
    {
        ValidateMember(member);
        NormalizeMember(member);
        await ValidateMemberTypeAsync(member.MemberTypeId);

        if (await _memberRepository.MemberCodeExistsAsync(member.MemberCode))
            throw new InvalidOperationException("Mã độc giả đã tồn tại.");

        member.RegisteredAt = DateTime.Now;
        member.ExpiredAt = null;
        member.MemberStatus = DatabaseConstant.MEMBERS_MEMBERSTATUS_ACTIVE;

        if (await _memberRepository.AddAsync(member) == 0)
            throw new InvalidOperationException("Không thể thêm độc giả.");
    }

    public async Task UpdateAsync(int id, MemberDTO submittedMember)
    {
        ValidateId(id);
        ValidateMember(submittedMember);
        NormalizeMember(submittedMember);

        var existing = await _memberRepository.GetAsync(id)
            ?? throw new KeyNotFoundException("Không tìm thấy độc giả cần cập nhật.");

        var typeChanged = existing.MemberTypeId != submittedMember.MemberTypeId;
        await ValidateMemberTypeAsync(submittedMember.MemberTypeId, !typeChanged);

        if (await _memberRepository.MemberCodeExistsAsync(submittedMember.MemberCode, id))
            throw new InvalidOperationException("Mã độc giả đã tồn tại.");

        existing.MemberCode = submittedMember.MemberCode;
        existing.MemberTypeId = submittedMember.MemberTypeId;
        existing.FullName = submittedMember.FullName;
        existing.DateOfBirth = submittedMember.DateOfBirth;
        existing.Gender = submittedMember.Gender;
        existing.Email = submittedMember.Email;
        existing.Phone = submittedMember.Phone;
        existing.Address = submittedMember.Address;
        existing.Organization = submittedMember.Organization;
        existing.Notes = submittedMember.Notes;

        if (await _memberRepository.UpdateAsync(existing) == 0)
            throw new InvalidOperationException("Không thể cập nhật độc giả.");
    }

    public async Task SetSuspendedAsync(int id, bool suspended)
    {
        ValidateId(id);
        var member = await GetAsync(id);

        var requiredCurrentStatus = suspended
            ? DatabaseConstant.MEMBERS_MEMBERSTATUS_ACTIVE
            : DatabaseConstant.MEMBERS_MEMBERSTATUS_SUSPENDED;
        var newStatus = suspended
            ? DatabaseConstant.MEMBERS_MEMBERSTATUS_SUSPENDED
            : DatabaseConstant.MEMBERS_MEMBERSTATUS_ACTIVE;

        if (member.MemberStatus != requiredCurrentStatus)
            throw new InvalidOperationException(
                suspended
                    ? "Chỉ có thể khóa độc giả đang hoạt động."
                    : "Chỉ có thể mở khóa độc giả đang bị khóa.");

        if (await _memberRepository.UpdateStatusAsync(
            id, requiredCurrentStatus, newStatus) == 0)
            throw new InvalidOperationException(
                "Trạng thái độc giả đã thay đổi. Vui lòng tải lại trang.");
    }

    public async Task RenewCardAsync(int id, DateTime expiredAt)
    {
        ValidateId(id);
        var member = await GetAsync(id);
        var newExpiryDate = expiredAt.Date;
        var currentExpiryDate = member.ExpiredAt?.Date ?? DateTime.Today;

        if (newExpiryDate <= currentExpiryDate)
            throw new ArgumentException(
                "Ngày hết hạn mới phải sau ngày hết hạn hiện tại.");

        if (await _memberRepository.UpdateExpiryAsync(id, newExpiryDate) == 0)
            throw new InvalidOperationException("Không thể gia hạn thẻ độc giả.");
    }

    public async Task DeleteAsync(int id)
    {
        ValidateId(id);
        _ = await GetAsync(id);

        if (await _memberRepository.IsInUseAsync(id))
            throw new InvalidOperationException(
                "Không thể xóa độc giả đã có thông tin mượn sách.");

        if (await _memberRepository.DeleteAsync(id) == 0)
            throw new InvalidOperationException("Không thể xóa độc giả.");
    }

    private async Task ValidateMemberTypeAsync(int memberTypeId, bool allowInactive = false)
    {
        if (memberTypeId <= 0)
            throw new ArgumentException("Vui lòng chọn loại độc giả.");

        var memberType = await _memberTypeRepository.GetAsync(memberTypeId)
            ?? throw new ArgumentException("Loại độc giả không tồn tại.");

        if (!memberType.IsActive && !allowInactive)
            throw new ArgumentException("Không thể gán loại độc giả đã ngừng hoạt động.");
    }

    private static void ValidateMember(Member member)
    {
        ArgumentNullException.ThrowIfNull(member);

        if (string.IsNullOrWhiteSpace(member.MemberCode))
            throw new ArgumentException("Mã độc giả không được để trống.");
        if (member.MemberCode.Trim().Length > 30)
            throw new ArgumentException("Mã độc giả không được vượt quá 30 ký tự.");

        if (string.IsNullOrWhiteSpace(member.FullName))
            throw new ArgumentException("Họ tên độc giả không được để trống.");
        if (member.FullName.Trim().Length > 200)
            throw new ArgumentException("Họ tên không được vượt quá 200 ký tự.");

        if (member.DateOfBirth > DateTime.Today)
            throw new ArgumentException("Ngày sinh không được ở tương lai.");

        if (!string.IsNullOrWhiteSpace(member.Gender) &&
            member.Gender.Trim() is not (
                DatabaseConstant.MEMBERS_GENDER_MALE or
                DatabaseConstant.MEMBERS_GENDER_FEMALE or
                DatabaseConstant.MEMBERS_GENDER_OTHER))
            throw new ArgumentException("Giới tính không hợp lệ.");

        if (!string.IsNullOrWhiteSpace(member.Email))
        {
            if (member.Email.Trim().Length > 254 ||
                !MailAddress.TryCreate(member.Email.Trim(), out var address) ||
                !string.Equals(address.Address, member.Email.Trim(), StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Email không hợp lệ.");
        }

        if (!string.IsNullOrWhiteSpace(member.Phone))
        {
            var phone = member.Phone.Trim();
            var digits = Regex.Replace(phone, @"\D", string.Empty);
            if (phone.Length > 20 || digits.Length is < 7 or > 15 ||
                !Regex.IsMatch(phone, @"^\+?[0-9\s().-]+$"))
                throw new ArgumentException("Số điện thoại không hợp lệ.");
        }

        if (member.Address?.Trim().Length > 300)
            throw new ArgumentException("Địa chỉ không được vượt quá 300 ký tự.");
        if (member.Organization?.Trim().Length > 200)
            throw new ArgumentException("Tên cơ quan/trường học không được vượt quá 200 ký tự.");
        if (member.Notes?.Trim().Length > 500)
            throw new ArgumentException("Ghi chú không được vượt quá 500 ký tự.");
    }

    private static void NormalizeMember(Member member)
    {
        member.MemberCode = member.MemberCode.Trim();
        member.FullName = member.FullName.Trim();
        member.Gender = NormalizeOptional(member.Gender);
        member.Email = NormalizeOptional(member.Email);
        member.Phone = NormalizeOptional(member.Phone);
        member.Address = NormalizeOptional(member.Address);
        member.Organization = NormalizeOptional(member.Organization);
        member.Notes = NormalizeOptional(member.Notes);
    }

    private static string? NormalizeOptional(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static bool IsKnownStatus(string status)
        => status.Trim() is
            DatabaseConstant.MEMBERS_MEMBERSTATUS_ACTIVE or
            DatabaseConstant.MEMBERS_MEMBERSTATUS_SUSPENDED or
            DatabaseConstant.MEMBERS_MEMBERSTATUS_EXPIRED or
            DatabaseConstant.MEMBERS_MEMBERSTATUS_CLOSED;

    private static void ValidateId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Mã độc giả không hợp lệ.");
    }
}
