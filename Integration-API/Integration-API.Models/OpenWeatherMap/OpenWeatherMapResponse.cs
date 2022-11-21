using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_API.Models.OpenWeatherMap
{
    public class OpenWeatherMapResponse
    {
        public double tempInCelcius { get; set; }
        public int humidity { get; set; }
        public double windSpeed { get; set; }
        public int windDirection { get; set; }
        public string weatherDescription { get; set; }
        public string imgName { get; set; }
        public string imgUrl { get; set; }

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
