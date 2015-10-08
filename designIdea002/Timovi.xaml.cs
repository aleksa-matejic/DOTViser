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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace designIdea002
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Timovi : Page
    {
        string id = "";
        string upit = "";
        string upit1;
        public Timovi()
        {
            this.InitializeComponent();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            id = e.Parameter as string;
            napuni();
        }

        private void napuni()
        {
            Baza baza = new Baza();

            
            upit = "SELECT * FROM smerovi WHERE id_smer = " + id;
            string naziv = "";
            int duzinaStringa = 0;
            MySqlDataReader reader;
            using (reader = baza.izvrsiUpit(upit))
            {
                while (reader.Read())
                {
                    naziv = reader.GetString("naziv");
                    duzinaStringa = reader.GetString("naziv").Length;
                    //if (duzinaStringa > 18) naziv = reader.GetString("skracenica");

                    Smer smer = new Smer(reader.GetInt32("id_smer"), naziv, reader.GetString("skracenica"), reader.GetInt32("id_smer"), reader.GetInt32("id_smer"), reader.GetString("slika"), reader.GetString("boja"), reader.GetString("opis"), reader.GetString("kompetencije"));
                    gridSmer.DataContext = smer;
                    upit = "SELECT * FROM profesori WHERE idProfesor = " + reader.GetString("id_ruk");
                    upit1 = "SELECT * FROM profesori WHERE idProfesor = " + reader.GetString("id_sek");
                }
            }

            using (reader = baza.izvrsiUpit(upit))
            {
                while (reader.Read())
                {
                    ProfesorC p = new ProfesorC(reader.GetString("idProfesor"), reader.GetString("imePrezime"), reader.GetString("titula"), reader.GetString("zvanje"), reader.GetString("slika"), reader.GetString("konsultacije"), reader.GetString("kabinet"), reader.GetString("mail"), reader.GetString("biografija"), reader.GetString("aktivnost"));
                    lvRukovodioci.Items.Add(p);
                }
            }

            using (reader = baza.izvrsiUpit(upit1))
            {
                while (reader.Read())
                {
                    ProfesorC p = new ProfesorC(reader.GetString("idProfesor"), reader.GetString("imePrezime"), reader.GetString("titula"), reader.GetString("zvanje"), reader.GetString("slika"), reader.GetString("konsultacije"), reader.GetString("kabinet"), reader.GetString("mail"), reader.GetString("biografija"), reader.GetString("aktivnost"));
                    lvRukovodioci.Items.Add(p);
                }
            }

            upit = "SELECT naziv, semestar FROM predmeti as P, splink as S WHERE id_smer =" + id +" AND P.id_predm = S.id_predm";

            ListViewItem lvi = new ListViewItem();
            lvi.Content = "I година";
            lvi.FontSize = 18;
            lvi.Background = new SolidColorBrush(Colors.RoyalBlue);
            lvi.VerticalContentAlignment = VerticalAlignment.Center;
            lvi.Padding = new Thickness(5, 0, 0, 0);
            lvSpisakPredmeta.Items.Add(lvi);

            int status = 1;

            using (reader = baza.izvrsiUpit(upit))
            {
                while (reader.Read())
                {
                    if ((reader.GetString("semestar").Equals("3") || reader.GetString("semestar").Equals("4")) && (status == 1))
                    {
                        lvi = new ListViewItem();
                        lvi.Content = "II година";
                        lvi.FontSize = 18;
                        lvi.Background = new SolidColorBrush(Colors.Purple);
                        lvi.VerticalContentAlignment = VerticalAlignment.Center;
                        lvi.Padding = new Thickness(5, 0, 0, 0);
                        lvSpisakPredmeta.Items.Add(lvi);
                        status = 2;
                    }
                    else if ((reader.GetString("semestar").Equals("5") || reader.GetString("semestar").Equals("6")) && (status == 2))
                    {
                        lvi = new ListViewItem();
                        lvi.Content = "III година";
                        lvi.FontSize = 18;
                        lvi.Background = new SolidColorBrush(Colors.Pink);
                        lvi.VerticalContentAlignment = VerticalAlignment.Center;
                        lvi.Padding = new Thickness(5, 0, 0, 0);
                        lvSpisakPredmeta.Items.Add(lvi);
                        status = 0;
                    }
                    
                    lvi = new ListViewItem();
                    lvi.BorderThickness = new Thickness(4, 0, 0, 0);
                    lvi.BorderBrush = new SolidColorBrush(Colors.RoyalBlue);
                    if (status == 2)
                    {
                        lvi.BorderBrush = new SolidColorBrush(Colors.Purple);
                    }
                    else if (status == 0)
                    {
                        lvi.BorderBrush = new SolidColorBrush(Colors.Pink);
                    }
                    lvi.VerticalContentAlignment = VerticalAlignment.Center;
                    lvi.Padding = new Thickness(5, 0, 0, 0);
                    lvi.Content = reader.GetString("naziv");

                    lvSpisakPredmeta.Items.Add(lvi);

                }
            }
        }

        private void lvRukovodioci_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string s;
            try
            {
                int ind = (sender as ListView).SelectedIndex;
                ListView lv = sender as ListView;
                ProfesorC p = (ProfesorC)lv.Items[ind];
                s = p.idProfesor;
            }
            catch (Exception exc)
            {
                string ex = exc.ToString();
                return;
            }
            this.Frame.Navigate(typeof(Profesor), s);
        }
    }
}
