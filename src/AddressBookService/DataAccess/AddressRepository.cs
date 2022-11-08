namespace AddressBookService.DataAccess;
using AddressBookService.AddressBook;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;

public class AddressRepository : IAddressRepository
{
    private readonly IDbConnection _dbConn;

    public AddressRepository(string connectionString)
    {
        _dbConn = new SqlConnection(connectionString);
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var query = 
            "SELECT * FROM Users " +
            "LEFT JOIN Addresses " +
            "ON Users.UserId = Addresses.UserId";

        var results = await _dbConn
            .QueryAsync<User, Address, User>(query, 
                (user, addr) => 
                {
                    user.Addresses = user.Addresses == null ? new List<Address>() : user.Addresses;
                    user.Addresses.Add(addr);
                    return user;
                }, splitOn: "AddressId");
        
        return results;
    }

    public async Task<IEnumerable<Address>> GetAddressAsync(int userId)
    {
        return await _dbConn.QueryAsync<Address>("SELECT * FROM Addresses where UserId = @UserId", new { UserId = userId });
    }

    public async Task<int> AddUserAsync(User user)
    {
        using var transScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var query = 
            "INSERT Users (FirstName, LastName) VALUES (@FirstName, @LastName); " + 
            "SELECT CAST(SCOPE_IDENTITY() as INT)";
        
        var queryResult = await _dbConn.QueryAsync<int>(query, user);
        var userId = queryResult.Single();

        if (user.Addresses?.Count > 0)
        {
            foreach (var addr in user.Addresses)
            {
                addr.UserId = userId;
                await AddAddressAsync(addr);
            }
        }

        transScope.Complete();
        return userId;
    }

    public async Task AddAddressAsync(Address addr)
    {
        var query =
            "INSERT Addresses (UserId, Street, PostalCode, Country) " +
            "VALUES (@UserId, @Street, @PostalCode, @Country);";

        await _dbConn.QueryAsync(query, addr);
    }
}
