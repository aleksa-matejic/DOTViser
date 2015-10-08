using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace designIdea002
{
    class StudentskeVesti
    {
        //string naslov, tekst, velikaSlika, malaSlika, opis;

        public StudentskeVesti (string n, string t, string vs, string ms, string o,  int i)
        {
            Naslov = n;
            Tekst = t;
            VelikaSlika = vs;
            MalaSlika = ms;
            Opis = o;
            Id = i;
        }
        public StudentskeVesti()
        {
            Naslov = "";
            Tekst = "";
            VelikaSlika = "";
            MalaSlika = "";
            Opis = "";
            Id = -1;
        }

        public string Naslov { get; set; }
        public int Id { get; set; }
        public string Tekst { get; private set; }
        public string VelikaSlika { get; private set; }
        public string MalaSlika { get; private set; }
        public string Opis { get; private set; }

        public StudentskeVesti vrati(Grid ArtikliLevo, Grid ArtikliDesno, MyXmlReader ride, string xxml = "")
        {
            string xml = "";
            if (xxml == "") ride.getText();
            else xml = @xxml + "";
            
             
            


            //for (int i = 0; i < 4; i++)
            //{
            //    StudentskeVesti podaci = new StudentskeVesti();
            //    if (xml == "") { podaci = ride.action(i + 1); }
            //    else { podaci = ride.action(i + 1, xml); }


            //    Rectangle testRek = new Rectangle();
            //    testRek.Fill = new SolidColorBrush(Color.FromArgb(255, 68, 68, 68));
            //    testRek.Width = 250;

            //    StackPanel testSP = new StackPanel();
            //    testSP.Tapped += testSP_Tapped;
            //    Grid.SetRow(testSP, 1);

            //    Rectangle testRek1 = new Rectangle();
            //    testRek1.Width = 230;
            //    testRek1.Height = 150;
            //    testRek1.VerticalAlignment = VerticalAlignment.Top;

            //    TextBlock testTB = new TextBlock();
            //    testTB.Width = 230;
            //    testTB.FontSize = 30;
            //    testTB.TextWrapping = TextWrapping.Wrap;
            //    testTB.FontWeight = FontWeights.Light;
            //    testTB.Margin = new Thickness(0, 20, 0, 30);
            //    testTB.Text = podaci.Naslov;

            //    TextBlock testTB1 = new TextBlock();
            //    testTB1.Width = 230;
            //    testTB1.FontSize = 18;
            //    testTB1.TextWrapping = TextWrapping.Wrap;
            //    testTB1.FontWeight = FontWeights.Light;
            //    testTB1.Text = podaci.Opis;

            //    switch (i)
            //    {
            //        case 0:
            //            {
            //                //testRek.Height = 400;
            //                //Grid.SetRow(testRek, 0);
            //                //Grid.SetRowSpan(testRek, 2);
            //                //ArtikliLevo.Children.Add(testRek);

            //                //Uri uri = new Uri("ms-appx://" + podaci.MalaSlika);
            //                //BitmapImage s = new BitmapImage();
            //                //s.UriSource = uri;
            //                //ImageBrush ss = new ImageBrush();
            //                //ss.ImageSource = s;
            //                //testRek1.Fill = ss;

            //                //Grid.SetRow(testSP, 1);
            //                //testSP.Children.Add(testRek1);
            //                //testSP.Children.Add(testTB);
            //                //testSP.Children.Add(testTB1);
            //                //ArtikliLevo.Children.Add(testSP);

            //                break;
            //            }
            //        case 1:
            //            {
            //                testRek.Height = 360;
            //                Grid.SetRow(testRek, 3);
            //                Grid.SetRowSpan(testRek, 3);
            //                ArtikliLevo.Children.Add(testRek);

            //                Uri uri = new Uri("ms-appx://" + podaci.MalaSlika);
            //                BitmapImage s = new BitmapImage();
            //                s.UriSource = uri;
            //                ImageBrush ss = new ImageBrush();
            //                ss.ImageSource = s;
            //                testRek1.Fill = ss;

            //                Grid.SetRow(testSP, 4);
            //                testSP.Children.Add(testRek1);
            //                testSP.Children.Add(testTB);
            //                testSP.Children.Add(testTB1);
            //                ArtikliLevo.Children.Add(testSP);
            //                break;
            //            }
            //        case 2:
            //            {
            //                testRek.Height = 310;
            //                Grid.SetRow(testRek, 0);
            //                Grid.SetRowSpan(testRek, 2);
            //                ArtikliDesno.Children.Add(testRek);

            //                Uri uri = new Uri("ms-appx://" + podaci.MalaSlika);
            //                BitmapImage s = new BitmapImage();
            //                s.UriSource = uri;
            //                ImageBrush ss = new ImageBrush();
            //                ss.ImageSource = s;
            //                testRek1.Fill = ss;

            //                Grid.SetRow(testSP, 1);
            //                testSP.Children.Add(testRek1);
            //                testSP.Children.Add(testTB);
            //                testSP.Children.Add(testTB1);
            //                ArtikliDesno.Children.Add(testSP);

            //                break;
            //            }
            //        case 3:
            //            {
            //                testRek.Height = 450;
            //                Grid.SetRow(testRek, 3);
            //                Grid.SetRowSpan(testRek, 3);
            //                ArtikliDesno.Children.Add(testRek);

            //                Uri uri = new Uri("ms-appx://" + podaci.MalaSlika);
            //                BitmapImage s = new BitmapImage();
            //                s.UriSource = uri;
            //                ImageBrush ss = new ImageBrush();
            //                ss.ImageSource = s;
            //                testRek1.Fill = ss;

            //                Grid.SetRow(testSP, 4);
            //                testSP.Children.Add(testRek1);
            //                testSP.Children.Add(testTB);
            //                testSP.Children.Add(testTB1);
            //                ArtikliDesno.Children.Add(testSP);
            //                break;
            //            }
            //    }





            //}














            //Artikli.Children.Add(testSP);
            return this;
        }

        void testSP_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }
        
     
    }
}
