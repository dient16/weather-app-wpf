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
using System.Windows.Shapes;

namespace Weather_App.UserController
{
    /// <summary>
    /// Interaction logic for ShowDialogCustom.xaml
    /// </summary>
    public partial class ShowDialogCustom : Window
    {
        public static int YesNo = 1;
        public static int OK = 0;
        public ShowDialogCustom()
        {
            InitializeComponent();
        }

        public ShowDialogCustom(string mess, string title, int mode) : this()
        {
            try
            {
                this.Title = title;
                txblMess.Text = mess;
                if (mode == YesNo)
                {
                    btnOK.Visibility = Visibility.Hidden;
                }
                else
                {
                    btnYes.Visibility = Visibility.Hidden;
                    btnNo.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Lỗi DialogCustom :" + ex.Message);
            }

        }
        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
