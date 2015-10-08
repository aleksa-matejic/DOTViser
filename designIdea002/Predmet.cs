using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designIdea002
{
    class Predmet
    {
        public string id_predm { get; set; }
        public string naziv { get; set; }
        
        public Predmet(string id_predm, string naziv)
        {
            this.id_predm = id_predm;
            this.naziv = naziv;
        }
    }
}
