using Dapper;
using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.DataLayer.SQLServer;

public class CategoryRepository : ICategoryRepository
{
    private readonly DbHelper _dbHelper;

    public CategoryRepository(DbHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }

    public async Task<IEnumerable<Category>> ListAsync()
    {
        using var connection = _dbHelper.GetConnection();

        string sql = "SELECT * FROM Categories";

        return await connection.QueryAsync<Category>(sql);
    }

    public async Task<Category?> GetAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            SELECT *
            FROM Categories
            WHERE CategoryId = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<Category>(
            sql, new { Id = id });
    }

    public async Task<int> AddAsync(Category entity)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            INSERT INTO Categories
                (CategoryName, ParentCategoryId, Description, IsActive)
            VALUES
                (@CategoryName, @ParentCategoryId, @Description, @IsActive)
            """;

        return await connection.ExecuteAsync(sql, entity);
    }

    public async Task<int> UpdateAsync(Category entity)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            UPDATE Categories
            SET CategoryName = @CategoryName,
                ParentCategoryId = @ParentCategoryId,
                Description = @Description,
                IsActive = @IsActive
            WHERE CategoryId = @CategoryId
            """;

        return await connection.ExecuteAsync(sql, entity);
    }

    public async Task<int> DeleteAsync(int id)
    {
        if (await InUsedAsync(id))
            return 0;

        using var connection = _dbHelper.GetConnection();

        string sql = """
            DELETE FROM Categories
            WHERE CategoryId = @Id
            """;

        return await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<bool> InUsedAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            SELECT CAST(
                CASE WHEN
                    EXISTS (
                        SELECT 1
                        FROM BookCategories
                        WHERE CategoryId = @Id
                    )
                    OR EXISTS (
                        SELECT 1
                        FROM Categories
                        WHERE ParentCategoryId = @Id
                    )
                THEN 1 ELSE 0 END
            AS BIT)
            """;

        return await connection.ExecuteScalarAsync<bool>(
            sql, new { Id = id });
    }
}