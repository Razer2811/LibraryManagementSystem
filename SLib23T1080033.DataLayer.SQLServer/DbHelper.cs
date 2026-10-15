
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SLib23T1080033.DataLayer.SQLServer;

public class DbHelper
{
    private readonly string _connectionString;

    public DbHelper(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString("DefaultConnection")!;
    }

    public SqlConnection GetConnection()
        => new SqlConnection(_connectionString);
}
