namespace AddressBookService.DataAccess;

public static class DataAccessExtensions
{
    public static async Task SetupDatabaseAsync(this WebApplication app)
    {
        // we need a scope here because the classes are added using AddScope
        using var scope = app.Services.CreateScope();
        IDatabaseSetup dbSetup = scope.ServiceProvider.GetRequiredService<IDatabaseSetup>();
        await dbSetup.InitializeAsync();
    }
}
