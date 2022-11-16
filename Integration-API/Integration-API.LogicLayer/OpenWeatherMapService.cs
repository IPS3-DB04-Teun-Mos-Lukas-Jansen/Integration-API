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

            var response = 


        }
    }
}
