using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather_App
{
    public class Cons
    {
        private static Cons? instance;
        public IDictionary<string, string> signalist;
        public List<IconWes> List;
        public static Cons Instant
        {
            get
            {
                if (instance == null)
                {
                    instance = new Cons(); 
                }
                    
                return Cons.instance;
            }
            private set { Cons.instance = value; }
        }
        private Cons() {
            signalist = new Dictionary<string, string>()
            {
                {"S", "Nam" },
                {"E", "Đông" },
                {"W", "Tây" },
                {"N", "Bắc" },
                {"SW", "Tây Nam" },
                {"NW", "Tây Bắc" },
                {"NE", "Đông Bắc" },
                {"SE", "Đông Nam" },
                {"SSE", "Nam Đông Nam" },
                {"SSW", "Nam Tây Nam" },
                {"WSW", "Tây Tây Nam" },
                {"WNW", "Tây Tây Bắc" },
                {"NNW", "Bắc Tây Bắc" },
                {"NNE", "Bắc Đông Bắc" },
                {"ENE", "Đông Đông Bắc" },
                {"ESE", "Đông Đông Nam" },
            };
            //List = new List<IconWes>()
            //{
            //    new IconWes("200","Có giông kèm mưa nhẹ","t01d","t01n"),
            //    new IconWes("201","Có giông kèm mưa","t02d","t02n"),
            //    new IconWes("202","Có giông kèm mưa lớn","t03d","t03n"),
            //    new IconWes("230","Bão có mưa phùn nhẹ","t04d","t04n"),
            //    new IconWes("231","Bão có mưa phùn","t04d","t04n"),
            //    new IconWes("232","Bão có mưa phùn lớn","t04d","t04n"),
            //    new IconWes("233","Sấm sét kèm theo mưa đá","t05d","t05n"),
            //    new IconWes("300","Mưa phùn nhẹ","d01d","d01n"),
            //    new IconWes("301","Mưa phùn","d02d","d02n"),
            //    new IconWes("302","Mưa phùn nặng hạt","d03d","d03n"),
            //    new IconWes("500","Mưa nhỏ","r01d","r01n"),
            //    new IconWes("501","Mưa vừa phải","r02d","r02n"),
            //    new IconWes("502","Mưa nặng hạt","r03d","r03n"),
            //    new IconWes("511","Mưa đóng băng","f01d","f01n"),
            //    new IconWes("520","Mưa rào nhẹ","r04d","r04n"),
            //    new IconWes("521","Tắm mưa","r05d","r05n"),
            //    new IconWes("522","Mưa rào","r06d","r06n"),
            //    new IconWes("600","Tuyết nhẹ","s01d","s01n"),
            //    new IconWes("601","Tuyết","s02d","s02n"),
            //    new IconWes("602","Tuyết rơi nhiều","s03d","s03n"),
            //    new IconWes("610","Kết hợp tuyết, mưa","s04d","s04n"),
            //    new IconWes("611","Tuyết hơi tan, mưa","s05d","s05n"),
            //    new IconWes("612","Mưa tuyết dày đặc","s05d","s05n"),
            //    new IconWes("621","Tắm tuyết","s01d","s01n"),
            //    new IconWes("622","Mưa tuyết dày đặc","s02d","s02n"),
            //    new IconWes("623","Trận mưa rào","s06d","s06n"),
            //    new IconWes("700","Sương mù","a01d","a01n"),
            //    new IconWes("711","Khói","a02d","a02n"),
            //    new IconWes("721","Sương mù","a03d","a03n"),
            //    new IconWes("731","Cát / bụi","a04d","a04n"),
            //    new IconWes("741","Sương mù","a05d","a05n"),
            //    new IconWes("751","Sương mù đóng băng","a06d","a06n"),
            //    new IconWes("800","Bầu trời quang đãng","c01d","c01n"),
            //    new IconWes("801","Vài đám mây","c02d","c02n"),
            //    new IconWes("802","Mây rải rác","c02d","c02n"),
            //    new IconWes("803","Mây tan vỡ","c03d","c03n"),
            //    new IconWes("804","Mây u ám","c04d","c04n"),
            //    new IconWes("900","Lượng mưa không xác định","u00d","u00n")
            //};
        }
        

        
        public static IconWes? GetIconWes(string id)
        {
            foreach (var item in instance.List)
            {
                if(item.IDIcon == id)
                    return item;
            }
            return null;
        }
        public class IconWes
        {
            private string? iDIcon;
            private string? nameIconnight;
            private string? nameIcondaytime;

            private string? descriptionIcon;

            public string? IDIcon
            {
                get { return iDIcon; }
                set { iDIcon = value; }
            }
            public string? NameIconnight
            {
                get { return nameIconnight; }
                set { nameIconnight = value; }
            }
            public string? NameIconDaytime
            {
                get { return nameIcondaytime; }
                set { nameIcondaytime = value; }
            }
            public string? DescriptionIcon
            {
                get { return descriptionIcon; }
                set { descriptionIcon = value; }
            }
            public IconWes(string iIcon, string descriptionIcon, string nameIcon, string nameIcondaytime)
            {
                this.IDIcon = iIcon;
                this.nameIconnight = nameIcon;
                this.DescriptionIcon = descriptionIcon;
                this.nameIcondaytime = nameIcondaytime;
            }
        }


    }
}
