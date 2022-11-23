

using Integration_API.LogicLayer;
using Integration_API.DataLayer.Internal;
using Integration_API.DataLayer.External;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string mongoDbConnString = Environment.GetEnvironmentVariable("INTEGRATION_DB_URL");
string openWeatherMapApiKey = Environment.GetEnvironmentVariable("OPENWEATHERMAP_APIKEY");

OpenWeatherMapCalls openWeatherMapCalls = new OpenWeatherMapCalls(openWeatherMapApiKey);
CredentialsDataAcces credentialsDataAcces = new CredentialsDataAcces(mongoDbConnString);

builder.Services.AddSingleton<IOpenWeatherMapService>(new OpenWeatherMapService(openWeatherMapCalls, credentialsDataAcces));

var AllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000");
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
