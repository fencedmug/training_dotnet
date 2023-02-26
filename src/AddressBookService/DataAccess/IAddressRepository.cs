namespace AddressBookService.DataAccess;
using AddressBookService.AddressBook;

public interface IAddressRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<IEnumerable<Address>> GetAddressesAsync();
    Task<IEnumerable<Address>> GetAddressAsync(int userId);

    Task<int> AddUserAsync(User user);
    Task AddAddressAsync(Address addr);
}
