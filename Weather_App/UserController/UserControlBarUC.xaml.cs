using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Weather_App.UserController
{
    /// <summary>
    /// Interaction logic for UserControlBarUC.xaml
    /// </summary>
    public partial class UserControlBarUC : UserControl
    {
        public UserControlBarUC()
        {
            InitializeComponent();
        }
        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Button? btn_close = sender as Button;
            Window mainwindows = Window.GetWindow(btn_close);
            mainwindows.Close();

        }

        private void Button_Maximize(object sender, RoutedEventArgs e)
        {
            Button? btn_close = sender as Button;
            Window mainwindows = Window.GetWindow(btn_close);
            if (mainwindows != null)
            {
                if (mainwindows.WindowState != WindowState.Maximized)
                {
                    mainwindows.WindowState = WindowState.Maximized;
                    btn_maximize.ToolTip = "Normal";
                    ucControlBar.MinHeight = 1;
                }
                else
                {
                    mainwindows.WindowState = WindowState.Normal;
                    btn_maximize.ToolTip = "Maximize";
                    ucControlBar.MinHeight = 0;
                }
            }
        }
        private void Button_Minimize(object sender, RoutedEventArgs e)
        {
            Button? btn_close = sender as Button;
            Window mainwindows = Window.GetWindow(btn_close);
            mainwindows.WindowState = WindowState.Minimized;
        }
    }
}
