using Integration_API.Models.OpenWeatherMap;
using System.Text.RegularExpressions;

namespace Integration_API.DataLayer.External
{
    public class OpenWeatherMapCalls : IOpenWeatherMapCalls
    {
        private readonly string ApiKey;
        private readonly string BaseUrl = "https://api.openweathermap.org/data/2.5/";
        public OpenWeatherMapCalls(string apiKey)
        {
            ApiKey = apiKey;
        }


        public async Task<string> GetCurrentLocalWeather(OpenWeatherMapCredentials credentials)
        {
            string city = credentials.City;
            Regex rgx = new Regex("[?&=]");
            city = rgx.Replace(city, "");

            string urlParams = $"weather?q={city}&appid={ApiKey}";
            string url = BaseUrl + urlParams;

            using (HttpClient client = new HttpClient())
            {
                    var response = await client.GetStringAsync(url);



                    return response;
                
            }

        }

    }
}
