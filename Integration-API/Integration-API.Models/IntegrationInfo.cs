using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_API.Models
{
    public class IntegrationInfo
    {
        public IntegrationInfo(string className, string name, string imgUrl, List<Property> properties, List<string> tags, int cardCount , string description)
        {
            this.className = className;
            this.name = name;
            this.imgUrl = imgUrl;
            this.properties = properties;
            this.tags = tags;
            this.description = description;
            this.cardCount = cardCount;
        }

        public IntegrationInfo()
        {

        }

        public string className { get; set; }
        public string name { get; set; }
        public string imgUrl { get; set; }
        public string description { get; set; } 
        public List<Property> properties { get; set; }

        public List<string> tags { get; set; }
        public int cardCount { get; set; }
    }

    public class Property
    {
        public string name { get; set; }
        public string type { get; set; }
        
        public Property(string name, string type)
        {
            this.name = name;
            this.type = type;
        }
    }
}
