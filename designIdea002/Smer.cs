using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designIdea002
{
    class Smer
    {
        public int id_smer { get; set; }
        public string naziv { get; set; }
        public string skracenica { get; set; }
        public int id_ruk { get; set; }
        public int id_sek { get; set; }
        public string slika { get; set; }
        public string boja { get; set; }
        public string opis { get; set; }
        public string kompetencije { get; set; }

        public Smer(int id_smer, string naziv, string skracenica, int id_ruk, int id_sek, string slika, string boja, string opis, string kompetencije)
        {
            this.id_smer = id_smer;
            this.naziv = naziv;
            this.skracenica = skracenica;
            this.id_ruk = id_ruk;
            this.id_sek = id_sek;
            this.slika = slika;
            this.boja = boja;
            this.opis = opis;
            this.kompetencije = kompetencije;
        }



    }
}
