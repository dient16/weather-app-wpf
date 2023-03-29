using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather_App
{
    public class WeatherNext7Day
    {
        private double weatherTempMAX;
        private double weatherTempMIN;
        private string? InfdWeather;
        private string? weathericon;
        private string? weatherTime;

        public double WeatherTempMAX
        {
            get => weatherTempMAX; 
            set { weatherTempMAX = value; }
        }
        public double WeatherTempMIN
        {
            get => weatherTempMIN;
            set { weatherTempMIN = value; }
        }
        public string? Weathericon
        {
            get => weathericon;
            set { weathericon = value; }
        }
        public string? INfoWeather
        {
            get => InfdWeather;
            set { InfdWeather = value; }
        }
        public string? WeatherTime
        {
            get { return weatherTime; }
            set { weatherTime = value; }
        }
    }
}
