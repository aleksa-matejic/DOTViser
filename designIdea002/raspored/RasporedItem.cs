using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designIdea002.raspored
{
    class RasporedItem
    {
        public RasporedItem(string broj, string smerSkraceno, string smer, string semestar, string link)
        {
            this.Broj = broj;
            this.SmerSkraceno = smerSkraceno;
            this.Smer = smer;
            this.Semestar = semestar;
            this.Link = link;
        }

        private string broj;
        private string smerSkraceno;
        private string smer;
        private string semestar;
        private string link;

        public string Broj { get { return broj; } set { broj = value; } }
        public string SmerSkraceno { get { return smerSkraceno; } set { smerSkraceno = value; } }
        public string Smer { get { return smer; } set { smer = value; } }
        public string Semestar { get { return semestar; } set { semestar = value; } }
        public string Link { get { return link; } set { link = value; } }
    }
}
