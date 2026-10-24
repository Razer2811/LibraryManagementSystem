using Dapper;
using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.DataLayer.SQLServer;

public class AuthorRepository : IAuthorRepository
{
    private readonly DbHelper _dbHelper;

    public AuthorRepository(DbHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }

    public async Task<IEnumerable<Author>> ListAsync()
    {
        using var connection = _dbHelper.GetConnection();

        string sql = "SELECT * FROM Authors";

        return await connection.QueryAsync<Author>(sql);
    }

    public async Task<Author?> GetAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            SELECT *
            FROM Authors
            WHERE AuthorId = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<Author>(
            sql, new { Id = id });
    }

    public async Task<int> AddAsync(Author entity)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            INSERT INTO Authors
                (AuthorName, Biography, BirthDate, DeathDate, IsActive)
            VALUES
                (@AuthorName, @Biography, @BirthDate, @DeathDate, @IsActive)
            """;

        return await connection.ExecuteAsync(sql, entity);
    }

    public async Task<int> UpdateAsync(Author entity)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            UPDATE Authors
            SET AuthorName = @AuthorName,
                Biography = @Biography,
                BirthDate = @BirthDate,
                DeathDate = @DeathDate,
                IsActive = @IsActive
            WHERE AuthorId = @AuthorId
            """;

        return await connection.ExecuteAsync(sql, entity);
    }

    public async Task<int> DeleteAsync(int id)
    {
        if (await InUsedAsync(id))
            return 0;

        using var connection = _dbHelper.GetConnection();

        string sql = """
            DELETE FROM Authors
            WHERE AuthorId = @Id
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
                    FROM BookAuthors
                    WHERE AuthorId = @Id
                )
                THEN 1 ELSE 0 END
            AS BIT)
            """;

        return await connection.ExecuteScalarAsync<bool>(
            sql, new { Id = id });
    }
}