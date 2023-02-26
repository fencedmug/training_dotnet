namespace AddressBookService.DataAccess;

public interface IDatabaseSetup
{
    Task InitializeAsync();
}
