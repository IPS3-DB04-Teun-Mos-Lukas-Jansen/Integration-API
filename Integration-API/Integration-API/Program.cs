

using Integration_API.LogicLayer;
using Integration_API.DataLayer.Internal;
using Integration_API.DataLayer.External;
using MongoDB.Driver;
using Integration_API.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string mongoDbConnString = Environment.GetEnvironmentVariable("INTEGRATION_DB_URL");
string openWeatherMapApiKey = Environment.GetEnvironmentVariable("OPENWEATHERMAP_APIKEY");

if (mongoDbConnString == null)
{
    mongoDbConnString = "mongodb://localhost:27016";
}

if (openWeatherMapApiKey == null)
{
    openWeatherMapApiKey = "bazinga";
}



OpenWeatherMapCalls openWeatherMapCalls = new OpenWeatherMapCalls(openWeatherMapApiKey);
BronFontysCalls bronFontysCalls = new BronFontysCalls();
CredentialsDataAcces credentialsDataAcces = new CredentialsDataAcces(new MongoClient(mongoDbConnString));

builder.Services.AddSingleton<IOpenWeatherMapService>(new OpenWeatherMapService(openWeatherMapCalls, credentialsDataAcces));
builder.Services.AddSingleton<IIntegrationsHelper>(new IntegrationsHelper(credentialsDataAcces));
builder.Services.AddSingleton<IAuthorisation>(new Authorisation());
builder.Services.AddSingleton<IBronFontysService>(new BronFontysService(bronFontysCalls,credentialsDataAcces));

var AllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000");
                          builder.AllowAnyHeader();
                          builder.AllowAnyMethod();
                      });
});

var app = builder.Build();
app.UseCors(AllowSpecificOrigins);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }

