using Dapper;
using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.DTOs;

namespace SLib23T1080033.DataLayer.SQLServer;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly DbHelper _dbHelper;

    public EmployeeRepository(DbHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }

    public async Task<EmployeePageDTO> GetPageAsync(
        string? name,
        bool? isWorking,
        int page,
        int pageSize)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            SELECT COUNT(*)
            FROM dbo.Employees e
            WHERE (@IsWorking IS NULL OR e.IsWorking = @IsWorking)
              AND (
                    @Name IS NULL
                    OR CONCAT(e.LastName, N' ', e.FirstName) LIKE N'%' + @Name + N'%'
                    OR e.Email LIKE N'%' + @Name + N'%'
                  );

            SELECT e.EmployeeId, e.FirstName, e.LastName, e.DateOfBirth,
                   e.Email, e.Phone, e.Address, e.IsWorking
            FROM dbo.Employees e
            WHERE (@IsWorking IS NULL OR e.IsWorking = @IsWorking)
              AND (
                    @Name IS NULL
                    OR CONCAT(e.LastName, N' ', e.FirstName) LIKE N'%' + @Name + N'%'
                    OR e.Email LIKE N'%' + @Name + N'%'
                  )
            ORDER BY e.EmployeeId DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            """;

        using var results = await connection.QueryMultipleAsync(sql, new
        {
            Name = name,
            IsWorking = isWorking,
            Offset = (long)(page - 1) * pageSize,
            PageSize = pageSize
        });

        var totalCount = await results.ReadSingleAsync<int>();
        var employees = (await results.ReadAsync<EmployeeDTO>()).AsList();

        return new EmployeePageDTO
        {
            Items = employees,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<EmployeeDTO?> GetAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            SELECT EmployeeId, FirstName, LastName, DateOfBirth,
                   Email, Phone, Address, IsWorking
            FROM dbo.Employees
            WHERE EmployeeId = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<EmployeeDTO>(
            sql, new { Id = id });
    }

    public async Task<int> AddAsync(EmployeeDTO employee)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            INSERT INTO dbo.Employees
                (FirstName, LastName, DateOfBirth, Email, Phone, Address, IsWorking)
            VALUES
                (@FirstName, @LastName, @DateOfBirth, @Email, @Phone, @Address, @IsWorking)
            """;

        return await connection.ExecuteAsync(sql, employee);
    }

    public async Task<int> UpdateAsync(EmployeeDTO employee)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            UPDATE dbo.Employees
            SET FirstName = @FirstName,
                LastName = @LastName,
                DateOfBirth = @DateOfBirth,
                Email = @Email,
                Phone = @Phone,
                Address = @Address
            WHERE EmployeeId = @EmployeeId
            """;

        return await connection.ExecuteAsync(sql, employee);
    }

    public async Task<int> UpdateWorkingStatusAsync(
        int id,
        bool currentValue,
        bool newValue)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            UPDATE dbo.Employees
            SET IsWorking = @NewValue
            WHERE EmployeeId = @Id
              AND IsWorking = @CurrentValue
            """;

        return await connection.ExecuteAsync(sql, new
        {
            Id = id,
            CurrentValue = currentValue,
            NewValue = newValue
        });
    }

    public async Task<int> DeleteAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            DELETE FROM dbo.Employees
            WHERE EmployeeId = @Id
            """;

        return await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<bool> IsInUseAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        const string sql = """
            SELECT CAST(CASE WHEN EXISTS (
                SELECT 1
                FROM dbo.Loans
                WHERE EmployeeId = @Id
            ) THEN 1 ELSE 0 END AS BIT)
            """;

        return await connection.ExecuteScalarAsync<bool>(sql, new { Id = id });
    }
}
