using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Weather_App.UserController;
namespace Weather_App
{
    public class HandleDataApi : Window
    {
        private static HandleDataApi? instance;
        public static HandleDataApi Instant
        {
            get
            {
                if (instance == null)
                    instance = new HandleDataApi();
                return HandleDataApi.instance;
            }
            private set { HandleDataApi.instance = value; }
        }
        private HandleDataApi() { }
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        public int TruDateTime(DateTime time1, DateTime time2)
        {
            TimeSpan date = time1 - time2;
            long i = Math.Abs(date.Ticks);
            DateTime newDate = new DateTime(i);
            int NumMinutes = (newDate.Hour * 60) + newDate.Minute +  (newDate.Second >30?1:0);
            return NumMinutes;
        }
        public List<string>?  CallApiWeatherCurr(string location)
        {
            Thread.Sleep(20);
            string? TypeSeach = null;
            // load setting 
            JObject @object = JObject.Parse(File.ReadAllText(@"DATA\setting.json"));
            Thread.Sleep(20);
            if (Convert.ToInt32(@object["cb_Seach_LatLon"]) == 1)
                TypeSeach = "Seach_LatLon";
            Thread.Sleep(20);
            if (Convert.ToInt32(@object["cb_Seach_city"]) == 1)
                TypeSeach = "Seach_city";
            Thread.Sleep(20);
            string Key_openweathermap = @object["Key_openweathermap"].ToString();
            string weatherapi = @object["Key_weatherapi"].ToString();

            string weatherbit = @object["Key_weatherbit"].ToString();
            //////

            if (location == null || location == ""|| location == string.Empty)
            {
                Thread.Sleep(20);
                new ShowDialogCustom("Địa chỉ không được để trống\n" +
                    "Vui lòng nhập lại", "Thông Báo", ShowDialogCustom.OK).ShowDialog();
                return null;
            }    
            List<string> listjson = new List<string>();
            string? json = null;
            JObject jobject;
            Thread.Sleep(20);
            using (HttpClient httpClient = new HttpClient())
            {
                Thread.Sleep(20);
                string APIopenweathermap = "";
                if (TypeSeach == "Seach_city")
                {
                    Thread.Sleep(20);
                    if (location.Contains(','))
                    {
                        new ShowDialogCustom("Địa chỉ sai định dạng\nVui lòng chỉnh lại tìm kiếm theo kinh độ và vĩ độ", "Thông Báo", ShowDialogCustom.OK).ShowDialog();
                        return null ;
                    }    
                    APIopenweathermap = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&lang=vi&mode=json&units=metric", location, Key_openweathermap);
                }    
                    

                if (TypeSeach == "Seach_LatLon")
                {
                    if (!location.Contains(','))
                    {
                        new ShowDialogCustom("Địa chỉ sai định dạng\nVui lòng chỉnh lại tìm kiếm theo thành phố", "Thông Báo", ShowDialogCustom.OK).ShowDialog();
                        return null;
                    }
                    APIopenweathermap = string.Format("https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}&lang=vi&mode=json&units=metric", location.Trim().Replace(" ", "").Split(',')[0], location.Trim().Replace(" ", "").Split(',')[1], Key_openweathermap);
                }     
                var endPoint = new Uri(APIopenweathermap);
                var result = httpClient.GetAsync(endPoint).Result;
                Thread.Sleep(200);
                json = result.Content.ReadAsStringAsync().Result;
                listjson.Add(json);
                jobject = JObject.Parse(json);
                if(jobject["cod"].ToString() == "404" || jobject["cod"].ToString() == "400")
                {
                    new ShowDialogCustom("Vui lòng thử lại", "Thông Báo", ShowDialogCustom.OK).ShowDialog();
                    return null;
                }    
            }
            using (HttpClient httpClient = new HttpClient())
            {
                string APIweatherapi = string.Format("http://api.weatherapi.com/v1/forecast.json?key={2}&q={0},{1}&days=8&aqi=yes&lang=vi", jobject["coord"]["lat"].ToString(), jobject["coord"]["lon"].ToString(), weatherapi);
                var endPoint = new Uri(APIweatherapi);
                var result = httpClient.GetAsync(endPoint).Result;
                Thread.Sleep(20);
                json = result.Content.ReadAsStringAsync().Result;
                listjson.Add(json);
            }
            using (HttpClient httpClient = new HttpClient())
            {
                string APIweatherbit = string.Format("https://api.weatherbit.io/v2.0/current?lat={0}&lon={1}&key={2}", jobject["coord"]["lat"].ToString(), jobject["coord"]["lon"].ToString(), weatherbit);
                var endPoint = new Uri(APIweatherbit);
                var result = httpClient.GetAsync(endPoint).Result;
                Thread.Sleep(200);
                json = result.Content.ReadAsStringAsync().Result;
                listjson.Add(json);
            }

            //using (HttpClient httpClient = new HttpClient())
            //{
            //    string APIweatherbit = string.Format("https://api.weatherbit.io/v2.0/forecast/hourly?lat={0}&lon={1}&key=cbf74e83594d400999a2808b8d2f0538&hours=24", jobject["coord"]["lat"].ToString(), jobject["coord"]["lon"].ToString());
            //    var endPoint = new Uri(APIweatherbit);
            //    var result = httpClient.GetAsync(endPoint).Result;
            //    json = result.Content.ReadAsStringAsync().Result;
            //    listjson.Add(json);
            //}

            return listjson;
        }
        public List<WeatherNext24h> GETweatherNext24h(string json)
        {
            List<WeatherNext24h> list = new List<WeatherNext24h>();
            JObject keyValuePairs = JObject.Parse(json);
            int nowhours = DateTime.Now.Hour;
            foreach (var keyValuePair in keyValuePairs["forecast"]["forecastday"][0]["hour"])
            {
                if(Convert.ToInt32(keyValuePair["time"].ToString().Split(" ")[1].Split(':')[0]) > nowhours)
                {
                    WeatherNext24h weatherNext24H = new WeatherNext24h();
                    weatherNext24H.WeatherTemp = keyValuePair["temp_c"].ToString();
                    weatherNext24H.Weathericon = keyValuePair["condition"]["icon"].ToString();
                    weatherNext24H.WeatherTime = keyValuePair["time"].ToString();
                    list.Add(weatherNext24H);
                }             
            }
            foreach (var keyValuePair in keyValuePairs["forecast"]["forecastday"][1]["hour"])
            {
                if (Convert.ToInt32(keyValuePair["time"].ToString().Split(" ")[1].Split(':')[0]) <= nowhours)
                {
                    WeatherNext24h weatherNext24H = new WeatherNext24h();
                    weatherNext24H.WeatherTemp = keyValuePair["temp_c"].ToString();
                    weatherNext24H.Weathericon = keyValuePair["condition"]["icon"].ToString();
                    weatherNext24H.WeatherTime = keyValuePair["time"].ToString();
                    list.Add(weatherNext24H);
                }
            }
            return list;
        }
        public List<WeatherNext24h> GET24hToday(string json)
        {       
            List<WeatherNext24h> list = new List<WeatherNext24h>();
            JObject @object = JObject.Parse(json);
            foreach (var keyValue in @object["forecast"]["forecastday"][0]["hour"])
            {
                WeatherNext24h weatherNext24H = new WeatherNext24h();
                weatherNext24H.WeatherTemp = keyValue["temp_c"].ToString();
                weatherNext24H.Weathericon = keyValue["condition"]["icon"].ToString();
                weatherNext24H.WeatherTime = keyValue["time"].ToString();
                list.Add(weatherNext24H);
            }      
            return list;
        }
        public List<WeatherNext7Day> GET7Day(string json)
        {
            List<WeatherNext7Day> list = new List<WeatherNext7Day>();
            JObject keyValuePairs = JObject.Parse(json);
            for (int i = 1; i < 8; i++)
            {
                WeatherNext7Day weatherNext7day = new WeatherNext7Day();
                weatherNext7day.WeatherTempMAX = Convert.ToDouble(keyValuePairs["forecast"]["forecastday"][i]["day"]["maxtemp_c"]);
                weatherNext7day.WeatherTempMIN = Convert.ToDouble(keyValuePairs["forecast"]["forecastday"][i]["day"]["mintemp_c"]);
                weatherNext7day.Weathericon = keyValuePairs["forecast"]["forecastday"][i]["day"]["condition"]["icon"].ToString();
                weatherNext7day.INfoWeather = keyValuePairs["forecast"]["forecastday"][i]["day"]["condition"]["text"].ToString();
                weatherNext7day.WeatherTime = keyValuePairs["forecast"]["forecastday"][i]["date"].ToString();
                list.Add(weatherNext7day);
            }
            return list;
        }
        public WeatherCurrent GETweatherCurrent(List<string> jsons)
        {
            WeatherCurrent weather = new WeatherCurrent();
            JObject jobject = JObject.Parse(jsons[0]);
            JObject jobject2 = JObject.Parse(jsons[2]);
            JObject keyValuePairs = JObject.Parse(jsons[1]);
            weather.WeatherDesc = jobject["weather"][0]["description"].ToString();
            weather.Lat = Convert.ToDouble(jobject["coord"]["lat"]);
            weather.Temperature = Convert.ToDouble(jobject["main"]["temp"]);
           
            weather.Lon = Convert.ToDouble(jobject["coord"]["lon"]);
            weather.ContryName = jobject["name"].ToString();
            weather.IdContry = Convert.ToInt32(jobject["id"]);
            weather.Visibility = Convert.ToInt32(jobject["visibility"]);
            weather.Humidity = Convert.ToInt32(jobject["main"]["humidity"]);
            weather.Pressure = Convert.ToDouble(jobject["main"]["pressure"]);
            weather.Sunset = Convert.ToInt32(jobject["sys"]["sunset"]);
            weather.Sunrise = Convert.ToInt32(jobject["sys"]["sunrise"]);
            weather.Wind_speed = Convert.ToDouble(jobject["wind"]["speed"]);


            //weather.UV_index = Convert.ToInt32(jobject2["data"][0]["uv"]);
            weather.AqiIndex = Convert.ToInt32(keyValuePairs["current"]["air_quality"]["us-epa-index"]);
            weather.O3 = Math.Round(Convert.ToDouble(keyValuePairs["current"]["air_quality"]["o3"]),2);
            weather.No2 = Math.Round(Convert.ToDouble(keyValuePairs["current"]["air_quality"]["no2"]),2);
            weather.Pm2_5 = Math.Round(Convert.ToDouble(keyValuePairs["current"]["air_quality"]["pm2_5"]),2);
            weather.Pm10 = Math.Round(Convert.ToDouble(keyValuePairs["current"]["air_quality"]["pm10"]),2);

            weather.TemperatureMIN = Convert.ToDouble(keyValuePairs["forecast"]["forecastday"][0]["day"]["mintemp_c"]);
            weather.TemperatureMAX = Convert.ToDouble(keyValuePairs["forecast"]["forecastday"][0]["day"]["maxtemp_c"]);
            weather.Possibilityofrain = Convert.ToInt32(keyValuePairs["forecast"]["forecastday"][0]["day"]["daily_chance_of_rain"]);
            weather.Monrise = keyValuePairs["forecast"]["forecastday"][0]["astro"]["moonrise"].ToString();
            weather.Wind_direction = keyValuePairs["current"]["wind_dir"].ToString();
            weather.Rain = Convert.ToDouble(keyValuePairs["current"]["precip_mm"]);
            weather.Clouds = keyValuePairs["current"]["cloud"].ToString();
            weather.WeatherID = keyValuePairs["current"]["condition"]["code"].ToString();
            weather.Weathericon = keyValuePairs["current"]["condition"]["icon"].ToString();
            //weather.AQI = Convert.ToInt32(jobject2["data"][0]["aqi"]);
            //weather.Dewpt = Convert.ToInt32(jobject2["data"][0]["dewpt"]);
            return weather;
        }
    }
}
