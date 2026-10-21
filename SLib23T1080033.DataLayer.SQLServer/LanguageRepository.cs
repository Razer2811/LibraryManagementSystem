using Dapper;
using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.DataLayer.SQLServer;

public class LanguageRepository : ILanguageRepository
{
    private readonly DbHelper _dbHelper;

    public LanguageRepository(DbHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }

    public async Task<IEnumerable<Language>> ListAsync()
    {
        using var connection = _dbHelper.GetConnection();

        string sql = "SELECT * FROM Languages";

        return await connection.QueryAsync<Language>(sql);
    }

    public async Task<Language?> GetAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            SELECT *
            FROM Languages
            WHERE LanguageId = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<Language>(
            sql, new { Id = id });
    }

    public async Task<int> AddAsync(Language entity)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            INSERT INTO Languages
                (LanguageCode, LanguageName, IsActive)
            VALUES
                (@LanguageCode, @LanguageName, @IsActive)
            """;

        return await connection.ExecuteAsync(sql, entity);
    }

    public async Task<int> UpdateAsync(Language entity)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            UPDATE Languages
            SET LanguageCode = @LanguageCode,
                LanguageName = @LanguageName,
                IsActive = @IsActive
            WHERE LanguageId = @LanguageId
            """;

        return await connection.ExecuteAsync(sql, entity);
    }

    public async Task<int> DeleteAsync(int id)
    {
        if (await InUsedAsync(id))
            return 0;

        using var connection = _dbHelper.GetConnection();

        string sql = """
            DELETE FROM Languages
            WHERE LanguageId = @Id
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
                    FROM Books
                    WHERE LanguageId = @Id
                )
                THEN 1 ELSE 0 END
            AS BIT)
            """;

        return await connection.ExecuteScalarAsync<bool>(
            sql, new { Id = id });
    }
}