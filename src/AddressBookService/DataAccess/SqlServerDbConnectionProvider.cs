namespace AddressBookService.DataAccess;

using Microsoft.Data.SqlClient;
using System.Data;

public class SqlServerDbConnectionProvider : IDbConnectionProvider
{
    private readonly string _connectionStr;

    public SqlServerDbConnectionProvider(string connStr)
    {
        _connectionStr = connStr;
    }

    public string DatabaseType { get => IDbConnectionProvider.SqlServer; }

    public string ConnectionString { get => _connectionStr; }

    public IDbConnection GetDbConnection()
    {
        var connStrBuilder = new SqlConnectionStringBuilder(_connectionStr);
        connStrBuilder.InitialCatalog = "master";

        return new SqlConnection(connStrBuilder.ConnectionString);
    }
}
