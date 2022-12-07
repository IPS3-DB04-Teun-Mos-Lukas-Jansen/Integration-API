using Integration_API.Auth;
using Integration_API.DataLayer.External;
using Integration_API.DataLayer.Internal;
using Integration_API.Models.OpenWeatherMap;
using MongoDB.Bson;
using Moq;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Text;

namespace Integration_API.Integration_tests
{
    public class TestEndpointOpenWeaterMap : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly string _openWeatherRawResponseDummy = "{\"coord\":{\"lon\":4.8897,\"lat\":52.374},\"weather\":[{\"id\":801,\"main\":\"Clouds\",\"description\":\"few clouds\",\"icon\":\"02d\"}],\"base\":\"stations\",\"main\":{\"temp\":279.9,\"feels_like\":277.38,\"temp_min\":278.08,\"temp_max\":281.4,\"pressure\":1013,\"humidity\":84},\"visibility\":10000,\"wind\":{\"speed\":3.6,\"deg\":290},\"clouds\":{\"all\":20},\"dt\":1670415301,\"sys\":{\"type\":2,\"id\":2012552,\"country\":\"NL\",\"sunrise\":1670398547,\"sunset\":1670426892},\"timezone\":3600,\"id\":2759794,\"name\":\"Amsterdam\",\"cod\":200}";

        private readonly BsonValue _credentials_dummy =
                    new BsonDocument
                    {
                        { "Active", true } ,
                        { "City", "Amsterdam" }
                    };

        private CustomWebApplicationFactory<Program> _factory;

        private void MockAuth(string id_token_dummy, string user_id_dummy)
        {
            //mock the authorisation
            var mockAuthorisation = new Mock<IAuthorisation>();
            mockAuthorisation.Setup(x => x.ValidateIdToken(id_token_dummy)).ReturnsAsync(user_id_dummy);
            _factory.MockAuthorisation(mockAuthorisation.Object);
        }


        [Fact]
        public async Task GetLocalWeather_returns_weather()
        {
            //arrange
            const string id_token_dummy = "1234";
            const string user_id_dummy = "4321";
            _factory = new CustomWebApplicationFactory<Program>();
            MockAuth(id_token_dummy, user_id_dummy);

            var mockCredentialsDataAcces = new Mock<ICredentialsDataAcces>();
            mockCredentialsDataAcces.Setup(x => x.GetCredentials(user_id_dummy, "openWeatherMap")).ReturnsAsync(_credentials_dummy);
            _factory.MockCredentialsDataAcces(mockCredentialsDataAcces.Object);

            Mock<IOpenWeatherMapCalls> mockedOpenWeatherMapCalls = new Mock<IOpenWeatherMapCalls>();
            mockedOpenWeatherMapCalls.Setup(x => x.GetCurrentLocalWeather(It.IsAny<OpenWeatherMapCredentials>())).ReturnsAsync(_openWeatherRawResponseDummy);
            _factory.MockOpenWeatherMapCalls(mockedOpenWeatherMapCalls.Object);

            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            //act
            var response = await client.GetAsync("/openweathermap/" + id_token_dummy);

            //assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            JObject responseObject = JObject.Parse(responseString);
            Assert.Equal("Amsterdam", responseObject["location"].ToString());
        }

        [Fact]
        public async Task SetLocalWeather_sets_local_weather()
        {
            //arrange
            const string id_token_dummy = "1234";
            const string user_id_dummy = "4321";
            _factory = new CustomWebApplicationFactory<Program>();
            MockAuth(id_token_dummy, user_id_dummy);

            bool hasBeenCalled = false;
            var mockCredentialsDataAcces = new Mock<ICredentialsDataAcces>();
            mockCredentialsDataAcces.Setup(x => x.SetCredentials(user_id_dummy, "openWeatherMap", It.IsAny<BsonDocument>())).Callback(() => { hasBeenCalled = true; });
            _factory.MockCredentialsDataAcces(mockCredentialsDataAcces.Object);

            Mock<IOpenWeatherMapCalls> mockedOpenWeatherMapCalls = new Mock<IOpenWeatherMapCalls>();
            _factory.MockOpenWeatherMapCalls(mockedOpenWeatherMapCalls.Object);

            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            //act
            StringContent requestContent = new StringContent("{ \"active\": true, \"city\": \"Amsterdam\"}", Encoding.UTF8, "application/json");

            OpenWeatherMapCredentials creds = new OpenWeatherMapCredentials(true, "Amsterdam");
            JsonContent content = JsonContent.Create(creds);
            var response = await client.PostAsync("/openweathermap/" + id_token_dummy, content );

            //assert
            Assert.True(hasBeenCalled);
            response.EnsureSuccessStatusCode();
            
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("credentials set!", responseString);
        }

    }
}
