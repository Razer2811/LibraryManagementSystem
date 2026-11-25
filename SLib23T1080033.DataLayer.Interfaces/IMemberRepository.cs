using SLib23T1080033.Models.DTOs;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.DataLayer.Interfaces;

public interface IMemberRepository
{
    Task<MemberPageDTO> GetPageAsync(
        int? memberTypeId,
        string? status,
        string? name,
        int page,
        int pageSize);

    Task<MemberDTO?> GetAsync(int id);
    Task<int> AddAsync(Member member);
    Task<int> UpdateAsync(Member member);
    Task<int> UpdateStatusAsync(int id, string currentStatus, string status);
    Task<int> UpdateExpiryAsync(int id, DateTime expiredAt);
    Task<int> DeleteAsync(int id);
    Task<bool> MemberCodeExistsAsync(string memberCode, int? exceptMemberId = null);
    Task<bool> IsInUseAsync(int id);
}
