using Integration_API.DataLayer.External;
using Integration_API.DataLayer.Internal;
using Integration_API.LogicLayer;
using Integration_API.LogicLayer.SmallServices;
using Integration_API.Models.BronFontys;
using MongoDB.Bson;
using Moq;
using Pose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xunit;

namespace Integration_API.UnitTests
{
    public class BronFontysTests
    {
        private readonly string _integrationName = "bronFontys";
        private readonly BsonValue _credentials_dummy =
                    new BsonDocument
                    {
                        { "Active", true } 
                    };

        [Fact]
        public async Task GetFeed_CachesFeed_AndUsesCache_IfCacheIsYounger_ThenTenMinutes()
        {

            //arrange
            string dummyUserID = "123456";

            Mock<ICredentialsDataAcces> mockCredentials = new Mock<ICredentialsDataAcces>();
            mockCredentials.Setup(x => x.GetCredentials(dummyUserID, _integrationName)).ReturnsAsync(_credentials_dummy);
            
            string path = Path.GetFullPath("../../../Assets/BronFontysFeed.xml");
            string feedString = File.ReadAllText(path);
            SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(new StringReader(feedString)));

            int callCount = 0;
            Mock<IBronFontysCalls> mockBronFontysCalls = new Mock<IBronFontysCalls>();
            mockBronFontysCalls.Setup(x => x.GetNewsFeed()).Callback(() => { callCount++; }).ReturnsAsync(feed);

            BronFontysService bronFontysService = new BronFontysService(mockBronFontysCalls.Object,mockCredentials.Object);

            //act
            var result1 = await bronFontysService.GetFeed(dummyUserID);
            var result2 = await bronFontysService.GetFeed(dummyUserID);

            //assert
            Assert.Equal(1, callCount);
            Assert.Equal(result1, result2);
        }


        [Fact]
        public async Task GetFeed_CachesFeed_AndDoesntUseCache_IfCacheIsOlder_ThenTenMinutes()
        {

            //arrange
            string dummyUserID = "123456";

            Mock<ICredentialsDataAcces> mockCredentials = new Mock<ICredentialsDataAcces>();
            mockCredentials.Setup(x => x.GetCredentials(dummyUserID, _integrationName)).ReturnsAsync(_credentials_dummy);

            string path = Path.GetFullPath("../../../Assets/BronFontysFeed.xml");
            string feedString = File.ReadAllText(path);
            SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(new StringReader(feedString)));

            int callCount = 0;
            Mock<IBronFontysCalls> mockBronFontysCalls = new Mock<IBronFontysCalls>();
            mockBronFontysCalls.Setup(x => x.GetNewsFeed()).Callback(() => { callCount++; }).ReturnsAsync(feed);

            BronFontysService bronFontysService = new BronFontysService(mockBronFontysCalls.Object, mockCredentials.Object);
            

            //act
            var result1 = await bronFontysService.GetFeed(dummyUserID);
            TimeTravelService.SetDateTime(DateTime.Now.AddMinutes(11));
            var result2 = await bronFontysService.GetFeed(dummyUserID);
            TimeTravelService.ResetDateTime();


            //assert
            Assert.Equal(2, callCount);
        }
    }
}
