using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integration_API.DataLayer.External;
using Integration_API.Models;
using Integration_API.Models.OpenWeatherMap;

namespace Integration_API.LogicLayer
{
    public class OpenWeatherMapService
    {
        private readonly OpenWeatherMapCalls mapCalls;

        public OpenWeatherMapService(OpenWeatherMapCalls mapCalls)
        {
            this.mapCalls = mapCalls;
        }


        public async Task<OpenWeatherMapResponse> GetLocalWeatherForecast(string UserId)
        {
            //Get credentials with userId
            OpenWeatherMapCredentials creds = new OpenWeatherMapCredentials(true, "Eindhoven"); //temporary

            string rawResponse = await mapCalls.GetCurrentLocalWeather(creds);
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(rawResponse);

            double tempInC = response["main"]["temp"] - 272.15;
            int humidity = response["main"]["humidity"];
            double windSpeed = response["wind"]["speed"];
            int windDirection = response["wind"]["deg"];
            string weatherDescripton = response["weather"][0]["description"];
            string imgName = response["weather"][0]["icon"];
            string imgUrl = $"https://openweathermap.org/img/wn/{response["weather"][0]["icon"]}@4x.png";

            OpenWeatherMapResponse filteredWeatherData = 
                new OpenWeatherMapResponse(
                    tempInC,
                    humidity,
                    windSpeed,
                    windDirection,
                    weatherDescripton,
                    imgName,
                    imgUrl
                    );

            return filteredWeatherData;
        }
    }
}
