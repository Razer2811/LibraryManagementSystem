using Dapper;
using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.DataLayer.SQLServer;

public class PublisherRepository : IPublisherRepository
{
    private readonly DbHelper _dbHelper;

    public PublisherRepository(DbHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }

    public async Task<IEnumerable<Publisher>> ListAsync()
    {
        using var connection = _dbHelper.GetConnection();

        string sql = "SELECT * FROM Publishers";

        return await connection.QueryAsync<Publisher>(sql);
    }

    public async Task<Publisher?> GetAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            SELECT *
            FROM Publishers
            WHERE PublisherId = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<Publisher>(
            sql, new { Id = id });
    }

    public async Task<int> AddAsync(Publisher entity)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            INSERT INTO Publishers
                (PublisherName, Address, Phone, Email, Website, IsActive)
            VALUES
                (@PublisherName, @Address, @Phone, @Email, @Website, @IsActive)
            """;

        return await connection.ExecuteAsync(sql, entity);
    }

    public async Task<int> UpdateAsync(Publisher entity)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            UPDATE Publishers
            SET PublisherName = @PublisherName,
                Address = @Address,
                Phone = @Phone,
                Email = @Email,
                Website = @Website,
                IsActive = @IsActive
            WHERE PublisherId = @PublisherId
            """;

        return await connection.ExecuteAsync(sql, entity);
    }

    public async Task<int> DeleteAsync(int id)
    {
        if (await InUsedAsync(id))
            return 0;

        using var connection = _dbHelper.GetConnection();

        string sql = """
            DELETE FROM Publishers
            WHERE PublisherId = @Id
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
                    WHERE PublisherId = @Id
                )
                THEN 1 ELSE 0 END
            AS BIT)
            """;

        return await connection.ExecuteScalarAsync<bool>(
            sql, new { Id = id });
    }
}