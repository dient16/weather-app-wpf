using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather_App
{
    public class WeatherNext24h
    {
        private string? weatherTemp;
        private string? weathericon;
        private string? weatherTime;

        public string? WeatherTemp
        {
            get { return weatherTemp; }
            set { weatherTemp = value; }
        }
        public string? Weathericon
        {
            get { return weathericon; }
            set { weathericon = value; }
        }
        public string? WeatherTime
        {
            get { return weatherTime; }
            set { weatherTime = value; }
        }
    }
}
