using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designIdea002
{
    class Baza
    {
        public MySqlConnection connection;
        public MySqlDataReader reader;
        public string baza = "server=127.0.0.1;database=dot;uid=root;password=;";

        // otvaranje konekcije na bazu
        public Baza()
        {
            connection = new MySqlConnection(baza);
            connection.Open(); // baca exception
        }

        public MySqlDataReader izvrsiUpit(string upit)
        {
            MySqlCommand komanda = new MySqlCommand(upit, connection);
            reader = komanda.ExecuteReader(); // baca exception
            return reader;
        }

        /*public string imena = "";
        public MySqlConnection konekcija()
        {
            using (connection = new MySqlConnection("server=127.0.0.1;database=proba;uid=root;password=;"))
            {
                connection.Open();
                MySqlCommand upit = new MySqlCommand("SELECT ime FROM student", connection);

                using (MySqlDataReader reader = upit.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        imena += reader.GetString("ime");
                    }
                }

                MySqlCommand upit1 = new MySqlCommand("INSERT INTO student(ime) VALUES('Jebac')", connection);
                for (int i = 0; i < 5; i++)
                    using (MySqlDataReader reader1 = upit1.ExecuteReader()) ;


                return connection;
            }
        }*/
    }
}
