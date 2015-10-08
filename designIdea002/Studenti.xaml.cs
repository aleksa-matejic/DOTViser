using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class Studenti : Page
    {
        string xml = "";
        MyXmlReader ride;
        public int idPageSent { get; set; }
        public Studenti()
        {
            this.InitializeComponent();

            string naslov = "", opis = "", velikaSlika = "", malaSlika = "", tekst = "";

            ride = new MyXmlReader(@"Assets\StudentskeVesti1.xml");
            idPageSent = 2;
            upisVesti(idPageSent);
        }

        public void upisVesti(int idPageSent)
        {
            int[] nizId = new int[4];
            int id;
            ride.getText();

            id = ride.getMaxId();

            int brojac = 0;
            while (brojac < 4)
            {
                if (id != idPageSent)
                {
                    nizId[brojac] = id;
                    brojac++;
                }
                id--;
            }

            xml = ride.getTeksti();
            StudentskeVesti vest1 = ride.action(idPageSent);
            GlavniGrid.DataContext = vest1;


            StudentskeVesti vest2 = ride.action(nizId[0], xml);
            artikal1.DataContext = vest2;
            artikal1.Tag = nizId[0];

            StudentskeVesti vest3 = ride.action(nizId[1], xml);
            artikal2.DataContext = vest3;
            artikal2.Tag = nizId[1];

            StudentskeVesti vest4 = ride.action(nizId[2], xml);
            artikal3.DataContext = vest4;
            artikal3.Tag = nizId[2];

            StudentskeVesti vest5 = ride.action(nizId[3], xml);
            artikal4.DataContext = vest5;
            artikal4.Tag = nizId[3];
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void infoWork_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private void ArtikliLevo_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private void artikal2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GlavniGridScroll.ScrollToHorizontalOffset(0);
            StackPanel ss = sender as StackPanel;
            int id;
            id = Convert.ToInt32(ss.Tag);

            upisVesti(id);
        }

        private void artikal1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GlavniGridScroll.ScrollToHorizontalOffset(0);
            StackPanel ss = sender as StackPanel;
            int id;
            id = Convert.ToInt32(ss.Tag);

            upisVesti(id);
        }

        private void artikal3_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GlavniGridScroll.ScrollToHorizontalOffset(0);
            StackPanel ss = sender as StackPanel;
            int id;
            id = Convert.ToInt32(ss.Tag);

            upisVesti(id);
        }

        private void artikal4_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GlavniGridScroll.ScrollToHorizontalOffset(0);
            StackPanel ss = sender as StackPanel;
            int id;
            id = Convert.ToInt32(ss.Tag);

            upisVesti(id);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            idPageSent= Convert.ToInt32(e.Parameter);
            upisVesti(idPageSent);
        }
        

      
    }

    
}
