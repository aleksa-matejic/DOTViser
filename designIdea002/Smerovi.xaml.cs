using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace designIdea002
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Smerovi : Page
    {
        public Smerovi()
        {
            this.InitializeComponent();
            // napraviListuTimova();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        void napraviListuTimova()
        {
            Baza baza = new Baza();
            MySqlDataReader reader;
            string upit = "SELECT * FROM timovi WHERE id_tim = " + id_tim;

            string id_preds = "";
            using (reader = baza.izvrsiUpit(upit))
            {
                while (reader.Read())
                {
                    id_preds = reader.GetString("id_preds");
                    frameTimovi.DataContext = new Tim(reader.GetString("id_tim"), reader.GetString("id_preds"), reader.GetString("ime"), reader.GetString("glavna_slika"), reader.GetString("pozadinska_slika"), reader.GetString("ikonica"), reader.GetString("podnaslov"), reader.GetString("tekst"));
                }
            }

            
            upit = "SELECT * FROM studenti WHERE id_stud = " + id_preds;

            using (reader = baza.izvrsiUpit(upit))
            {
                while (reader.Read())
                {

                    rectPredstavnik.DataContext = new Student(reader.GetString("id_stud"), reader.GetString("ime_prezime"), reader.GetString("mail"), reader.GetString("slika"), reader.GetString("opis"));
                    spPredstavnik.DataContext = new Student(reader.GetString("id_stud"), reader.GetString("ime_prezime"), reader.GetString("mail"), reader.GetString("slika"), reader.GetString("opis"));
                }
            }

            upit = "SELECT * FROM studenti WHERE id_stud IN (SELECT id_stud FROM tslink WHERE id_tim = " + id_tim + " AND id_stud != " + id_preds + ")";

            using (reader = baza.izvrsiUpit(upit))
            {
                while (reader.Read())
                {
                    // itemListView je lista studenata koji su u timu
                    itemListView.Items.Add(new Student(reader.GetString("id_stud"), reader.GetString("ime_prezime"), reader.GetString("mail"), reader.GetString("slika"), reader.GetString("opis")));
                }
            }
        }

        string id_tim;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            id_tim = e.Parameter.ToString();
            napraviListuTimova();
        }
    }
}
