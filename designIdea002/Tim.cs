using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designIdea002
{
    class Tim
    {
        public string id_tim { get; set; }
        public string id_preds { get; set; }
        public string ime { get; set; }
        public string glavna_slika { get; set; }
        public string pozadinska_slika { get; set; }
        public string ikonica { get; set; }
        public string podnaslov { get; set; }
        public string tekst { get; set; }

        public Tim(string id_tim, string id_preds, string ime, string glavna_slika, string pozadinska_slika, string ikonica, string podnaslov, string tekst)
        {
            this.id_tim = id_tim;
            this.id_preds = id_preds;
            this.ime = ime;
            this.glavna_slika = glavna_slika;
            this.pozadinska_slika = pozadinska_slika;
            this.ikonica = ikonica;
            this.podnaslov = podnaslov;
            this.tekst = tekst;
        }
    }
}
