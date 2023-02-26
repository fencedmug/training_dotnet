using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Threading.Tasks;

namespace AddressBookService.DataAccess;

public class SqliteSetup : IDatabaseSetup
{
    private readonly ILogger<SqliteSetup> _log;
    private readonly IDbConnectionProvider _dbConnProv;

    public SqliteSetup(ILogger<SqliteSetup> log, IDbConnectionProvider dbConnProv)
    {
        _log = log;
        _dbConnProv = dbConnProv;
    }

    public async Task InitializeAsync()
    {
        using var dbConn = _dbConnProv.GetDbConnection();

        await CreateUsers(dbConn);
        var createdTable = await CreateAddresses(dbConn);

        if (!createdTable)
        {
            return;
        }

        // seed data
        var query = "SELECT COUNT(UserId) from Users";
        var results = dbConn.Query<int>(query).Single();
        if (results == 0)
        {
            // using var transScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await InsertAsync(dbConn, "Michael", "Angelo", "Ancient Greek", "7979");
            await InsertAsync(dbConn, "Donatello", "Lovefeller", "Ancient World", "7979");
            await InsertAsync(dbConn, "Raphael", "Cristo", "Long Greek Street", "7979");
            await InsertAsync(dbConn, "Leonardo", "Cappucino", "Big House", "7979");
            // transScope.Complete();
            _log.LogDebug("Created SeedData");
        }
    }

    private Task<bool> CreateUsers(IDbConnection dbConn)
    {
        var task = dbConn.ExecuteAsync(@"
	            CREATE TABLE Users (
		            UserId INTEGER PRIMARY KEY,
		            FirstName NVARCHAR(100),
		            LastName NVARCHAR(100)
	            )");

        return TryCreateTable("Users", task);
    }

    private Task<bool> CreateAddresses(IDbConnection dbConn)
    {
        var task = dbConn.ExecuteAsync(@"
	            CREATE TABLE Addresses (
		            AddressId INTEGER PRIMARY KEY,
		            UserId INTEGER REFERENCES Users(UserId) ON DELETE SET DEFAULT,
		            Street NVARCHAR(100),
		            PostalCode NVARCHAR(100),
		            Country NVARCHAR(100),
                    FOREIGN KEY (UserId) REFERENCES Users(UserId)
            )"
        );

        return TryCreateTable("Addresses", task);
    }

    private async Task<bool> TryCreateTable(string table, Task task)
    {
        try
        {
            await task;
            _log.LogInformation($"Created table: {table}");
            return true;
        }
        catch (SqliteException ex) when (ex.Message.Contains("already exists"))
        {
            _log.LogDebug($"Table {table} already exists");
            return false;
        }
    }

    private static async Task InsertAsync(IDbConnection dbConn, string firstName, string lastName, string street, string postalCode)
    {
        var query =
            "INSERT INTO Users (FirstName, LastName) VALUES (@firstName, @lastName);" +
            "SELECT last_insert_rowid()";
        var user = new { firstName, lastName };
        var userId = (await dbConn.QueryAsync<int>(query, user)).Single();

        query =
            "INSERT INTO Addresses (UserId, Street, PostalCode, Country) " +
            "VALUES (@userId, @street, @postalCode, @country)";
        var addr = new { userId, street, postalCode, country = "Europe" };
        await dbConn.QueryAsync(query, addr);
    }
}
