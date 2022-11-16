using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_API.Models.OpenWeatherMap
{
    public class OpenWeatherMapCredentials
    {
        public bool isActive;
        public string cityName;

        public OpenWeatherMapCredentials(bool isActive, string cityName)
        {
            this.isActive = isActive;
            this.cityName = cityName;
        }
    }
}
