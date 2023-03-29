using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather_App
{
    public class WeatherCurrent
    {
        private double lon;
        private double lat;
        private int idcontry;
        private string? contryName;
        private string? status;
        private string? weatherDesc;
        private string? weathericon;
        private string? weatherID;
        private double temperature;
        private double temperatureMIN;
        private double temperatureMAX;
        private int visibility;
        private int sunset;
        private int sunrise;
        private double wind_speed;
        private string? wind_direction;
        private string? wind_deg;
        private int uv_index;
        private int humidity;
        private int dewpt;
        private int possibilityofrain;
        private int aQI;
        private string? clouds;
        private string? monrise;
        private double pressure;
        private double rain;
        private int aqiIndex;
        private double so2;
        private double no2;
        private double pm2_5;
        private double pm10;
        private double o3;
        public int AqiIndex
        {
            get { return aqiIndex; }
            set { aqiIndex = value; }
        }
        public double So2
        {
            get { return so2; }
            set { so2 = value; }
        }
        public double O3
        {
            get { return o3; }
            set { o3 = value; }
        }
        public double No2
        {
            get { return no2; }
            set { no2 = value; }
        }
        public double Pm2_5
        {
            get { return pm2_5; }
            set { pm2_5 = value; }
        }
        public double Pm10
        {
            get { return pm10; }
            set { pm10 = value; }
        }

        public int Possibilityofrain
        {
            get { return possibilityofrain; }
            set { possibilityofrain = value; }
        }
        public int Dewpt
        {
            get { return dewpt; }
            set { dewpt = value; }
        }
        public double Lat
        { 
            get { return lat; } 
            set { lat = value; }
        }
        public double Lon
        { 
            get => lon;  
            set { lon = value; } 
        }
        public int IdContry
        { 
            get { return idcontry; } 
            set { idcontry = value; } 
        }
        public int AQI
        {
            get { return aQI; }
            set { aQI = value; }
        }
        public string? Wind_direction
        {
            get { return wind_direction; }
            set { wind_direction = value; }
        }
        public string? WeatherID
        { 
            get { return weatherID; } 
            set { weatherID = value; } 
        }
        public string? Status 
        { 
            get { return status; } 
            set { status = value; } 
        }
        public string? Monrise
        {
            get { return monrise; }
            set { monrise = value; }
        }
        public int Visibility
        {
            get { return visibility; }
            set { visibility = value; }
        }
        public string? WeatherDesc   
        { 
            get { return weatherDesc; } 
            set { weatherDesc = value; } 
        }
        public string? ContryName     
        { 
            get { return contryName; } 
            set { contryName = value; } 
        }
        public string? Statuss
        { 
            get { return status; } 
            set { status = value; } 
        }
        public string? Weathericon            
        { 
            get { return weathericon; } 
            set { weathericon = value; } 
        }
        public double Temperature            
        { 
            get { return temperature; } 
            set { temperature = value; } 
        }
        public double TemperatureMIN
        {
            get { return temperatureMIN; }
            set { temperatureMIN = value; }
        }
        public double TemperatureMAX
        {
            get { return temperatureMAX; }
            set { temperatureMAX = value; }
        }
        public int Sunset           
        { 
            get { return sunset; } 
            set { sunset = value; } 
        }
        public int Sunrise           
        { 
            get { return sunrise; } 
            set { sunrise = value; } 
        }
        public double Wind_speed
        { 
            get { return wind_speed; } 
            set { wind_speed = value; } 
        }
        public string? Wind_deg           
        { 
            get { return wind_deg; } 
            set { wind_deg = value; } 
        }
        public int UV_index          
        { 
            get { return uv_index; } 
            set { uv_index = value; } 
        }
        public int Humidity          
        { 
            get { return humidity; } 
            set { humidity = value; } 
        }
        public string? Clouds          
        { 
            get { return clouds; } 
            set { clouds = value; } 
        }
        public double Pressure
        { 
            get { return pressure; } 
            set { pressure = value; } 
        }
        public double Rain
        { 
            get { return rain; } 
            set { rain = value; } 
        }
    }
}
