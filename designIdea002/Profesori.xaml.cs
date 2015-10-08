using designIdea002.Common;
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
using Windows.UI.Xaml.Shapes;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace designIdea002
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Profesori : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        int maxCnt = 7;
        ListView lvProfesori = new ListView();
        public Profesori()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            
            baza = new Baza();

            using (reader = baza.izvrsiUpit(upit))
            {
                while (reader.Read())
                {
                    profesori.Add(new ProfesorC(reader.GetString("idProfesor"), reader.GetString("imePrezime"), reader.GetString("titula"), reader.GetString("zvanje"), reader.GetString("slika"), reader.GetString("konsultacije"), reader.GetString("kabinet"), reader.GetString("mail"), reader.GetString("biografija"), reader.GetString("aktivnost")));
                }
            }

            spProfesori.Margin = new Thickness(100, 50, 100, 100);

            lvProfesori.Tapped += lvProfesori_Tapped;
            lvProfesori.Width = 500;
            lvProfesori.HorizontalAlignment = HorizontalAlignment.Left;
            lvProfesori.IsSwipeEnabled = false;

            spProfesori.Children.Add(lvProfesori);

            StackPanel sp1 = new StackPanel();
            TextBlock tbSlovo = new TextBlock();

            tbSlovo.Text = "А";
            tbSlovo.FontSize = 60;
            tbSlovo.Margin = new Thickness(5, 0, 0, 0);
            sp1.Background = new SolidColorBrush(Colors.RoyalBlue);
            sp1.Width = 500;
            sp1.Children.Add(tbSlovo);

            lvProfesori.Items.Add(sp1);
            
            foreach(ProfesorC p in profesori)
            {
                if (lvProfesori.Items.Count == maxCnt)
                {
                    lvProfesori = new ListView();
                    lvProfesori.IsSwipeEnabled = false;
                    lvProfesori.Width = 500;
                    lvProfesori.HorizontalAlignment = HorizontalAlignment.Left;
                    lvProfesori.Tapped += lvProfesori_Tapped;
                    spProfesori.Children.Add(lvProfesori);
                }
                if (tbSlovo.Text.Equals(p.imePrezime.Substring(0, 1)))
                {
                    dodajStavkuListe(p);
                }
                else
                {
                    tbSlovo = new TextBlock();
                    tbSlovo.FontSize = 60;
                    tbSlovo.Margin = new Thickness(5, 0, 0, 0);
                    tbSlovo.Text = p.imePrezime.Substring(0, 1);
                    if (lvProfesori.Items.Count == maxCnt)
                    {
                        lvProfesori = new ListView();
                        lvProfesori.Width = 500;
                        lvProfesori.HorizontalAlignment = HorizontalAlignment.Left;
                        lvProfesori.Tapped += lvProfesori_Tapped;
                        spProfesori.Children.Add(lvProfesori);
                    }

                    sp1 = new StackPanel();
                    sp1.Background = new SolidColorBrush(Colors.RoyalBlue);
                    sp1.Width = 500;
                    sp1.Children.Add(tbSlovo);

                    lvProfesori.Items.Add(sp1);
                    
                    dodajStavkuListe(p);
                }
            }
        }

        void lvProfesori_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // ovo je nabudzeno do jaja
            int a;
            ListView lv;
            StackPanel sp;
            string s;
            try
            {
                a = (sender as ListView).SelectedIndex;
                lv = sender as ListView;
                sp = lv.Items[a] as StackPanel;
                s = (string)sp.Tag;
                if (s == null)
                {
                    return;
                }
            }
            catch (Exception exc)
            {
                string ex = exc.ToString();
                return;
            }
            this.Frame.Navigate(typeof(Profesor), s);
        }

        List<ProfesorC> profesori = new List<ProfesorC>();
        Baza baza;
        MySqlDataReader reader;
        string upit = "SELECT idProfesor, imePrezime, titula, zvanje, slika, konsultacije, kabinet, mail, biografija, aktivnost FROM profesori ORDER BY imePrezime";


        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion



        public void dodajStavkuListe(ProfesorC p)
        {
            StackPanel sp = new StackPanel();
            sp.Tag = p.idProfesor;
            Grid g = new Grid();
            Rectangle r = new Rectangle();
            TextBlock tb = new TextBlock();

            

            ColumnDefinition c = new ColumnDefinition();
            c.Width = new GridLength(1, GridUnitType.Auto);

            ColumnDefinition c1 = new ColumnDefinition();
            c1.Width = new GridLength(1, GridUnitType.Star);

            g.ColumnDefinitions.Add(c);
            g.ColumnDefinitions.Add(c1);

            r.Height = 70;
            r.Width = 70;
            ImageBrush imageBrush = new ImageBrush();
            string putanja = "ms-appx://" + p.slika;

            imageBrush.ImageSource = new BitmapImage(new Uri(putanja, UriKind.RelativeOrAbsolute));
            r.Fill = imageBrush;
            r.Margin = new Thickness(5, 5, 5, 5);

            tb.Text = p.titula + " " + p.imePrezime;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.FontSize = 20;
            tb.Margin = new Thickness(15, 0, 0, 0);

            g.Children.Add(r);
            g.Children.Add(tb);
            Grid.SetColumn(r, 0);
            Grid.SetColumn(tb, 1);

            sp.Children.Add(g);
            if (lvProfesori.Items.Count == maxCnt)
            {
                lvProfesori = new ListView();
                lvProfesori.Width = 500;
                lvProfesori.HorizontalAlignment = HorizontalAlignment.Left;
                lvProfesori.Tapped += lvProfesori_Tapped;
                spProfesori.Children.Add(lvProfesori);
            }
            lvProfesori.Items.Add(sp);

        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
        
    }
}
