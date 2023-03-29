using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Weather_App.UserController;
using Forms = System.Windows.Forms;

namespace Weather_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow? _instance;
        private bool IsStop = false;
        private string pathImg = Directory.GetCurrentDirectory().Split("\\bin\\Debug")[0];
        string location = null;
        
        public MainWindow()
        {
            InitializeComponent();
            _instance = this;
            Init();
        }
        private void Init()
        {
            new Thread(() =>
            {
                while (true)
                {
                    Card_SunriseSunset.Dispatcher.Invoke(new Action(() =>
                    {
                        if (IsStop) return;
                        if (Card_SunriseSunset.Deg == 360)
                            Card_SunriseSunset.Deg = 0;
                        Card_SunriseSunset.Deg += 40;
                    }));
                    Thread.Sleep(50);
                }
            }).Start();

        }

        private void mainwindow_Loaded(object sender, EventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(UpdateTimerCur);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
            //DoSomething();

        }
        public async Task DoSomething()
        {
          
            JObject @object = JObject.Parse(File.ReadAllText(@"DATA\setting.json"));
            List<string>? list = HandleDataApi.Instant.CallApiWeatherCurr(@object["Location_default"].ToString());
            location = @object["Location_default"].ToString();
            if (list == null)
            {
                return;
            }
            else
            {
                WeatherCurrent weatherCurrent = HandleDataApi.Instant.GETweatherCurrent(list);
                List<WeatherNext24h> lsweatherNext24H = HandleDataApi.Instant.GETweatherNext24h(list[1]);
                List<WeatherNext24h> ls24htoday = HandleDataApi.Instant.GET24hToday(list[1]);
                List<WeatherNext7Day> ls7day = HandleDataApi.Instant.GET7Day(list[1]);
                UpdateDataIntoView(weatherCurrent, lsweatherNext24H, ls24htoday, ls7day);
            }
        }
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);
        private System.Drawing.Icon bitmapToIcon(System.Drawing.Bitmap myBitmap)
        {
            IntPtr Hicon = myBitmap.GetHicon();
            System.Drawing.Icon newIcon = System.Drawing.Icon.FromHandle(Hicon);

            return newIcon;
        }
        private void UpdateTimerCur(object sender, EventArgs e)
        {
            TimerCurr.Text = DateTime.Now.ToString();
           
        }
        private void mainwindow_Closed(object sender, EventArgs e)
        {
            IsStop = true;
            Application.Current.Shutdown();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
        }
        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text) && txtSearch.Text.Length > 0)
                textSearch.Visibility = Visibility.Collapsed;
            else
                textSearch.Visibility = Visibility.Visible;
        }

        private void SeachButon_Click(object sender, RoutedEventArgs e)
        {

            List<string>? list = HandleDataApi.Instant.CallApiWeatherCurr(txtSearch.Text);
            if (list == null)
            {
                return;
            }
            else
            {
                location = txtSearch.Text;
                WeatherCurrent weatherCurrent = HandleDataApi.Instant.GETweatherCurrent(list);
                List<WeatherNext24h> lsweatherNext24H = HandleDataApi.Instant.GETweatherNext24h(list[1]);
                List<WeatherNext24h> ls24htoday = HandleDataApi.Instant.GET24hToday(list[1]);
                List<WeatherNext7Day> ls7day = HandleDataApi.Instant.GET7Day(list[1]);
                UpdateDataIntoView(weatherCurrent, lsweatherNext24H, ls24htoday, ls7day);
            }
        }
        public static Control? GetChildrenControl(int index)
        {
            Control? control = null;
            _instance.WrapPanel_cardhours.Dispatcher.Invoke(() =>
            {
                foreach (Control item in _instance.WrapPanel_cardhours.Children)
                {
                    if (item.Name == "Card_hours" + index)
                    {
                        control = item;
                        break;
                    }
                }
                return control;
            });
            return control;
        }
        DispatcherTimer Notification = null;
        private void UpdateDataIntoView(WeatherCurrent weatherCurrent, List<WeatherNext24h> lsweatherNext24H, List<WeatherNext24h> ls24htoday, List<WeatherNext7Day> days)
        {
            JObject @object = JObject.Parse(File.ReadAllText(@"DATA\setting.json"));
            if (Notification != null)
                Notification.Stop();
            Notification = new DispatcherTimer();
            Notification.Tick += new EventHandler((object sender, EventArgs e) =>
            {
                List<string>? list = HandleDataApi.Instant.CallApiWeatherCurr(location);
                if (list == null)
                {
                    return;
                }
                else
                {
                    WeatherCurrent weatherCurrent = HandleDataApi.Instant.GETweatherCurrent(list);
                    List<WeatherNext24h> lsweatherNext24H = HandleDataApi.Instant.GETweatherNext24h(list[1]);
                    List<WeatherNext24h> ls24htoday = HandleDataApi.Instant.GET24hToday(list[1]);
                    List<WeatherNext7Day> ls7day = HandleDataApi.Instant.GET7Day(list[1]);
                    UpdateDataIntoView(weatherCurrent, lsweatherNext24H, ls24htoday, ls7day);
                }
                Forms.NotifyIcon notify = new Forms.NotifyIcon();
                notify.Text = "Weather App Nhóm 14";
                notify.Visible = true;
                notify.BalloonTipTitle = weatherCurrent.ContryName + " hiện tại: " + weatherCurrent.Temperature + "°C";
                notify.BalloonTipText = weatherCurrent.WeatherDesc;
                notify.Icon = bitmapToIcon(new System.Drawing.Bitmap(pathImg + "/ImgSource/" + weatherCurrent.Weathericon.Split("64x64")[1]));
                notify.ShowBalloonTip(50000);

            });
            Notification.Interval = new TimeSpan(0, Int32.Parse(@object["Minute_Update"].ToString()), 0);
            Notification.Start();
            Lb_Location.Content = weatherCurrent.ContryName;
            Lb_StatusWeatherCurrent.Content = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(weatherCurrent.WeatherDesc.ToLower());
            Lb_UVindex.Text = weatherCurrent.UV_index.ToString();
            Thread thread3 = new Thread(() =>
            {
                int uv = 0;
                while (uv <= weatherCurrent.UV_index)
                {
                    Slider_uvindex.Dispatcher.Invoke(new Action(() =>
                    {
                        Slider_uvindex.Value = uv;
                    }));
                    uv++;
                    Thread.Sleep(1000);
                }

            });
            thread3.Start();
            thread3.IsBackground = true;
            Img_IconCurr.Source = new BitmapImage(new Uri(pathImg + "/ImgSource/" + weatherCurrent.Weathericon.Split("64x64")[1]));
            Lb_TempCurr.Content = Math.Round(weatherCurrent.Temperature, 0).ToString();
            Lb_tempmin.Content = Math.Round(weatherCurrent.TemperatureMIN, 0).ToString() + "°C";
            Lb_tempmax.Content = Math.Round(weatherCurrent.TemperatureMAX, 0).ToString() + "°C";
            Monrise.Content = weatherCurrent.Monrise;
            if (weatherCurrent?.UV_index < 3)
            {
                Lb_UVlevel.Text = "Thấp";
            }
            else if (weatherCurrent?.UV_index >= 3 && weatherCurrent.UV_index < 6)
            {
                Lb_UVlevel.Text = "Trung bình";
            }
            else if (weatherCurrent?.UV_index >= 6 && weatherCurrent.UV_index < 8)
            {
                Lb_UVlevel.Text = "Cao";
            }
            else if (weatherCurrent?.UV_index >= 8 && weatherCurrent.UV_index < 11)
            {
                Lb_UVlevel.Text = "Rất cao";
            }
            else
            {
                Lb_UVlevel.Text = "Cực cao";
            }
            Lb_DoAm.Text = weatherCurrent?.Humidity.ToString();
            Slider_DoAm.Value = weatherCurrent.Humidity;
            if (weatherCurrent?.Humidity < 10)
            {
                Lb_UVlevel.Text = "Cực kì khô";
            }
            else if (weatherCurrent?.Humidity >= 10 && weatherCurrent.Humidity < 20)
            {
                Lb_LevelDoAm.Text = "Khô hạn";
            }
            else if (weatherCurrent?.Humidity >= 20 && weatherCurrent.Humidity < 30)
            {
                Lb_LevelDoAm.Text = "Cực kì thấp";
            }
            else if (weatherCurrent.Humidity >= 30 && weatherCurrent.Humidity < 40)
            {
                Lb_LevelDoAm.Text = "Rất thấp";
            }
            else if (weatherCurrent.Humidity >= 40 && weatherCurrent.Humidity < 50)
            {
                Lb_LevelDoAm.Text = "Thấp";
            }
            else if (weatherCurrent.Humidity >= 50 && weatherCurrent.Humidity < 60)
            {
                Lb_LevelDoAm.Text = "Trung bình";
            }
            else if (weatherCurrent.Humidity >= 60 && weatherCurrent.Humidity < 70)
            {
                Lb_LevelDoAm.Text = "Lí Tưởng";
            }
            else if (weatherCurrent.Humidity >= 70 && weatherCurrent.Humidity < 80)
            {
                Lb_LevelDoAm.Text = "Vừa";
            }
            else if (weatherCurrent.Humidity >= 80 && weatherCurrent.Humidity < 90)
            {
                Lb_LevelDoAm.Text = "Tương đối cao";
            }
            else if (weatherCurrent.Humidity >= 90 && weatherCurrent.Humidity < 100)
            {
                Lb_LevelDoAm.Text = "Cao";
            }
            else
            {
                Lb_LevelDoAm.Text = "Bão hòa";
            }
            Lb_dewpt.Text = weatherCurrent.Dewpt.ToString() + "°";
            Lb_rain.Text = weatherCurrent.Rain.ToString();
            sld_Rain.Value = weatherCurrent.Rain;
            Lb_daily_chance_of_rain.Text = weatherCurrent.Possibilityofrain.ToString();
            Lb_TamNhin.Text = (weatherCurrent.Visibility / 1000).ToString();
            Slider_TamNhin.Value = (weatherCurrent.Visibility / 1000);
            Lb_gio.Text = Math.Round(weatherCurrent.Wind_speed, 2).ToString();

            Lb_huongGio.Text = Cons.Instant.signalist[weatherCurrent.Wind_direction];
            Thread thread1 = new Thread(() =>
            {
                double tam = 0;
                while (tam <= weatherCurrent.Pressure)
                {

                    AngularGauge_apSuat.Dispatcher.Invoke(new Action(() =>
                    {
                        AngularGauge_apSuat.Value = tam;
                        Lb_Apsuat.Text = tam.ToString();
                    }));
                    tam += 10.0;
                    Thread.Sleep(50);
                }
                AngularGauge_apSuat.Dispatcher.Invoke(new Action(() =>
                {
                    AngularGauge_apSuat.Value = weatherCurrent.Pressure;
                    Lb_Apsuat.Text = weatherCurrent.Pressure.ToString();
                }));
            });
            thread1.Start();
            thread1.IsBackground = true;
            DateTime timeSunrise = HandleDataApi.Instant.UnixTimeStampToDateTime(weatherCurrent.Sunrise);
            DateTime timeSunset = HandleDataApi.Instant.UnixTimeStampToDateTime(weatherCurrent.Sunset);
            string sunrise = (Convert.ToInt32(timeSunrise.Hour) < 10 ? "0" : "") + timeSunrise.Hour
                + ':' + (Convert.ToInt32(timeSunrise.Minute) < 10 ? "0" : "") + timeSunset.Minute;
            string sunset = (Convert.ToInt32(timeSunset.Hour) < 10 ? "0" : "") + timeSunset.Hour
                + ':' + (Convert.ToInt32(timeSunset.Minute) < 10 ? "0" : "") + timeSunset.Minute;
            Lb_sunrise.Text = "MT mọc: " + sunrise;
            Lb_sunset.Text = "MT lặn: " + sunset;

            int numMinues = HandleDataApi.Instant.TruDateTime(HandleDataApi.Instant.UnixTimeStampToDateTime(weatherCurrent.Sunrise), HandleDataApi.Instant.UnixTimeStampToDateTime(weatherCurrent.Sunset));
            int numMinuesCurr = HandleDataApi.Instant.TruDateTime(HandleDataApi.Instant.UnixTimeStampToDateTime(weatherCurrent.Sunrise), DateTime.Now);
            Card_SunriseSunset.Dispatcher.Invoke(new Action(() =>
            {
                Card_SunriseSunset.Amount = 0;
                Card_SunriseSunset.Deg = 0;
            }));
            Thread thread = new Thread(() =>
            {
                if (DateTime.Now <= HandleDataApi.Instant.UnixTimeStampToDateTime(weatherCurrent.Sunrise) && Convert.ToInt32(DateTime.Now.Hour) > 0)
                {
                    Card_SunriseSunset.Dispatcher.Invoke(new Action(() =>
                    {
                        Card_SunriseSunset.Amount = 0;
                    }));

                }
                else if (DateTime.Now > HandleDataApi.Instant.UnixTimeStampToDateTime(weatherCurrent.Sunrise) && DateTime.Now < HandleDataApi.Instant.UnixTimeStampToDateTime(weatherCurrent.Sunset))
                {
                    double percent = (Convert.ToDouble(numMinuesCurr) / Convert.ToDouble(numMinues)) * 100;
                    double valueTime = (180 * percent) / 100;
                    int tam = 0;
                    while (tam <= Convert.ToInt32(Math.Round(valueTime, 0)))
                    {

                        Card_SunriseSunset.Dispatcher.Invoke(new Action(() =>
                        {
                            Card_SunriseSunset.Amount = tam;
                            //if (Card_SunriseSunset.Deg == 360)
                            //    Card_SunriseSunset.Deg = 0;
                            //Card_SunriseSunset.Deg += 30;
                        }));
                        tam += 1;
                        Thread.Sleep(50);
                    }
                }
                else
                {
                    int tam = 0;
                    while (tam <= 180)
                    {

                        Card_SunriseSunset.Dispatcher.Invoke(new Action(() =>
                        {
                            Card_SunriseSunset.Amount = tam;
                            //if (Card_SunriseSunset.Deg == 360)
                            //    Card_SunriseSunset.Deg = 0;
                            //Card_SunriseSunset.Deg +=30;
                        }));
                        tam += 1;
                        Thread.Sleep(50);
                    }
                }
            });

            thread.Start();
            thread.IsBackground = true;
            //load data weather next 24h
            Thread thread2 = new Thread(() =>
            {
                for (int i = 0; i < 24; i++)
                {
                    if (GetChildrenControl(i) != null)
                    {
                        Control cardhours = GetChildrenControl(i);
                        int hours = Convert.ToInt32(lsweatherNext24H[i].WeatherTime.Split(" ")[1].Split(':')[0]);
                        string hoursfomat = (hours >= 10 ? "" : "0") + hours + ":00";
                        (cardhours as UserController.CardHours).Dispatcher.Invoke(new Action(() =>
                        {
                            (cardhours as UserController.CardHours).Hours = hoursfomat;
                            (cardhours as UserController.CardHours).Temp = Math.Round(Convert.ToDouble(lsweatherNext24H[i].WeatherTemp), 0).ToString() + "°C";
                            (cardhours as UserController.CardHours).Source = new BitmapImage(new Uri(pathImg + "/ImgSource/" + lsweatherNext24H[i].Weathericon.Split("64x64")[1]));
                        }));
                    }
                    Thread.Sleep(500);
                }
            });
            thread2.Start();
            thread2.IsBackground = true;

            ChartValues<double> vs = new ChartValues<double>();
            //list.Sort((x, y) => Convert.ToDouble(x.WeatherTime).CompareTo(Convert.ToDouble(y.WeatherTime)));
            foreach (var item in ls24htoday)
            {
                vs.Add(Convert.ToDouble(item.WeatherTemp));
            }
            LiveChart_temp.Values = vs;
            LiveChart_temp.Title = "Nhiệt độ";
            Img_9h.Source = new BitmapImage(new Uri(pathImg + "/ImgSource/" + ls24htoday[9].Weathericon.Split("64x64")[1]));
            Txt_9h.Text = Math.Round(Convert.ToDouble(ls24htoday[9].WeatherTemp), 0).ToString() + "°C";
            Img_15h.Source = new BitmapImage(new Uri(pathImg + "/ImgSource/" + ls24htoday[15].Weathericon.Split("64x64")[1]));
            Txt_15h.Text = Math.Round(Convert.ToDouble(ls24htoday[15].WeatherTemp), 0).ToString() + "°C";
            Img_21h.Source = new BitmapImage(new Uri(pathImg + "/ImgSource/" + ls24htoday[21].Weathericon.Split("64x64")[1]));
            Txt_21h.Text = Math.Round(Convert.ToDouble(ls24htoday[21].WeatherTemp), 0).ToString() + "°C";
            Img_3h.Source = new BitmapImage(new Uri(pathImg + "/ImgSource/" + ls24htoday[3].Weathericon.Split("64x64")[1]));
            Txt_3h.Text = Math.Round(Convert.ToDouble(ls24htoday[3].WeatherTemp), 0).ToString() + "°C";
            LiveChart_temp.PointGeometry = DefaultGeometries.Square;
            Lb_o3.Content = weatherCurrent.O3.ToString();
            Lb_no2.Content = weatherCurrent.No2.ToString();
            Lb_pm10.Content = weatherCurrent.Pm10.ToString();
            Lb_pm2_5.Content = weatherCurrent.Pm2_5.ToString();
            Lb_aqi.Text = weatherCurrent.AQI.ToString();
            if (weatherCurrent.AqiIndex == 1)
            {
                lB_aqiindex.Text = "Tốt";
            }
            else if (weatherCurrent.AqiIndex == 2)
            {
                lB_aqiindex.Text = "Vừa phải";
            }
            else if (weatherCurrent.AqiIndex == 3)
            {
                lB_aqiindex.Text = "Không tốt cho\nnhóm nhạy cảm";
            }
            else if (weatherCurrent.AqiIndex == 4)
            {
                lB_aqiindex.Text = "Không Tốt";
            }
            else if (weatherCurrent.AqiIndex == 5)
            {
                lB_aqiindex.Text = "Rất không Tốt";
            }
            else
            {
                lB_aqiindex.Text = "Nguy hiểm";
            }

            var bc = new BrushConverter();
            //O3
            if (weatherCurrent.O3 < 60)
            {
                Level_o3.Background = (Brush)bc.ConvertFrom("#60f542");
            }
            else if (weatherCurrent.O3 >= 60 && weatherCurrent.O3 < 120)
            {
                Level_o3.Background = (Brush)bc.ConvertFrom("#99f542");
            }
            else if (weatherCurrent.O3 >= 120 && weatherCurrent.O3 < 180)
            {
                Level_o3.Background = (Brush)bc.ConvertFrom("#e0f542");
            }
            else if (weatherCurrent.O3 >= 180 && weatherCurrent.O3 < 240)
            {
                Level_o3.Background = (Brush)bc.ConvertFrom("#f5ef42");
            }
            else
            {
                Level_no2.Background = (Brush)bc.ConvertFrom("#d61d09");
            }
            //no2
            if (weatherCurrent.No2 < 50)
            {
                Level_no2.Background = (Brush)bc.ConvertFrom("#60f542");
            }
            else if (weatherCurrent.No2 >= 50 && weatherCurrent.No2 < 100)
            {
                Level_no2.Background = (Brush)bc.ConvertFrom("#99f542");
            }
            else if (weatherCurrent.No2 >= 100 && weatherCurrent.No2 < 200)
            {
                Level_no2.Background = (Brush)bc.ConvertFrom("#e0f542");
            }
            else if (weatherCurrent.No2 >= 200 && weatherCurrent.No2 < 400)
            {
                Level_no2.Background = (Brush)bc.ConvertFrom("#f5ef42");
            }
            else
            {
                Level_no2.Background = (Brush)bc.ConvertFrom("#d61d09");
            }
            //pm10
            if (weatherCurrent.Pm10 < 25)
            {
                Level_pm10.Background = (Brush)bc.ConvertFrom("#60f542");
            }
            else if (weatherCurrent.Pm10 >= 25 && weatherCurrent.Pm10 < 50)
            {
                Level_pm10.Background = (Brush)bc.ConvertFrom("#99f542");
            }
            else if (weatherCurrent.Pm10 >= 50 && weatherCurrent.Pm10 < 90)
            {
                Level_pm10.Background = (Brush)bc.ConvertFrom("#e0f542");
            }
            else if (weatherCurrent.Pm10 >= 90 && weatherCurrent.Pm10 < 180)
            {
                Level_pm10.Background = (Brush)bc.ConvertFrom("#f5ef42");
            }
            else
            {
                Level_pm10.Background = (Brush)bc.ConvertFrom("#d61d09");
            }
            //pm25
            if (weatherCurrent.Pm2_5 < 15)
            {
                Level_pm2_5.Background = (Brush)bc.ConvertFrom("#60f542");
            }
            else if (weatherCurrent.Pm2_5 >= 15 && weatherCurrent.Pm2_5 < 30)
            {
                Level_pm2_5.Background = (Brush)bc.ConvertFrom("#99f542");
            }
            else if (weatherCurrent.Pm2_5 >= 30 && weatherCurrent.Pm2_5 < 55)
            {
                Level_pm2_5.Background = (Brush)bc.ConvertFrom("#e0f542");
            }
            else if (weatherCurrent.Pm2_5 >= 55 && weatherCurrent.Pm2_5 < 110)
            {
                Level_pm2_5.Background = (Brush)bc.ConvertFrom("#f5ef42");
            }
            else
            {
                Level_pm2_5.Background = (Brush)bc.ConvertFrom("#d61d09");
            }
            for (int i = 0; i <= 7; i++)
            {
                if (GetChildrenControl1(i) != null)
                {
                    Control card7day = GetChildrenControl1(i);
                    string[] ar = days[i].WeatherTime.Split("-");
                    string dayofweek = (new DateTime(Int32.Parse(ar[0]), Int32.Parse(ar[1]), Int32.Parse(ar[2]))).DayOfWeek.ToString();
                    string fomartdayofweek = dayofweek.ToCharArray()[0] + dayofweek.ToCharArray()[1] + dayofweek.ToCharArray()[2] +"";
                    (card7day as UserController.CardInfo).Day = dayofweek + " " + ar[2]+"/" +ar[1];
                    (card7day as UserController.CardInfo).MinNum = Math.Round(Convert.ToDouble(days[i].WeatherTempMIN), 0).ToString() + "°C";
                    (card7day as UserController.CardInfo).MaxNum = Math.Round(Convert.ToDouble(days[i].WeatherTempMAX), 0).ToString() + "°C";
                    (card7day as UserController.CardInfo).InfoWeather = days[i].INfoWeather;
                    (card7day as UserController.CardInfo).Source = new BitmapImage(new Uri(pathImg + "/ImgSource/" + days[i].Weathericon.Split("64x64")[1]));
                }
                Thread.Sleep(500);
            }
        }
        public static Control? GetChildrenControl1(int index)
        {
            foreach (Control item in _instance.WrapCard_Day.Children)
            {
                if (item.Name == "CardDay_" + index)
                {
                    return item;
                }

            }
            return null;
        }
        //Function Chuyển Độ F Sang Độ C
        private double toCelc(double fahr)
        {
            return (fahr - 32.0) * (5.0 / 9.0);
        }
        //Function Chuyển Độ C Sang Độ F
        private double toFahr(double celc)
        {
            return celc * 9.0 / 5.0 + 32.0;
        }
        private void btn_ChangeDoC_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var bc = new BrushConverter();
                btn_ChangeFahrenheit.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                btn_ChangeFahrenheit.Foreground = (Brush)bc.ConvertFrom("#1A1A1A");
                btn_ChangeCelsius.Background = (Brush)bc.ConvertFrom("#1A1A1A");
                btn_ChangeCelsius.Foreground = (Brush)bc.ConvertFrom("#FFFFFF");
                double temp = Convert.ToDouble(Lb_TempCurr.Content);
                Lb_TempCurr.Content = Math.Round(toCelc(temp), 0).ToString();
                Text_Unit.Content = "°C";
                for (int i = 0; i < 24; i++)
                {
                    if (GetChildrenControl(i) != null)
                    {
                        Control cardhours = GetChildrenControl(i);
                        double temp24h = Convert.ToDouble((cardhours as UserController.CardHours).Temp.Replace("°F", ""));
                        (cardhours as UserController.CardHours).Temp = Math.Round(toCelc(temp24h), 0).ToString() + "°C";
                    }
                }
                double tempMIN = Convert.ToDouble(Lb_tempmin.Content.ToString().Replace("°F", ""));
                Lb_tempmin.Content = Math.Round(toCelc(tempMIN), 0) + "°C";
                double tempMAX = Convert.ToDouble(Lb_tempmax.Content.ToString().Replace("°F", ""));
                Lb_tempmax.Content = Math.Round(toCelc(tempMAX), 0) + "°C";
            }
            catch (Exception)
            {

            }
            btn_ChangeFahrenheit.IsEnabled = true;
            btn_ChangeCelsius.IsEnabled = false;
            //new ShowDialogCustom("Đã Đổi Thành Công", "Thông Báo", ShowDialogCustom.OK).ShowDialog();
        }

        private void btn_ChangeDoF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var bc = new BrushConverter();
                btn_ChangeCelsius.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                btn_ChangeCelsius.Foreground = (Brush)bc.ConvertFrom("#1A1A1A");
                btn_ChangeFahrenheit.Background = (Brush)bc.ConvertFrom("#1A1A1A");
                btn_ChangeFahrenheit.Foreground = (Brush)bc.ConvertFrom("#FFFFFF");
                double tempcurr = Convert.ToDouble(Lb_TempCurr.Content);
                Lb_TempCurr.Content = Math.Round(toFahr(tempcurr), 0).ToString();
                Text_Unit.Content = "°F";
                for (int i = 0; i < 24; i++)
                {
                    if (GetChildrenControl(i) != null)
                    {
                        Control cardhours = GetChildrenControl(i);
                        double temp24h = Convert.ToDouble((cardhours as UserController.CardHours).Temp.Replace("°C", ""));
                        (cardhours as UserController.CardHours).Temp = Math.Round(toFahr(temp24h), 0).ToString() + "°F";
                    }
                }

                double tempMIN = Convert.ToDouble(Lb_tempmin.Content.ToString().Replace("°C", ""));
                Lb_tempmin.Content = Math.Round(toFahr(tempMIN), 0) + "°F";
                double tempMAX = Convert.ToDouble(Lb_tempmax.Content.ToString().Replace("°C", ""));
                Lb_tempmax.Content = Math.Round(toFahr(tempMAX), 0) + "°F";
                //
            }
            catch (Exception)
            {

            }
            btn_ChangeFahrenheit.IsEnabled = false;
            btn_ChangeCelsius.IsEnabled = true;
        }

        private void Setting_btn_Click(object sender, RoutedEventArgs e)
        {
            Setting setting = new Setting();
            setting.ShowDialog();
        }
    }
}
