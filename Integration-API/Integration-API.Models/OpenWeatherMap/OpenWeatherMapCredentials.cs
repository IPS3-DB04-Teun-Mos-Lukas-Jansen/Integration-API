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
        [BsonElement("isActive")]
        public bool isActive { get; set; }

        [BsonElement("cityName")]
        public string cityName { get; set; }

        public OpenWeatherMapCredentials(bool isActive, string cityName)
        {
            this.isActive = isActive;
            this.cityName = cityName;
        }
    }
}
