namespace AddressBookService.DataAccess;

using System.Data;

public interface IDbConnectionProvider
{
    public const string SqlServer = "sqlserver";
    public const string Sqlite = "sqlite";

    string DatabaseType { get; }
    string ConnectionString { get; }
    IDbConnection GetDbConnection();
}
