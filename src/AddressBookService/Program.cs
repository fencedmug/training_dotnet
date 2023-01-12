using AddressBookService.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddEnvironmentVariables();

var conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddScoped<IAddressRepository, AddressRepository>(sp => new AddressRepository(conn));

// above this line --> make sure to register all your interfaces + classes before
var app = builder.Build(); // ------------------------------------------------------------
// below this line --> middleware registration + get services 


var databaseSetup = bool.Parse(builder.Configuration["DatabaseSetup"]);
if (databaseSetup)
{
    var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(SqlServer));
    await SqlServer.Initialize(conn, logger);
}

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
