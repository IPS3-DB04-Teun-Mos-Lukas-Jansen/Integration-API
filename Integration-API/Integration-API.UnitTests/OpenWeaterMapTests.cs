using Integration_API.Models.OpenWeatherMap;
using Xunit;
using Moq;
using Integration_API.DataLayer.External;
using Integration_API.DataLayer.Internal;
using MongoDB.Bson;

namespace Integration_API.LogicLayer.UnitTests
{
    public class OpenWeaterMapServiceTests
    {
        private readonly string _integrationName =  "openWeatherMap";

        private readonly string _openWeatherRawResponseDummy = "{\"coord\":{\"lon\":4.8897,\"lat\":52.374},\"weather\":[{\"id\":801,\"main\":\"Clouds\",\"description\":\"few clouds\",\"icon\":\"02d\"}],\"base\":\"stations\",\"main\":{\"temp\":279.9,\"feels_like\":277.38,\"temp_min\":278.08,\"temp_max\":281.4,\"pressure\":1013,\"humidity\":84},\"visibility\":10000,\"wind\":{\"speed\":3.6,\"deg\":290},\"clouds\":{\"all\":20},\"dt\":1670415301,\"sys\":{\"type\":2,\"id\":2012552,\"country\":\"NL\",\"sunrise\":1670398547,\"sunset\":1670426892},\"timezone\":3600,\"id\":2759794,\"name\":\"Amsterdam\",\"cod\":200}";
        private readonly BsonValue _credentials_dummy =
                    new BsonDocument
                    {
                        { "Active", true } ,
                        { "City", "Amsterdam" }
                    };
        [Fact]
        public async void GetLocalWeatherForecast_returns_OpenWeatherMapResponse()    
        {
            //arrange
            string dummyUserID = "123456";

            Mock<IOpenWeatherMapCalls> mockMapCalls = new Mock<IOpenWeatherMapCalls>();
            Mock<ICredentialsDataAcces> mockCredentials = new Mock<ICredentialsDataAcces>();

            mockCredentials.Setup(x => x.GetCredentials(dummyUserID, _integrationName)).ReturnsAsync(_credentials_dummy);
            mockMapCalls.Setup(x => x.GetCurrentLocalWeather(It.IsAny<OpenWeatherMapCredentials>())).ReturnsAsync(_openWeatherRawResponseDummy);

            OpenWeatherMapService service = new OpenWeatherMapService(mockMapCalls.Object, mockCredentials.Object);

            //act
            OpenWeatherMapResponse result = await service.GetLocalWeatherForecast(dummyUserID);

            //assert
            Assert.IsType<OpenWeatherMapResponse>(result);
            Assert.NotNull(result);
            Assert.Equal("Amsterdam", result.location);
        }
    }
}