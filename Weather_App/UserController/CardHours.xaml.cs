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
    /// Interaction logic for CardHours.xaml
    /// </summary>
    public partial class CardHours : UserControl
    {
        public CardHours()
        {
            InitializeComponent();
        }
        public string Hours
        {
            get
            {
                return (string)GetValue(HoursProperty);
            }
            set
            {
                SetValue(HoursProperty, value);
            }
        }

        public static readonly DependencyProperty HoursProperty = DependencyProperty.Register("Hours", typeof(string), typeof(CardHours));


        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(CardHours));


        public string Temp
        {
            get
            {
                return (string)GetValue(TempProperty);
            }
            set
            {
                SetValue(TempProperty, value);
            }
        }

        public static readonly DependencyProperty TempProperty = DependencyProperty.Register("Temp", typeof(string), typeof(CardHours));

    }
}
