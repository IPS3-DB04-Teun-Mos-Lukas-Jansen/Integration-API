using Integration_API.Auth;
using Integration_API.DataLayer.Internal;
using MongoDB.Bson;
using Moq;
using Newtonsoft.Json.Linq;

namespace Integration_API.Integration_tests
{
    public class TestEndpointActiveIntegrations : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private CustomWebApplicationFactory<Program> _factory;
        private readonly BsonValue _credentials_dummy = new BsonDocument
            {
                { "integration1",
                    new BsonDocument
                    {
                        { "Active", true } ,
                        { "Credential1", "ike woone ine eindje-hoven" }
                    }
                },
                { "integration2",
                    new BsonDocument
                    {
                        { "Active", false } ,
                        { "Credential1", "ike woone ine beste" }
                    }
                }
            };

        private void MockAuth(string id_token_dummy, string user_id_dummy)
        {
            //mock the authorisation
            var mockAuthorisation = new Mock<IAuthorisation>();
            mockAuthorisation.Setup(x => x.ValidateIdToken(id_token_dummy)).ReturnsAsync(user_id_dummy);
            _factory.MockAuthorisation(mockAuthorisation.Object);
        }

        private void MockAuth()
        {
            string id_token_dummy = "1234";
            string user_id_dummy = "4321";
            //mock the Data returned by the datalayer
            MockAuth(id_token_dummy, user_id_dummy);
        }

        //test endpoint to get all integrations
        [Fact]
        public async Task GetAllIntegrations_returnsSuccess()
        {
            //arrange
            _factory = new CustomWebApplicationFactory<Program>();

            var mockCredentialsDataAcces = new Mock<ICredentialsDataAcces>();
            _factory.MockCredentialsDataAcces(mockCredentialsDataAcces.Object);
            MockAuth();

            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            //act
            var response = await client.GetAsync("/all");

            //assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetUserCredentials_returnsSuccess()
        {

            _factory = new CustomWebApplicationFactory<Program>();
            //arrange
            string id_token_dummy = "1234";
            string user_id_dummy = "4321";
            MockAuth(id_token_dummy, user_id_dummy);

            var mockCredentialsDataAcces = new Mock<ICredentialsDataAcces>();
            mockCredentialsDataAcces.Setup(x => x.GetAllCredentials(user_id_dummy)).ReturnsAsync(_credentials_dummy);

            _factory.MockCredentialsDataAcces(mockCredentialsDataAcces.Object);

            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

                   


            //act
            var response = await client.GetAsync($"/credentials/{id_token_dummy}");

            //assert

            //  ensure status code 200 OK
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //  ensure response body is correct
            var result = await response.Content.ReadAsStringAsync();
            JArray jArray = JArray.Parse(result);
            Assert.Equal("integration1", jArray[0]["name"].ToString());
            Assert.Equal("integration2", jArray[1]["name"].ToString());
            Assert.True(jArray[0]["credentials"]["Active"].ToObject<bool>());
            Assert.False(jArray[1]["credentials"]["Active"].ToObject<bool>());
        }



    }
}