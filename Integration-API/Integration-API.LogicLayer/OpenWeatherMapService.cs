using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integration_API.DataLayer.External;
using Integration_API.DataLayer.Internal;
using Integration_API.Models;
using Integration_API.Models.OpenWeatherMap;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Integration_API.LogicLayer
{
    public class OpenWeatherMapService : IOpenWeatherMapService
    {
        private readonly IOpenWeatherMapCalls _mapCalls;
        private readonly ICredentialsDataAcces _credentials;

        private const string _integrationName = "openWeatherMap";
        
        public OpenWeatherMapService(IOpenWeatherMapCalls mapCalls, ICredentialsDataAcces credentials)
        {
            this._mapCalls = mapCalls;
            this._credentials = credentials;
        }


        public async Task<OpenWeatherMapResponse> GetLocalWeatherForecast(string UserId)
        {
            //Get credentials with userId
            BsonValue credentialsResponse = await _credentials.GetCredentials(UserId, _integrationName);
            OpenWeatherMapCredentials creds = BsonSerializer.Deserialize<OpenWeatherMapCredentials>(credentialsResponse.AsBsonDocument);

            if (creds.Active == false)
            {
                throw new HttpRequestException("integration is disabled", new Exception(), System.Net.HttpStatusCode.Forbidden);
            }


            string rawResponse = await _mapCalls.GetCurrentLocalWeather(creds);
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(rawResponse);

            double tempInC = response["main"]["temp"] - 272.15;
            int humidity = response["main"]["humidity"];
            double windSpeed = response["wind"]["speed"];
            int windDirection = response["wind"]["deg"];
            string weatherDescripton = response["weather"][0]["description"];
            string imgName = response["weather"][0]["icon"];
            string imgUrl = $"https://openweathermap.org/img/wn/{response["weather"][0]["icon"]}@4x.png";
            string location = response["name"];

            OpenWeatherMapResponse filteredWeatherData =
                new OpenWeatherMapResponse(
                    tempInC,
                    humidity,
                    windSpeed,
                    windDirection,
                    weatherDescripton,
                    imgName,
                    imgUrl,
                    location
                    );

            return filteredWeatherData;
        }


        public async Task SetLocalWeatherForecastCredentials(string UserId, OpenWeatherMapCredentials credentials)
        {
            await _credentials.SetCredentials(UserId, _integrationName, credentials.ToBsonDocument());
        }
    }
}
