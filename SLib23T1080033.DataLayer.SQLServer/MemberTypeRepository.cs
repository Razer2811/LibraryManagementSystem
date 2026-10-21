using Dapper;
using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.DataLayer.SQLServer;

public class MemberTypeRepository : IMemberTypeRepository
{
    private readonly DbHelper _dbHelper;

    public MemberTypeRepository(DbHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }

    public async Task<IEnumerable<MemberType>> ListAsync()
    {
        using var connection = _dbHelper.GetConnection();

        string sql = "SELECT * FROM MemberTypes";

        return await connection.QueryAsync<MemberType>(sql);
    }

    public async Task<MemberType?> GetAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            SELECT *
            FROM MemberTypes
            WHERE MemberTypeId = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<MemberType>(
            sql, new { Id = id });
    }

    public async Task<int> AddAsync(MemberType entity)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            INSERT INTO MemberTypes
                (TypeName, MaxActiveLoans, LoanPeriodDays,
                 MaxRenewals, IsActive)
            VALUES
                (@TypeName, @MaxActiveLoans, @LoanPeriodDays,
                 @MaxRenewals, @IsActive)
            """;

        return await connection.ExecuteAsync(sql, entity);
    }

    public async Task<int> UpdateAsync(MemberType entity)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            UPDATE MemberTypes
            SET TypeName = @TypeName,
                MaxActiveLoans = @MaxActiveLoans,
                LoanPeriodDays = @LoanPeriodDays,
                MaxRenewals = @MaxRenewals,
                IsActive = @IsActive
            WHERE MemberTypeId = @MemberTypeId
            """;

        return await connection.ExecuteAsync(sql, entity);
    }

    public async Task<int> DeleteAsync(int id)
    {
        if (await InUsedAsync(id))
            return 0;

        using var connection = _dbHelper.GetConnection();

        string sql = """
            DELETE FROM MemberTypes
            WHERE MemberTypeId = @Id
            """;

        return await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<bool> InUsedAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            SELECT CAST(
                CASE WHEN EXISTS (
                    SELECT 1
                    FROM Members
                    WHERE MemberTypeId = @Id
                )
                THEN 1 ELSE 0 END
            AS BIT)
            """;

        return await connection.ExecuteScalarAsync<bool>(
            sql, new { Id = id });
    }
}