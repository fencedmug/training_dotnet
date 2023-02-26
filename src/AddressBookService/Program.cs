using AddressBookService.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddEnvironmentVariables();

var dbflag = builder.Configuration["DatabaseType"];
var conn = builder.Configuration.GetConnectionString(dbflag);

if (dbflag == IDbConnectionProvider.SqlServer)
{
    builder.Services.AddScoped<IDbConnectionProvider, SqlServerDbConnectionProvider>(sp => new SqlServerDbConnectionProvider(conn));
    builder.Services.AddScoped<IDatabaseSetup, SqlServerSetup>();
}
else if (dbflag == IDbConnectionProvider.Sqlite)
{
    builder.Services.AddScoped<IDbConnectionProvider, SqliteDbConnectionProvider>(sp => new SqliteDbConnectionProvider(conn));
    builder.Services.AddScoped<IDatabaseSetup, SqliteSetup>();
}
else
{
    throw new InvalidOperationException("Need valid database type");
}


builder.Services.AddScoped<IAddressRepository, AddressRepository>();

// above this line --> make sure to register all your interfaces + classes before
var app = builder.Build(); // ------------------------------------------------------------
// below this line --> middleware registration + get services 

await app.SetupDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "Address Book Service");
app.Run();
