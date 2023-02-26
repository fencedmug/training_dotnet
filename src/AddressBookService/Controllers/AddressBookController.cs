using Microsoft.AspNetCore.Mvc;

namespace AddressBookService.Controllers;
using AddressBookService.AddressBook;
using AddressBookService.DataAccess;

[ApiController]
[Route("[controller]")]
public class AddressBookController : ControllerBase
{
    private readonly ILogger<AddressBookController> _logger;
    private readonly IAddressRepository _repository;

    public AddressBookController(ILogger<AddressBookController> logger, IAddressRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet("GetUsers")]
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var users = await _repository.GetUsersAsync();
        return users;
    }

    [HttpGet("GetAddresses")]
    public async Task<IEnumerable<Address>> GetAddressesAsync()
    {
        var addresses = await _repository.GetAddressesAsync();
        return addresses;
    }

    [HttpGet("GetUserAddresses")]
    public async Task<IEnumerable<Address>> GetUserAddressesAsync(int userId)
    {
        var addresses = await _repository.GetAddressAsync(userId);
        return addresses;
    }

    [HttpPost("AddUser")]
    public async Task<IActionResult> AddUserAsync(User user)
    {
        await _repository.AddUserAsync(user);
        return Ok();
    }
}
