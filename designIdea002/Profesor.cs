using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designIdea002
{
    public class ProfesorC
    {
        public string idProfesor { get; set; }
        public string imePrezime { get; set; }
        public string titula { get; set; }
        public string zvanje { get; set; }
        public string slika { get; set; }
        public string konsultacije { get; set; }
        public string kabinet { get; set; }
        public string mail { get; set; }
        public string biografija { get; set; }
        public string aktivnost { get; set; }

        public ProfesorC(string idProfesor, string imePrezime, string titula, string zvanje, string slika, string konsultacije, string kabinet, string mail, string biografija, string aktivnost)
        {
            this.idProfesor = idProfesor;
            this.imePrezime = imePrezime;
            this.titula = titula;
            this.zvanje = zvanje;
            this.slika = slika;
            this.konsultacije = konsultacije;
            this.kabinet = kabinet;
            this.mail = mail;
            this.biografija = biografija;
            this.aktivnost = aktivnost;
        }

    }
}
