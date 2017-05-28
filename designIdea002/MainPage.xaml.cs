using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
using designIdea002.RSS;
using designIdea002.RSS;
using designIdea002.Common;
using Windows.Data.Pdf;
using System.Collections.ObjectModel;
using designIdea002.Data;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Animation;
using MySql.Data.MySqlClient;
using designIdea002.raspored;
using System.Text.RegularExpressions;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238


namespace designIdea002
{

    
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private MainViewModel ViewModel { get; set; }


        List<string> desc = new List<string>();
        NewsItem preth = null;
        bool flag = true;
        int index = 0;
        public MainPage()
        {
            this.InitializeComponent();
            napuniSmerovi();
            profesori();
            timovi();
            napuniRaspored();
            initDispatcher();
            this.ViewModel = new MainViewModel();
            this.DataContext = this.ViewModel;
            //ReadTextFile();
            ucitajPozadinskeSlike();
            
            vezaRSS();
             
        }
        private  void vezaRSS()
        {

            string url = "http://www.viser.edu.rs/xml/rss-vesti.php";
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.BeginGetResponse(new AsyncCallback(ReadRss), request);

        }
        // Aleksa: fix for not loaded chosen pdf - changed privacy to public, added static and task instead of void
        public static async System.Threading.Tasks.Task downloadPdf(String urlLink)
        {
            HttpClient client = new HttpClient();
            using (IInputStream inputStream = await client.GetInputStreamAsync(new Uri("http://www.viser.edu.rs/" + urlLink, UriKind.Absolute)))
            {
                Stream webStream = inputStream.AsStreamForRead();
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("2.pdf", CreationCollisionOption.ReplaceExisting);

                //  StorageFile file = await Windows.ApplicationModel.Package.Current.InstalledLocation.CreateFileAsync("2.pdf",CreationCollisionOption.ReplaceExisting);

                Stream fileStream = await file.OpenStreamForWriteAsync();
                await webStream.CopyToAsync(fileStream);
                // MessageDialog md = new MessageDialog("File Saved");

                // await md.ShowAsync();
                webStream.Dispose();
                fileStream.Dispose();
            }
        }
        private async void ReadRss(IAsyncResult result)
        {
            // Aleksa TODO: added try and catch block because when no internet connection method throws some exception, investigate this
            try
            {
                HttpWebRequest request = result.AsyncState as HttpWebRequest;
                HttpWebResponse response = request.EndGetResponse(result) as HttpWebResponse;
                List<NewsItem> NewsItemsNew = new List<NewsItem>();


                using (Stream stream = response.GetResponseStream())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Rss));
                    Rss rss = (Rss)serializer.Deserialize(stream);

                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        for (int i = 0; i < 16; i++)
                        {

                            NewsItem n = new NewsItem();

                            n.pubDate = rss.Channel.NewsItems[i].pubDate;
                            n.Description = "";
                            desc.Add(rss.Channel.NewsItems[i].Description);
                            n.Title = rss.Channel.NewsItems[i].Title;
                            n.setDATE();
                            n.Prikaz = "";


                            string d = desc[i];
                            desc.RemoveAt(i);
                            List<string> des_link = ocistiDescription(ref d);
                            desc.Add((des_link[0]));
                            n.link = "";
                            n.Vidljivost = Visibility.Collapsed;
                            if (des_link.Count > 1)
                            {
                            // Aleksa TODO: re-use the logic for "raspored casova"
                            // n.link = "download/RT_2016_17_p.pdf";
                            n.link = des_link[1];
                                n.Prikaz = "Прикажи ПДФ";
                            }


                            lista.Items.Add(n);
                        }
                    //rss.Channel.NewsItems = NewsItemsNew;
                    //this.ViewModel.CurrentRss = rss;


                });
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        private List<string> ocistiDescription(ref string desc)
        {
            string link = "";
            List<string> des_link = new List<string>();
            string des = desc;
            if (des.IndexOf("<p>") >= 0)
                des = des.Replace("<p>", " ");
            if (des.IndexOf("<br>") >= 0)
                des = des.Replace("<br>", Environment.NewLine);
            if (des.IndexOf("</p>") >= 0)
                des = des.Replace("</p>", Environment.NewLine);
            if (des.IndexOf("&nbsp;") >= 0)
                des = des.Replace("&nbsp;", Environment.NewLine);

            if (des.IndexOf("<a href='do") >= 0)
            {
                string linija = des.Substring(des.IndexOf("<a href='do"));
                string[] nizLinije = linija.Split('\'');
                link = nizLinije[1];
                des = des.Substring(0, des.IndexOf("<a href='do"));

            }

            // Aleksa: method call added
            des = GetPlainTextFromHtml(des);

            des_link.Add(des);
            if (link != "")
                des_link.Add(link);
            return des_link;
        }

        // Aleksa: added method for parsing
        private string GetPlainTextFromHtml(string htmlString)
        {
            string htmlTagPattern = "<.*?>";
            var regexCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            htmlString = regexCss.Replace(htmlString, string.Empty);
            htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
            // htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            htmlString = htmlString.Replace("&nbsp;", string.Empty);

            return htmlString;
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(Studenti), 1);
        }

        private void TextBlock_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Smerovi));
        }
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
      
        //public async void ReadTextFile()
        //{
        //    var folder = Package.Current.InstalledLocation;
        //    var file = await folder.GetFileAsync(@"Assets\StudentskeVesti1.xml");
        //    var read = await FileIO.ReadTextAsync(file);
        //    string xml = read;
        //    studentiIspis(xml);
        //}
        //public void studentiIspis(string xml)
        //{
        //    MyXmlReader ride = new MyXmlReader(@"unknown");
        //    StudentskeVesti vest = ride.action(1, xml);
        //    int maxId = ride.getMaxId(xml);
            
        //        for(int j = 0; j<3; j=j+2)
        //        {
        //            for(int k =0; k<3;k=k+2)
        //            {
        //                Rectangle rek = new Rectangle();
        //                rek.Width = 200;
        //                rek.Height = 200;
        //                Grid.SetColumn(rek, j);
        //                Grid.SetRow(rek, k);
        //                StudentskeVesti vest1 = ride.action(maxId, xml);

        //                maxId--;
        //                Uri uri = new Uri("ms-appx://" + vest1.MalaSlika);
        //                BitmapImage bitmap = new BitmapImage();
        //                bitmap.UriSource = uri;
        //                ImageBrush imageBrush = new ImageBrush();
        //                imageBrush.ImageSource = bitmap;
        //                rek.Fill = imageBrush;
        //                rek.Tag = vest1.Id;
        //                rek.Tapped += rek_Tapped;
        //                rek.Stroke = new SolidColorBrush(Color.FromArgb(255, 68, 68, 68));
        //                rek.StrokeThickness = 4;
        //                GridStudenti.Children.Add(rek);
        //            }
        //        }
            
        //}

        void rek_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Rectangle rek = sender as Rectangle;
            int id = Convert.ToInt32(rek.Tag);
            this.Frame.Navigate(typeof(Studenti), id);
        }

        private void Rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Timovi));
        }

        private void TextBlock_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Profesori));           
        }
        private void Rectangle_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Profesor));
        }

        private void Rectangle_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            Rectangle r = sender as Rectangle;
            string id = (string)r.Tag;
            this.Frame.Navigate(typeof(Profesor), id);
        }

        private void napuniSmerovi()
        {
            string upit = "SELECT * FROM smerovi";
            Baza baza = new Baza();
            MySqlDataReader reader;
            List<string> lstSkracenica = new List<string>();
            List<string> lstIdSmerovi = new List<string>();
            List<string> lstBoje = new List<string>();
            using (reader = baza.izvrsiUpit(upit))
            {
                while (reader.Read())
                {
                    lstSkracenica.Add(reader.GetString("skracenica"));
                    lstIdSmerovi.Add(reader.GetString("id_smer"));
                    lstBoje.Add(reader.GetString("boja"));
                }
            }

            int brojac = 0;
            for (int i = 0; i < 7; i = i + 2)
            {
                for (int j = 0; j < 7; j = j + 2)
                {
                    TextBlock tb = new TextBlock();
                    tb.VerticalAlignment = VerticalAlignment.Center;
                    tb.HorizontalAlignment = HorizontalAlignment.Center;
                    tb.FontSize = 30;
                    tb.Tag = lstIdSmerovi[brojac];
                    tb.Tapped += tb_Tapped;
                    tb.Text = lstSkracenica[brojac];
                    Grid.SetColumn(tb, i);
                    Grid.SetRow(tb, j);

                    string rgb = lstBoje[brojac];
                    byte r, g, b;

                    r = System.Convert.ToByte(rgb.Substring(1, 2), 16);
                    g = System.Convert.ToByte(rgb.Substring(3, 2), 16);
                    b = System.Convert.ToByte(rgb.Substring(5, 2), 16);

                    Border pozadina = new Border();
                    pozadina.Height = 160;
                    pozadina.Width = 160;
                    pozadina.Tag = lstIdSmerovi[brojac++];
                    /*pozadina.Background = new SolidColorBrush(Color.FromArgb(255, 65, 105, 225));
                    if (brojac > 8) pozadina.Background = new SolidColorBrush(Color.FromArgb(255, 106, 90, 205));*/
                    pozadina.Background = new SolidColorBrush(Color.FromArgb(255, r, g, b));
                    if (brojac > 8) pozadina.Background = new SolidColorBrush(Color.FromArgb(255, r, g, b));
                    pozadina.Tapped += pozadina_Tapped;
                    Grid.SetColumn(pozadina, i);
                    Grid.SetRow(pozadina, j);

                    gridSmerovi.Children.Add(pozadina);
                    gridSmerovi.Children.Add(tb);

                    if (brojac == 14) return;
                }
            }
        }

        void tb_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            string id = (string)tb.Tag;
            this.Frame.Navigate(typeof(Timovi), id);
        }

        void pozadina_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Border brd = sender as Border;
            string id = (string)brd.Tag;
            this.Frame.Navigate(typeof(Timovi), id);
        }
          /****** PROFESORI ******/
        static int SEK = 5;
        DispatcherTimer dispatch;
        int sekunde = SEK;

        public void initDispatcher()
        {
            dispatch = new DispatcherTimer();
            dispatch.Tick += dispatch_Tick;
            dispatch.Interval = new TimeSpan(0, 0, 3);
            dispatch.Start();
        }

        void dispatch_Tick(object sender, object e)
        {
            zamenaProfesora();
        }
        
        List<ProfesorC> prof = new List<ProfesorC>();
        void napraviListuProfesora()
        {
            Baza baza = new Baza();
            MySqlDataReader reader;
            string upit = "SELECT idProfesor, imePrezime, titula, zvanje, slika, konsultacije, kabinet, mail, biografija, aktivnost FROM profesori WHERE slika != '/adresar/muskarac.jpg' AND slika != '/adresar/zena.jpg' ORDER BY imePrezime";

            using (reader = baza.izvrsiUpit(upit))
            {
                while (reader.Read())
                {
                    prof.Add(new ProfesorC(reader.GetString("idProfesor"), reader.GetString("imePrezime"), reader.GetString("titula"), reader.GetString("zvanje"), reader.GetString("slika"), reader.GetString("konsultacije"), reader.GetString("kabinet"), reader.GetString("mail"), reader.GetString("biografija"), reader.GetString("aktivnost")));
                }
            }
        }
        
        void randomListaProfesora()
        {
            List<ProfesorC> profPom = new List<ProfesorC>();
            Random random = new Random(DateTime.Now.Millisecond);
            int lenght = prof.Count;
            for (int x = 0; x < lenght; x++)
            {
                int randomBroj = random.Next(0, prof.Count);
                profPom.Add(prof[randomBroj]);

                prof.RemoveAt(randomBroj);
            }

            prof = profPom;
        }

        int rand(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        int pozI, pozJ, ind;
        public void zamenaMesta(int indexA, int indexB)
        {
            ProfesorC tmp = prof[indexA];
            prof[indexA] = prof[indexB];
            prof[indexB] = tmp;
            ind = indexA;
            switch (indexA)
            {
                case 0: pozI = 0; pozJ = 0; break;
                case 1: pozI = 0; pozJ = 2; break;
                case 2: pozI = 0; pozJ = 4; break;
                case 3: pozI = 0; pozJ = 6; break;
                case 4: pozI = 2; pozJ = 0; break;
                case 5: pozI = 2; pozJ = 2; break;
                case 6: pozI = 2; pozJ = 4; break;
                case 7: pozI = 2; pozJ = 6; break;
            }
        }

        void profesori()
        {
            napraviListuProfesora();
            randomListaProfesora();
            
            for (int index = 0, i = 0; i < 3; i = i + 2)
            {
                for (int j = 0; j < 7; j = j + 2)
                {
                    if (index == 8)
                    {
                        return;
                    }
                    napraviGrid(i, j, prof[index++], 1);
                }
            }
        }

        void zamenaProfesora()
        {
            zamenaMesta(rand(0, 7), rand(8, prof.Count));
            napraviGrid(pozI, pozJ, prof[ind], 0);
        }

        void napraviGrid(int i, int j, ProfesorC p, double op)
        {
            Rectangle r1 = new Rectangle();
            ImageBrush imageBrush = new ImageBrush();
            string putanja = "ms-appx://" + p.slika;
            imageBrush.ImageSource = new BitmapImage(new Uri(putanja, UriKind.RelativeOrAbsolute));
            r1.Fill = imageBrush;
            Grid.SetColumn(r1, i);
            Grid.SetRow(r1, j);
            r1.Tag = p.idProfesor;
            r1.Opacity = op / 2;
             Rectangle r2 = new Rectangle();
            r2.Fill = new SolidColorBrush(Colors.Black);
            r2.Height = 30;
            r2.Opacity = (op / 2); // to je 0.5
            r2.VerticalAlignment = VerticalAlignment.Bottom;
            Grid.SetColumn(r2, i);
            Grid.SetRow(r2, j);

            TextBlock tb = new TextBlock();
            tb.Margin = new Thickness(10, 135, 0, 0);
            tb.FontSize = 15;
            Grid.SetColumn(tb, i);
            Grid.SetRow(tb, j);
            tb.Text = p.imePrezime;
            tb.Opacity = op;

            Rectangle r = new Rectangle();
            r.Fill = new SolidColorBrush(Colors.Black);
            r.Opacity = 0;
            r.Width = 160;
            r.Height = 160;
            Grid.SetColumn(r, i);
            Grid.SetRow(r, j);
            r.Tag = p.idProfesor;
            r.Tapped += Rectangle_Tapped_2;

            gridProfesori.Children.Add(r1);
            gridProfesori.Children.Add(r2);
            gridProfesori.Children.Add(tb);
            gridProfesori.Children.Add(r);
            DoubleAnimation anim1 = new DoubleAnimation();

            Duration duration = new Duration(TimeSpan.FromSeconds(0.5));
            anim1.Duration = duration;
            Storyboard sb = new Storyboard();
            sb.Duration = duration;

            sb.Children.Add(anim1);

            Storyboard.SetTarget(anim1, r1);

            Storyboard.SetTargetProperty(anim1, "Opacity");

            anim1.EnableDependentAnimation = true;

            anim1.To = 1;


            DoubleAnimation anim2 = new DoubleAnimation();

            anim2.Duration = duration;
            sb.Duration = duration;

            sb.Children.Add(anim2);

            Storyboard.SetTarget(anim2, r2);

            Storyboard.SetTargetProperty(anim2, "Opacity");

            anim2.EnableDependentAnimation = true;

            anim2.To = 0.5;


            DoubleAnimation anim3 = new DoubleAnimation();

            anim3.Duration = duration;
            sb.Duration = duration;

            sb.Children.Add(anim3);

            Storyboard.SetTarget(anim3, tb);

            Storyboard.SetTargetProperty(anim3, "Opacity");

            anim3.EnableDependentAnimation = true;

            anim3.To = 1;

            sb.Begin();
        }
        /***** TIMOVI *****/

        List<Tim> timoviLst = new List<Tim>();
        public void timovi()
        {
            napraviListuTimova();
            napraviTimovi();
        }

        void napraviListuTimova()
        {
            Baza baza = new Baza();
            MySqlDataReader reader;
            string upit = "SELECT * FROM timovi";

            using (reader = baza.izvrsiUpit(upit))
            {
                while (reader.Read())
                {
                    timoviLst.Add(new Tim(reader.GetString("id_tim"), reader.GetString("id_preds"), reader.GetString("ime"), reader.GetString("glavna_slika"), reader.GetString("pozadinska_slika"), reader.GetString("ikonica"), reader.GetString("podnaslov"), reader.GetString("tekst")));
                }
            }
        }

        void napraviTimovi()
        {
            for (int index = 0, i = 0; i < 5; i = i + 2)
            {
                for (int j = 0; j < 7; j = j + 2)
                {
                    if (index == timoviLst.Count)
                    {
                        return;
                    }
                    napraviGridove(i, j, timoviLst[index++]);
                }
            }
        }

        void napraviGridove(int i, int j, Tim p)
        {
            Rectangle r3 = new Rectangle();
            r3.Fill = new SolidColorBrush(Colors.RoyalBlue);
            Grid.SetColumn(r3, i);
            Grid.SetRow(r3, j);

            Rectangle r1 = new Rectangle();
            r1.Margin = new Thickness(0, 0, 0, 30);
            r1.Width = 80;
            r1.Height = 80;
            ImageBrush imageBrush = new ImageBrush();
            string putanja = "ms-appx:///Assets/" + p.ikonica;
            imageBrush.ImageSource = new BitmapImage(new Uri(putanja, UriKind.RelativeOrAbsolute));
            r1.Fill = imageBrush;
            Grid.SetColumn(r1, i);
            Grid.SetRow(r1, j);
            r1.Tag = p.id_tim;

            Rectangle r2 = new Rectangle();
            r2.Fill = new SolidColorBrush(Colors.Black);
            r2.Height = 30;
            r2.Opacity = 0.5;
            r2.VerticalAlignment = VerticalAlignment.Bottom;
            Grid.SetColumn(r2, i);
            Grid.SetRow(r2, j);

            TextBlock tb = new TextBlock();
            tb.Margin = new Thickness(10, 135, 0, 0);
            tb.FontSize = 15;
            Grid.SetColumn(tb, i);
            Grid.SetRow(tb, j);
            tb.Text = p.ime;

            Rectangle r = new Rectangle();
            r.Fill = new SolidColorBrush(Colors.Black);
            r.Opacity = 0;
            r.Width = 160;
            r.Height = 160;
            Grid.SetColumn(r, i);
            Grid.SetRow(r, j);
            r.Tag = p.id_tim;
            r.Tapped += r_Tapped;

            gridTimovi.Children.Add(r3);
            gridTimovi.Children.Add(r1);
            gridTimovi.Children.Add(r2);
            gridTimovi.Children.Add(tb);
            gridTimovi.Children.Add(r);
        }
        void r_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string s = (string)(sender as Rectangle).Tag;
            this.Frame.Navigate(typeof(Smerovi), s);
        }

        public string link = "";
        private void lista_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (((ListView)sender).SelectedIndex == -1)
            {
                return;
            }

            NewsItem vest = (NewsItem)((ListView)sender).SelectedItem;

            if (preth == null)
            {
                if (vest.link != "")
                {
                    vest.Vidljivost = Visibility.Visible;
                    // Aleksa: fix for not loaded chosen pdf
                    // downloadPdf(vest.link);
                    link = vest.link;
                }

                preth = vest;
                vest.Description = desc[(index = ((ListView)sender).SelectedIndex)];
                flag = false;
                lista.Items[((ListView)sender).SelectedIndex] = vest;

            }
            else if (preth == vest)
            {
                if (flag == false)
                {
                    preth.Description = "";
                    preth.Vidljivost = Visibility.Collapsed;
                    lista.Items[((ListView)sender).SelectedIndex] = preth;
                    flag = true;
                }
                else
                {
                    vest.Description = desc[(index = ((ListView)sender).SelectedIndex)];
                    // Aleksa: fix for defect when button is added for downloading PDF but not needed
                    if (vest.link != "")
                    {
                        vest.Vidljivost = Visibility.Visible;
                        // downloadPdf(vest.link);
                        link = vest.link;
                    }
                    lista.Items[((ListView)sender).SelectedIndex] = vest;
                    flag = false;

                }
            }
            if (preth != vest)
            {

                preth.Description = "";
                preth.Vidljivost = Visibility.Collapsed;
                if (vest.link != "")
                {
                    vest.Vidljivost = Visibility.Visible;
                    // downloadPdf(vest.link);
                    link = vest.link;
                }
                vest.Description = desc[((ListView)sender).SelectedIndex];
                lista.Items[index] = preth;
                index = ((ListView)sender).SelectedIndex;
                lista.Items[index] = vest;
                flag = false;
                preth = vest;

            }
        }

        private void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer interval = new DispatcherTimer();
            interval.Interval = new TimeSpan(0, 0, 15);
            interval.Start();
            interval.Tick += interval_Tick;
        }

        void interval_Tick(object sender, object e)
        {
            nestajanje.Begin();
            DispatcherTimer interval2 = new DispatcherTimer();
            interval2.Interval = new TimeSpan(0, 0, 0,0,700);
            interval2.Start();
            interval2.Tick += interval2_Tick;
        }

        List<string> pozadniskeSlike = new List<string>();
        int indexSl = 0;

        void ucitajPozadinskeSlike()
        {
            Baza baza = new Baza();
            MySqlDataReader reader;
            string upit = "SELECT * FROM smerovi LIMIT 7";

            using (reader = baza.izvrsiUpit(upit))
            {
                while (reader.Read())
                {
                    pozadniskeSlike.Add(reader.GetString("slika"));
                }
            }
        }

        void interval2_Tick(object sender, object e)
        {
            pozadinskeSlike.ImageSource = new BitmapImage(new Uri("ms-appx:///" + pozadniskeSlike[indexSl++]));
            if (indexSl == pozadniskeSlike.Count)
            {
                indexSl = 0;
            }
            postajanje.Begin();
            DispatcherTimer interval2 = sender as DispatcherTimer;
            interval2.Stop();
            DispatcherTimer interval3 = new DispatcherTimer();
            interval3.Interval = new TimeSpan(0, 0, 0, 0, 900);
            interval3.Start();
            interval3.Tick += interval3_Tick;
        }

        void interval3_Tick(object sender, object e)
        {
            
        }

        private void TextBlock_Tapped_3(object sender, TappedRoutedEventArgs e)
        {
            // Aleksa: passed argument link for consistency
            this.Frame.Navigate(typeof(PdfPage), link);
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (this.itemsViewSource.View != null)
            {
                var selectedItem = (designIdea002.Data.SampleDataItem)this.itemsViewSource.View.CurrentItem;
                if (selectedItem != null) e.PageState["SelectedItem"] = selectedItem.UniqueId;
            }
        }

        /****** RASPORED ******/
        private void napuniRaspored()
        {
            // Aleksa TODO: implementation
            // throw new NotImplementedException();


            // Aleksa TODO: hard coded
            List<string> skraceno = new List<string>();
            List<string> linkovi = new List<string>();
            List<string> smerovi = new List<string>();
            List<string> semestar = new List<string>();

            #region hard coded
            skraceno.Add("АСУВ");
            linkovi.Add("download/ASUV_2016_17_p.pdf");
            smerovi.Add("Аутоматика и системи управаљања возилима");
            semestar.Add("1. семестар");

            skraceno.Add("АВТ");
            linkovi.Add("download/AVT1p.pdf");
            smerovi.Add("Аудио и видео технологије");
            semestar.Add("1. семестар");

            skraceno.Add("ЕЛИТЕ");
            linkovi.Add("download/ELITE_2016_17_p.pdf");
            smerovi.Add("Електроника и телекомуникације");
            semestar.Add("1. семестар");

            skraceno.Add("ЕПО");
            linkovi.Add("download/EPO1p.pdf");
            smerovi.Add("Електронско пословање");
            semestar.Add("1. семестар");

            skraceno.Add("НЕТ");
            linkovi.Add("download/NET_2016_17_p.pdf");
            smerovi.Add("Нове енергетске технологије");
            semestar.Add("1. семестар");

            skraceno.Add("НРТ");
            linkovi.Add("download/NRT1p.pdf");
            smerovi.Add("Нове рачунарске технологије");
            semestar.Add("1. семестар");

            skraceno.Add("РТ");
            linkovi.Add("download/RT_2016_17_p.pdf");
            smerovi.Add("Рачунарска техника");
            semestar.Add("1. семестар");

            /***********************/

            skraceno.Add("АСУВ");
            linkovi.Add("download/ASUV3p_2016_17_p.pdf");
            smerovi.Add("Аутоматика и системи управаљања возилима");
            semestar.Add("3. семестар");

            skraceno.Add("АВТ");
            linkovi.Add("download/AVT3p_2016_17_p.pdf");
            smerovi.Add("Аудио и видео технологије");
            semestar.Add("3. семестар");
            
            skraceno.Add("ЕЛИТЕ");
            linkovi.Add("download/ELITE3p_2016_17_p.pdf");
            smerovi.Add("Електроника и телекомуникације");
            semestar.Add("3. семестар");

            skraceno.Add("ЕПО");
            linkovi.Add("download/EPO3p_2016_17_p.pdf");
            smerovi.Add("Електронско пословање");
            semestar.Add("3. семестар");

            skraceno.Add("НЕТ");
            linkovi.Add("download/NET3p_2016_17_p.pdf");
            smerovi.Add("Нове енергетске технологије");
            semestar.Add("3. семестар");

            skraceno.Add("НРТ");
            linkovi.Add("download/NRT3p_2016_17_p.pdf");
            smerovi.Add("Нове рачунарске технологије");
            semestar.Add("3. семестар");

            skraceno.Add("РТ");
            linkovi.Add("download/RT3p_2016_17_p.pdf");
            smerovi.Add("Рачунарска техника");
            semestar.Add("3. семестар");

            /***********************/

            skraceno.Add("АСУВ");
            linkovi.Add("download/ASUV5p_2016_17_p.pdf");
            smerovi.Add("Аутоматика и системи управаљања возилима");
            semestar.Add("5. семестар");

            skraceno.Add("АВТ");
            linkovi.Add("download/AVT5p_2016_17_p.pdf");
            smerovi.Add("Аудио и видео технологије");
            semestar.Add("5. семестар");

            skraceno.Add("ЕЛИТЕ");
            linkovi.Add("download/ELITE5p_2016_17_p.pdf");
            smerovi.Add("Електроника и телекомуникације");
            semestar.Add("5. семестар");

            skraceno.Add("ЕПО");
            linkovi.Add("download/EPO5p_2016_17_p.pdf");
            smerovi.Add("Електронско пословање");
            semestar.Add("5. семестар");

            skraceno.Add("НЕТ");
            linkovi.Add("download/NET5p_2016_17_p.pdf");
            smerovi.Add("Нове енергетске технологије");
            semestar.Add("5. семестар");

            skraceno.Add("НРТ");
            linkovi.Add("download/NRT5p_2016_17_p.pdf");
            smerovi.Add("Нове рачунарске технологије");
            semestar.Add("5. семестар");

            skraceno.Add("РТ");
            linkovi.Add("download/RT5p_2016_17_p.pdf");
            smerovi.Add("Рачунарска техника");
            semestar.Add("5. семестар");

            /***********************/

            skraceno.Add("ССЕЛИТЕ");
            linkovi.Add("download/Raspored_zimski_%20ELITE_SS_%202016_17.pdf");
            smerovi.Add("СС Електроника и телекомуникације");
            semestar.Add("7. семестар");

            skraceno.Add("МЕХА");
            linkovi.Add("download/Raspored_zimski_MEHA__2016_17.pdf");
            smerovi.Add("СС Мехатроника");
            semestar.Add("7. семестар");

            skraceno.Add("МТДТВ");
            linkovi.Add("download/Raspored_zimski_%20MDTV_SS_%202016_17.pdf");
            smerovi.Add("СС Мултимедијалне технологије и дигитална телевизија");
            semestar.Add("7. семестар");

            skraceno.Add("ССНЕТ");
            linkovi.Add("download/Raspored_zimski_NET_SS_2016_17.pdf");
            smerovi.Add("СС Нове енергетске технологије");
            semestar.Add("7. семестар");

            skraceno.Add("ССНРТ");
            linkovi.Add("download/Raspored_zimski_%20NRT_SS_%202016_17.pdf");
            smerovi.Add("СС Нове рачунарске технологије");
            semestar.Add("7. семестар");

            skraceno.Add("СИКС");
            linkovi.Add("download/Raspored_zimski_SIKS_SS_%202016_17.pdf");
            smerovi.Add("СС Сигурност у информационо комуникационим системима");
            semestar.Add("7. семестар");
            #endregion

            int i = 0;
            foreach (string link in linkovi)
            {
                RasporedItem r = new RasporedItem(semestar[i].Substring(0, 2), skraceno[i], smerovi[i], semestar[i], linkovi[i]);
                listaRaspored.Items.Add(r);
                i++;
            }
        }

        private void listaRaspored_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Aleksa TODO: implementation
            // throw new NotImplementedException();

            RasporedItem r = (RasporedItem)((ListView)sender).SelectedItem;
            
            this.Frame.Navigate(typeof(PdfPage), r.Link);
        }

    }
}
