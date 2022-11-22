using Integration_API.Models.OpenWeatherMap;

namespace Integration_API.LogicLayer
{
    public interface IOpenWeatherMapService
    {
        Task<OpenWeatherMapResponse> GetLocalWeatherForecast(string UserId);
        Task SetLocalWeatherForecastCredentials(string UserId, OpenWeatherMapCredentials credentials);
    }
}