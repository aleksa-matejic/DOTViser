using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class Profesor : Page
    {
        string id;
        string upit;
        string upit1;
        MySqlDataReader reader;
        ProfesorC profesor;
        public Profesor()
        {
            this.InitializeComponent();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void AppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            mejl.FontSize = 25;
            mejl.Text = profesor.mail;
        }

        private void AppBarButton_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            mejl.FontSize = 25;
            mejl.Text = "кабинет: " + profesor.kabinet;
        }

        private void AppBarButton_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            mejl.FontSize = 25;
            mejl.Text = profesor.konsultacije;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            id = e.Parameter.ToString();
            napuni();
        }

        public void napuni()
        {
            Baza baza = new Baza();
            string kabinet, mejl, konsultacije = "";
            string biografija = "Нема описа.";
            string aktivnost = "Нема описа.";

            upit = "SELECT * FROM profesori WHERE idprofesor = " + id;
            using (reader = baza.izvrsiUpit(upit))
            {
                while (reader.Read())
                {
                    if (reader.GetString("biografija") != "") biografija = reader.GetString("biografija");
                    if (reader.GetString("aktivnost") != "") aktivnost = reader.GetString("aktivnost");

                    profesor = new ProfesorC(reader.GetString("idProfesor"), reader.GetString("imePrezime"), reader.GetString("titula"), reader.GetString("zvanje"), reader.GetString("slika"), reader.GetString("konsultacije"), reader.GetString("kabinet"), reader.GetString("mail"), biografija, aktivnost);
                    gridProfesor.DataContext = profesor;
                }
            }

            upit1 = "SELECT * FROM predmeti WHERE id_predm IN (SELECT id_predm FROM pplink WHERE id_prof = " + id + ")";
            using (reader = baza.izvrsiUpit(upit1))
            {
                while (reader.Read())
                {
                    Predmet p = new Predmet(reader.GetString("id_predm"), reader.GetString("naziv"));
                    lvPredmeti.Items.Add(p);
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (tbAktivnosti.Text == "Нема описа.")
                tbAktivnosti.FontStyle = Windows.UI.Text.FontStyle.Italic;
            if (tbBiografija.Text == "Нема описа.")
                tbBiografija.FontStyle = Windows.UI.Text.FontStyle.Italic;
        }

    }
}
