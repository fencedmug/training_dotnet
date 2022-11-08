namespace AddressBookService.AddressBook;

public class Address
{
    public int UserId { get; set; } = 0;
    public int AddressId { get; set; } = 0;
    public string Street { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}