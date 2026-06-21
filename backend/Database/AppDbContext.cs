using Microsoft.Data.Sqlite;
using System.Data;

namespace backend.Database;

public class AppDbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString =
            _configuration.GetConnectionString("DefaultConnection");

        return new SqliteConnection(connectionString);
    }
}