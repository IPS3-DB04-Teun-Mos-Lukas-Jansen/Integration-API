using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Integration_API.Models.OpenWeatherMap
{
    public class OpenWeatherMapCredentials
    {
        [BsonElement("Active")]
        public bool Active { get; set; }
        
        [BsonElement("City")]
        public string City { get; set; }

        public OpenWeatherMapCredentials(bool Active, string City)
        {
            this.Active = Active;
            this.City = City;
        }
    }
}
