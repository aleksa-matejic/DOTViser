using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designIdea002
{
    class Student
    {
        public string id_stud { get; set; }
        public string ime_prezime { get; set; }
        public string mail { get; set; }
        public string slika { get; set; }
        public string opis { get; set; }

        public Student(string id_stud, string ime_prezime, string mail, string slika, string opis)
        {
            this.id_stud = id_stud;
            this.ime_prezime = ime_prezime;
            this.mail = mail;
            this.slika = slika;
            this.opis = opis;
        }
    }
}
