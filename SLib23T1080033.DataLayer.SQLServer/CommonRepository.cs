
using Dapper;
using SLib23T1080033.DataLayer.Interfaces;

namespace SLib23T1080033.DataLayer.SQLServer;

public abstract class CommonRepository<T> : ICommonRepository<T>
    where T : class
{
    private readonly DbHelper _dbHelper;
    private readonly string _tableName;
    private readonly string _keyColumn;
    private readonly string[] _columns;
    private readonly string _inUsedSql;

    protected CommonRepository(
        DbHelper dbHelper,
        string tableName,
        string keyColumn,
        string[] columns,
        string inUsedSql)
    {
        _dbHelper = dbHelper;
        _tableName = tableName;
        _keyColumn = keyColumn;
        _columns = columns;
        _inUsedSql = inUsedSql;
    }

    // Lấy danh sách toàn bộ bản ghi
    public async Task<IEnumerable<T>> ListAsync()
    {
        using var connection = _dbHelper.GetConnection();

        string sql = $"SELECT * FROM [{_tableName}]";

        return await connection.QueryAsync<T>(sql);
    }

    // Lấy một bản ghi theo ID
    public async Task<T?> GetAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        string sql = $"""
            SELECT *
            FROM [{_tableName}]
            WHERE [{_keyColumn}] = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<T>(
            sql, new { Id = id });
    }

    // Thêm bản ghi
    public async Task<int> AddAsync(T entity)
    {
        using var connection = _dbHelper.GetConnection();

        string columnNames = string.Join(
            ", ", _columns.Select(c => $"[{c}]"));

        string parameterNames = string.Join(
            ", ", _columns.Select(c => $"@{c}"));

        string sql = $"""
            INSERT INTO [{_tableName}] ({columnNames})
            VALUES ({parameterNames})
            """;

        return await connection.ExecuteAsync(sql, entity);
    }

    // Cập nhật bản ghi
    public async Task<int> UpdateAsync(T entity)
    {
        using var connection = _dbHelper.GetConnection();

        string setClause = string.Join(
            ", ", _columns.Select(c => $"[{c}] = @{c}"));

        string sql = $"""
            UPDATE [{_tableName}]
            SET {setClause}
            WHERE [{_keyColumn}] = @{_keyColumn}
            """;

        return await connection.ExecuteAsync(sql, entity);
    }

    // Xóa bản ghi theo ID
    public async Task<int> DeleteAsync(int id)
    {
        if (await InUsedAsync(id))
            return 0;

        using var connection = _dbHelper.GetConnection();

        string sql = $"""
            DELETE FROM [{_tableName}]
            WHERE [{_keyColumn}] = @Id
            """;

        return await connection.ExecuteAsync(sql, new { Id = id });
    }

    // Kiểm tra bản ghi có được tham chiếu hay không
    public async Task<bool> InUsedAsync(int id)
    {
        using var connection = _dbHelper.GetConnection();

        return await connection.ExecuteScalarAsync<bool>(
            _inUsedSql, new { Id = id });
    }
}
