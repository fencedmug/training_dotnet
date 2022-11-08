namespace AddressBookService.AddressBook;

public class User
{
    public int UserId { get; set; } = 0;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<Address> Addresses { get; set; } = new List<Address>();
}