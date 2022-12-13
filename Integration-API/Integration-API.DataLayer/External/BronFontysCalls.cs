using Integration_API.DataLayer.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Integration_API.DataLayer.External
{
    public class BronFontysCalls : IBronFontysCalls
    {
        private const string _bronFontysUrl = "https://bron.fontys.nl/feed/nl";

        public async Task<SyndicationFeed> GetNewsFeed()
        {
            using (var reader = XmlReader.Create(_bronFontysUrl))
            {
                SyndicationFeed response = SyndicationFeed.Load(reader);
                return response;
            }

        }
    }
}
