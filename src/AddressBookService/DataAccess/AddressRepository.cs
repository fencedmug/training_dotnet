namespace AddressBookService.DataAccess;
using AddressBookService.AddressBook;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;

public class AddressRepository : IAddressRepository
{
    private readonly IDbConnectionProvider _dbConnProv;

    private readonly string _getUsersQuery =
        "SELECT * FROM Users " +
        "LEFT JOIN Addresses " +
        "ON Users.UserId = Addresses.UserId";

    private readonly string _getAddrsQuery =
        "SELECT * FROM Addresses";

    private readonly string _getAddrByUserIdQuery =
        "SELECT * FROM Addresses where UserId = @UserId";

    private readonly string _addUserQuery = 
        "INSERT INTO Users (FirstName, LastName) VALUES (@FirstName, @LastName); ";

    private readonly string _addAddrQuery = 
        "INSERT INTO Addresses (UserId, Street, PostalCode, Country) " +
        "VALUES (@UserId, @Street, @PostalCode, @Country);";

    public AddressRepository(IDbConnectionProvider provider)
    {
        _dbConnProv = provider;

        _addUserQuery = _addUserQuery + _dbConnProv.DatabaseType switch
        {
            // different ways to get primary key back after insert
            IDbConnectionProvider.SqlServer => "SELECT CAST(SCOPE_IDENTITY() as INT)",
            IDbConnectionProvider.Sqlite => "SELECT last_insert_rowid()",
            _ => throw new NotSupportedException("Database type not supported")
        };
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        using var dbConn = _dbConnProv.GetDbConnection();
        var results = await dbConn
            .QueryAsync<User, Address, User>(_getUsersQuery, 
                (user, addr) => 
                {
                    user.Addresses = user.Addresses == null ? new List<Address>() : user.Addresses;
                    user.Addresses.Add(addr);
                    return user;
                }, splitOn: "AddressId");
        
        return results;
    }

    public async Task<IEnumerable<Address>> GetAddressesAsync()
    {
        using var dbConn = _dbConnProv.GetDbConnection();
        return await dbConn.QueryAsync<Address>(_getAddrsQuery);
    }

    public async Task<IEnumerable<Address>> GetAddressAsync(int userId)
    {
        using var dbConn = _dbConnProv.GetDbConnection();
        return await dbConn.QueryAsync<Address>(_getAddrByUserIdQuery, new { UserId = userId });
    }

    public async Task<int> AddUserAsync(User user)
    {
        // todo: have to find out if this works with sqlite
        using var transScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        using var dbConn = _dbConnProv.GetDbConnection();
        var queryResult = await dbConn.QueryAsync<int>(_addUserQuery, user);
        var userId = queryResult.Single();

        if (user.Addresses?.Count > 0)
        {
            foreach (var addr in user.Addresses)
            {
                addr.UserId = userId;
                await dbConn.QueryAsync(_addAddrQuery, addr);
            }
        }

        transScope.Complete();
        return userId;
    }

    public async Task AddAddressAsync(Address addr)
    {
        using var dbConn = _dbConnProv.GetDbConnection();
        await dbConn.QueryAsync(_addAddrQuery, addr);
    }
}
