using Dapper;
using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.DTOs;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.DataLayer.SQLServer;

public class MemberRepository : IMemberRepository
{
    private readonly DbHelper _dbHelper;

    public MemberRepository(DbHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }

    public async Task<MemberPageDTO> GetPageAsync(
        int? memberTypeId,
        string? status,
        string? name,
        int page,
        int pageSize)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            SELECT COUNT(*)
            FROM Members m
            WHERE (@MemberTypeId IS NULL OR m.MemberTypeId = @MemberTypeId)
              AND (@Status IS NULL OR m.MemberStatus = @Status)
              AND (@Name IS NULL OR m.FullName LIKE '%' + @Name + '%');

            SELECT m.*, mt.TypeName AS MemberTypeName
            FROM Members m
            INNER JOIN MemberTypes mt ON mt.MemberTypeId = m.MemberTypeId
            WHERE (@MemberTypeId IS NULL OR m.MemberTypeId = @MemberTypeId)
              AND (@Status IS NULL OR m.MemberStatus = @Status)
              AND (@Name IS NULL OR m.FullName LIKE '%' + @Name + '%')
            ORDER BY m.MemberId DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            """;

        using var results = await connection.QueryMultipleAsync(sql, new
        {
            MemberTypeId = memberTypeId,
            Status = status,
            Name = name,
            Offset = (long)(page - 1) * pageSize,
            PageSize = pageSize
        });

        var totalCount = await results.ReadSingleAsync<int>();
        var members = (await results.ReadAsync<MemberDTO>()).AsList();

        return new MemberPageDTO
        {
            Items = members,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<MemberDTO?> GetAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            SELECT m.*, mt.TypeName AS MemberTypeName
            FROM Members m
            INNER JOIN MemberTypes mt ON mt.MemberTypeId = m.MemberTypeId
            WHERE m.MemberId = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<MemberDTO>(
            sql, new { Id = id });
    }

    public async Task<int> AddAsync(Member member)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            INSERT INTO Members
                (MemberCode, MemberTypeId, FullName, DateOfBirth, Gender,
                 Email, Phone, Address, Organization, RegisteredAt,
                 ExpiredAt, MemberStatus, Notes)
            VALUES
                (@MemberCode, @MemberTypeId, @FullName, @DateOfBirth, @Gender,
                 @Email, @Phone, @Address, @Organization, @RegisteredAt,
                 @ExpiredAt, @MemberStatus, @Notes)
            """;

        return await connection.ExecuteAsync(sql, member);
    }

    public async Task<int> UpdateAsync(Member member)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            UPDATE Members
            SET MemberCode = @MemberCode,
                MemberTypeId = @MemberTypeId,
                FullName = @FullName,
                DateOfBirth = @DateOfBirth,
                Gender = @Gender,
                Email = @Email,
                Phone = @Phone,
                Address = @Address,
                Organization = @Organization,
                Notes = @Notes
            WHERE MemberId = @MemberId
            """;

        return await connection.ExecuteAsync(sql, member);
    }

    public async Task<int> UpdateStatusAsync(int id, string currentStatus, string status)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            UPDATE Members
            SET MemberStatus = @Status
            WHERE MemberId = @Id
              AND MemberStatus = @CurrentStatus
            """;

        return await connection.ExecuteAsync(sql, new
        {
            Id = id,
            Status = status,
            CurrentStatus = currentStatus
        });
    }

    public async Task<int> UpdateExpiryAsync(int id, DateTime expiredAt)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            UPDATE Members
            SET ExpiredAt = @ExpiredAt
            WHERE MemberId = @Id
            """;

        return await connection.ExecuteAsync(sql, new { Id = id, ExpiredAt = expiredAt });
    }

    public async Task<int> DeleteAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            DELETE FROM Members
            WHERE MemberId = @Id
            """;

        return await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<bool> MemberCodeExistsAsync(
        string memberCode,
        int? exceptMemberId = null)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            SELECT CAST(CASE WHEN EXISTS (
                SELECT 1
                FROM Members
                WHERE MemberCode = @MemberCode
                  AND (@ExceptMemberId IS NULL OR MemberId <> @ExceptMemberId)
            ) THEN 1 ELSE 0 END AS BIT)
            """;

        return await connection.ExecuteScalarAsync<bool>(sql, new
        {
            MemberCode = memberCode,
            ExceptMemberId = exceptMemberId
        });
    }

    public async Task<bool> IsInUseAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            SELECT CAST(CASE WHEN EXISTS (
                SELECT 1
                FROM Loans
                WHERE MemberId = @Id
            ) THEN 1 ELSE 0 END AS BIT)
            """;

        return await connection.ExecuteScalarAsync<bool>(sql, new { Id = id });
    }
}
