using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_API.Models.OpenWeatherMap
{
    public class OpenWeatherMapResponse
    {
        public double tempInCelcius;
        public int humidity;
        public double windSpeed;
        public int windDirection;
        public string weatherDescription;
        public string imgName;
        public string imgUrl;

        public OpenWeatherMapResponse(double tempInCelcius, int humidity, double windSpeed, int windDirection, string weatherDescription, string imgName, string imgUrl)
        {
            this.tempInCelcius = tempInCelcius;
            this.humidity = humidity;
            this.windSpeed = windSpeed;
            this.windDirection = windDirection;
            this.weatherDescription = weatherDescription;
            this.imgName = imgName;
            this.imgUrl = imgUrl;
        }
    }
}
