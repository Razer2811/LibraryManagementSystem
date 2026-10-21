using Dapper;
using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.DataLayer.SQLServer;

public class LocationRepository : ILocationRepository
{
    private readonly DbHelper _dbHelper;

    public LocationRepository(DbHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }

    public async Task<IEnumerable<Location>> ListAsync()
    {
        using var connection = _dbHelper.GetConnection();

        string sql = "SELECT * FROM Locations";

        return await connection.QueryAsync<Location>(sql);
    }

    public async Task<Location?> GetAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            SELECT *
            FROM Locations
            WHERE LocationId = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<Location>(
            sql, new { Id = id });
    }

    public async Task<int> AddAsync(Location entity)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            INSERT INTO Locations
                (LocationCode, LocationName, Floor, Room,
                 Shelf, Description, IsActive)
            VALUES
                (@LocationCode, @LocationName, @Floor, @Room,
                 @Shelf, @Description, @IsActive)
            """;

        return await connection.ExecuteAsync(sql, entity);
    }

    public async Task<int> UpdateAsync(Location entity)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = """
            UPDATE Locations
            SET LocationCode = @LocationCode,
                LocationName = @LocationName,
                Floor = @Floor,
                Room = @Room,
                Shelf = @Shelf,
                Description = @Description,
                IsActive = @IsActive
            WHERE LocationId = @LocationId
            """;

        return await connection.ExecuteAsync(sql, entity);
    }

    public async Task<int> DeleteAsync(int id)
    {
        if (await InUsedAsync(id))
            return 0;

        using var connection = _dbHelper.GetConnection();

        string sql = """
            DELETE FROM Locations
            WHERE LocationId = @Id
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
                    FROM BookCopies
                    WHERE LocationId = @Id
                )
                THEN 1 ELSE 0 END
            AS BIT)
            """;

        return await connection.ExecuteScalarAsync<bool>(
            sql, new { Id = id });
    }
}