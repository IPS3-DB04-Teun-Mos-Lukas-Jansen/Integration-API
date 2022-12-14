using Integration_API.DataLayer.External;
using Integration_API.DataLayer.Internal;
using Integration_API.Models.BronFontys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Xml.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Integration_API.LogicLayer.SmallServices;

namespace Integration_API.LogicLayer
{
    public class BronFontysService : IBronFontysService
    {
        private readonly IBronFontysCalls _BronFontysCalls;
        private readonly ICredentialsDataAcces _credentials;
        private readonly string _integrationName = "bronFontys";

        private List<BronFontysResponse> cachedFeedItems;
        private DateTime lastCacheTime;

        public BronFontysService(IBronFontysCalls BronFontysCalls, ICredentialsDataAcces credentials)
        {
            _BronFontysCalls = BronFontysCalls;
            _credentials = credentials;
        }
        
        //Get list of bronfontyscalls models
        public async Task<List<BronFontysResponse>> GetFeed(string UserId)
        {
            //Get credentials with userId
            BsonValue credentialsResponse = await _credentials.GetCredentials(UserId, _integrationName);
            if (credentialsResponse == null)
            {
                throw new HttpRequestException("no credentials specified", new Exception(), System.Net.HttpStatusCode.Unauthorized);
            }
            BronFontysCredentials creds = BsonSerializer.Deserialize<BronFontysCredentials>(credentialsResponse.AsBsonDocument);

            if (creds.Active == false)
            {
                throw new HttpRequestException("integration is disabled", new Exception(), System.Net.HttpStatusCode.Forbidden);
            }
            
            if (TimeTravelService.Now() - lastCacheTime > TimeSpan.FromMinutes(10)) //Checks if time difference since last time is more then 10 min.
            {
                //get feed list
                SyndicationFeed RSSfeed = await _BronFontysCalls.GetNewsFeed();
                List<BronFontysResponse> feedItems = new List<BronFontysResponse>();

                if (RSSfeed != null)
                {
                    foreach (var item in RSSfeed.Items)
                    {
                        string boilerplate = null;
                        try
                        {
                            boilerplate = item.ElementExtensions.ReadElementExtensions<XElement>("boilerplate", "http://www.presspage.com/rss/")[0].Value;
                        }
                        catch (ArgumentOutOfRangeException)
                        {

                        }


                        var img = item.ElementExtensions.ReadElementExtensions<XElement>("image", "http://www.presspage.com/rss/");



                        feedItems.Add(
                            new BronFontysResponse
                            {
                                title = item.Title.Text,
                                link = item.Links[0].Uri.AbsoluteUri,
                                description = item.Summary.Text,
                                pubDate = item.PublishDate.DateTime,
                                imgUrl = img[0].Value,
                                shortDescription = boilerplate,
                            }
                           );
                    }
                }

                lastCacheTime = TimeTravelService.Now();
                cachedFeedItems = feedItems;
                return feedItems;

            }
            else
            {
                return cachedFeedItems;
            }
        }

        public async Task SetBronFontysCredentials(string UserId, BronFontysCredentials credentials)
        {
            await _credentials.SetCredentials(UserId, _integrationName, credentials.ToBsonDocument());
        }
    }
}
