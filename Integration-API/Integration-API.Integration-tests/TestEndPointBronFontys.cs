using Integration_API.Auth;
using Integration_API.DataLayer.External;
using Integration_API.DataLayer.Internal;
using Integration_API.Models.OpenWeatherMap;
using MongoDB.Bson;
using Moq;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.IO;
using Integration_API.Models.BronFontys;

namespace Integration_API.Integration_tests
{
    public class TestEndPointBronFontys: IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private CustomWebApplicationFactory<Program> _factory;


        private readonly BsonValue _credentials_dummy =
                    new BsonDocument
                    {
                        { "Active", true }
                    };

        private void MockAuth(string id_token_dummy, string user_id_dummy)
        {
            //mock the authorisation
            var mockAuthorisation = new Mock<IAuthorisation>();
            mockAuthorisation.Setup(x => x.ValidateIdToken(id_token_dummy)).ReturnsAsync(user_id_dummy);
            _factory.MockAuthorisation(mockAuthorisation.Object);
        }

        [Fact]
        public async Task GetFeed_returnsNewArticles()
        {
            //arrange
            const string id_token_dummy = "1234";
            const string user_id_dummy = "4321";
            _factory = new CustomWebApplicationFactory<Program>();
            MockAuth(id_token_dummy, user_id_dummy);

            var mockCredentialsDataAcces = new Mock<ICredentialsDataAcces>(); //mocks the datalayer
            mockCredentialsDataAcces.Setup(x => x.GetCredentials(user_id_dummy, "bronFontys")).ReturnsAsync(_credentials_dummy);
            _factory.MockCredentialsDataAcces(mockCredentialsDataAcces.Object);

            string path = Path.GetFullPath("../../../Assets/BronFontysFeed.xml");
            string feedString = File.ReadAllText(path);
            SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(new StringReader(feedString)));

            var mockBronFontysCalls = new Mock<IBronFontysCalls>(); 
            mockBronFontysCalls.Setup(x => x.GetNewsFeed()).ReturnsAsync(feed);
            _factory.MockBronFontysCalls(mockBronFontysCalls.Object);


            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            //act
            var response = await client.GetAsync("/bronfontys/" + id_token_dummy);

            //assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            JArray responseObject = JArray.Parse(responseString);

            Assert.NotNull(responseObject);
            Assert.NotNull(responseObject[0]);
            Assert.Equal(25 , responseObject.Count);
        }

        [Fact]
        public async Task SetBronFontysCredentials_setsFontysCredentials()
        {
            //arrange
            const string id_token_dummy = "1234";
            const string user_id_dummy = "4321";
            _factory = new CustomWebApplicationFactory<Program>();
            MockAuth(id_token_dummy, user_id_dummy);


            bool hasBeenCalled = false;

            var mockCredentialsDataAcces = new Mock<ICredentialsDataAcces>();
            mockCredentialsDataAcces.Setup(x => x.SetCredentials(user_id_dummy, "bronFontys", It.IsAny<BsonDocument>())).Callback(() => { hasBeenCalled = true; });
            _factory.MockCredentialsDataAcces(mockCredentialsDataAcces.Object);

            _factory.MockBronFontysCalls(new Mock<IBronFontysCalls>().Object);

            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            //act
            StringContent requestContent = new StringContent("{ \"active\": true}", Encoding.UTF8, "application/json");

            BronFontysCredentials creds = new BronFontysCredentials(true);
            JsonContent content = JsonContent.Create(creds);
            var response = await client.PostAsync("/bronfontys/" + id_token_dummy, content);

            //assert
            Assert.True(hasBeenCalled);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("credentials set!", responseString);

        }
    }
}
