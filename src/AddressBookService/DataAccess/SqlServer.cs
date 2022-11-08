using System.Data;
using AddressBookService.AddressBook;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AddressBookService.DataAccess;

public static class SqlServer
{
    private const string CreateDatabaseSql = "CreateDatabase.sql";
    private const string CreateTablesSql = "CreateTables.sql";
    private static readonly TimeSpan InitialWaitTime = TimeSpan.FromSeconds(10);

    public static async Task Initialize(string connectionString, ILogger log)
    {
        log.LogDebug($"Wait of {InitialWaitTime.TotalSeconds}s before running scripts");
        await Task.Delay(InitialWaitTime);

        await CreateDatabaseAsync(connectionString);
        log.LogDebug($"Executed {CreateDatabaseSql}");
        
        var createTbSql = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "SqlScripts", CreateTablesSql));
        var dbConn = new SqlConnection(connectionString);
        dbConn.Execute(createTbSql);   
        log.LogDebug($"Executed {CreateTablesSql}");

        var query = "SELECT COUNT(UserId) from Users";
        var results = dbConn.Query<int>(query).Single();
        if (results == 0)
        {
            await InsertAsync(dbConn, "Michael", "Angelo", "Ancient Greek", "7979");
            await InsertAsync(dbConn, "Donatello", "Lovefeller", "Ancient World", "7979");
            await InsertAsync(dbConn, "Raphael", "Cristo", "Long Greek Street", "7979");
            await InsertAsync(dbConn, "Leonardo", "Cappucino", "Big House", "7979");
            log.LogDebug("Created SeedData");
        }
    }

    private static async Task CreateDatabaseAsync(string connStr)
    {
        // assuming the initial database doesn't exist yet
        // change initial catalog to something else
        var connStrBuilder = new SqlConnectionStringBuilder(connStr);
        connStrBuilder.InitialCatalog = "master";

        var dbConn = new SqlConnection(connStrBuilder.ConnectionString);
        var createDbSql = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "SqlScripts", CreateDatabaseSql));

        await dbConn.ExecuteAsync(createDbSql);
    }

    private static async Task InsertAsync(IDbConnection dbConn, string firstName, string lastName, string street, string postalCode)
    {
        var query = 
            "INSERT Users (FirstName, LastName) VALUES (@firstName, @lastName);" +
            "SELECT CAST(SCOPE_IDENTITY() as INT)";
        var user = new { firstName, lastName };
        var userId = (await dbConn.QueryAsync<int>(query, user)).Single();
        
        query =  
            "INSERT Addresses (UserId, Street, PostalCode, Country) " +
            "VALUES (@userId, @street, @postalCode, @country)";
        var addr = new { userId, street, postalCode, country = "Europe" };
        await dbConn.QueryAsync(query, addr);
    }
}