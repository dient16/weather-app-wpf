using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Weather_App
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : Window
    {
        public Setting()
        {
            InitializeComponent();
        }
        private void SaveSettting()
        {
            JObject @object = new JObject();
            @object["cb_Seach_LatLon"] = (bool)Seach_LatLon.IsChecked ? 1 : 0;
            @object["cb_Seach_city"] = (bool)Seach_city.IsChecked ? 1 : 0;

            @object["Key_openweathermap"] = txtKey_openweathermap.Text;
            @object["Location_default"] = txt_Location.Text;
            @object["Minute_Update"] = Int32.Parse(txt_minuteUpdate.Text);
            @object["Key_weatherapi"] = txtKey_weatherapi.Text;
            @object["Key_weatherbit"] = txtKey_weatherbit.Text;

            File.WriteAllText(@"DATA\setting.json", @object.ToString());

        }
        private void LoadSettings()
        {
            JObject @object = JObject.Parse(File.ReadAllText(@"DATA\setting.json"));
            if (Convert.ToInt32(@object["cb_Seach_LatLon"]) == 1)
                Seach_LatLon.IsChecked = true;

            if (Convert.ToInt32(@object["cb_Seach_city"]) == 1)
                Seach_city.IsChecked = true;

            txtKey_openweathermap.Text = @object["Key_openweathermap"].ToString();
            txtKey_weatherapi.Text = @object["Key_weatherapi"].ToString();
            txt_Location.Text = @object["Location_default"].ToString();
            txt_minuteUpdate.Text = @object["Minute_Update"].ToString();
            txtKey_weatherbit.Text = @object["Key_weatherbit"].ToString();
        }
        private void SettingWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettting();
        }

        private void SettingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();
        }
    }
}
