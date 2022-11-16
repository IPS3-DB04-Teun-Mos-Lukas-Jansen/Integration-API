using Integration_API.DataLayer.External;
using Integration_API.Models.OpenWeatherMap;

OpenWeatherMapCalls calls = new OpenWeatherMapCalls("AAA");
OpenWeatherMapCredentials creds = new OpenWeatherMapCredentials(true, "Eindhoven");
await calls.GetCurrentLocalWeather(creds);
creds = new OpenWeatherMapCredentials(true, "Best");
await calls.GetCurrentLocalWeather(creds);