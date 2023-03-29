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
    /// Interaction logic for CardSunriseSunset.xaml
    /// </summary>
    public partial class CardSunriseSunset : UserControl
    {
        public CardSunriseSunset()
        {
            InitializeComponent();
        }
        private void valueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                fillPathRT.Angle = valueSlider.Value;

                var angle = 180 + valueSlider.Value;
                double radius = 100 - 22;
                double centerX = needleLine.X1, centerY = needleLine.Y1;

                double x2 = (radius * Math.Cos(angle * Math.PI / 180)) + centerX;
                double y2 = (radius * Math.Sin(angle * Math.PI / 180)) + centerY;

                needleLine.X2 = x2;
                needleLine.Y2 = y2;
                Canvas.SetLeft(Sun, x2 - 25);
                Canvas.SetTop(Sun, y2 - 20);    
               

            }
            catch (Exception)
            {

                throw;
            }
        }

        public int Amount
        {
            get { return (int)GetValue(AmountProperty); }
            set { SetValue(AmountProperty, value); }
        }

        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register("Amount", typeof(int), typeof(CardSunriseSunset));
        public int Deg
        {
            get { return (int)GetValue(DegProperty); }
            set { SetValue(DegProperty, value); }
        }

        public static readonly DependencyProperty DegProperty = DependencyProperty.Register("Deg", typeof(int), typeof(CardSunriseSunset));
    }
}
