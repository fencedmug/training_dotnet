namespace AddressBookService.DataAccess;

using Microsoft.Data.Sqlite;
using System.Data;

public class SqliteDbConnectionProvider : IDbConnectionProvider
{
    private readonly string _connectionStr;

    public SqliteDbConnectionProvider(string connStr)
    {
        _connectionStr = connStr;
    }
    public string DatabaseType { get => IDbConnectionProvider.Sqlite; }

    public string ConnectionString { get => _connectionStr; }

    public IDbConnection GetDbConnection()
    {
        return new SqliteConnection(_connectionStr);
    }
}