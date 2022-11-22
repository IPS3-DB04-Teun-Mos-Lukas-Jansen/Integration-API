using Integration_API.Models.OpenWeatherMap;

namespace Integration_API.DataLayer.External
{
    public interface IOpenWeatherMapCalls
    {
        Task<string> GetCurrentLocalWeather(OpenWeatherMapCredentials credentials);
    }
}