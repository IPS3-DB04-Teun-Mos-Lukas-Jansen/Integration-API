using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Integration_API.Models.OpenWeatherMap;

namespace Integration_API.DataLayer.External
{
    public class OpenWeatherMapCalls
    {
        private readonly string ApiKey;
        private readonly string BaseUrl = "https://api.openweathermap.org/data/2.5/";
        public OpenWeatherMapCalls(string apiKey)
        {
            ApiKey = apiKey;
        }
        

        public async Task<string> GetCurrentLocalWeather(OpenWeatherMapCredentials credentials)
        {
            string city = credentials.cityName;
            Regex rgx = new Regex("[?&=]");
            city = rgx.Replace(city, "");

            string urlParams = $"weather?q={city}&appid={ApiKey}";
            string url = BaseUrl + urlParams;

            using (HttpClient client = new HttpClient())
            {
                var response =  await client.GetStringAsync(url);
                return response;
            }

        }

    }
}
