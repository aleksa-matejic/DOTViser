using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.UI.Xaml;
namespace designIdea002.RSS
{
    public   class NewsItem
    {
        [XmlElement("title")]
        public string Title { get; set; }
        [XmlElement("description")]
        public string Description { get; set; }
        [XmlElement("link")]
        public string link { get; set; }
        [XmlElement("pubDate")]
        public string pubDate { get; set; }

        private string dan, mesec;
        private Visibility vidljivost;
        private string prikaz;

        public Visibility Vidljivost
        {
            get { return vidljivost; }
            set { vidljivost = value; }
        }
        public string Prikaz
        {
            get { return prikaz; }
            set { prikaz = value; }
        }
        public string Dan
        {
            get{return dan;}
            set { dan = value; }
        }
        public string Mesec
        {
            get { return mesec; }
            set { mesec = value; }
        }

        public void setDATE()
        {
           
            string[] linija =pubDate.Split('-');
            switch(linija[1])
            {
                     case("01"): Mesec="Januar";
                            break;
                     case("02"): Mesec="Februar";
                            break;
                     case("03"): Mesec="Mart";
                            break;
                     case("04"): Mesec="April";
                            break;
                     case("05"): Mesec="Maj";
                            break;
                     case("06"): Mesec="Jun";
                            break;
                     case("07"): Mesec="Jul";
                            break;
                     case("08"): Mesec="Avgust";
                            break;
                     case("09"): Mesec="Septembar";
                            break;
                     case("10"): Mesec="Oktobar";
                            break;
                     case("11"): Mesec="Novembar";
                            break;
                     case("12"): Mesec="Decembar";
                            break;
            }
            Dan=linija[2].Substring(0,2);

        }
    }
}
