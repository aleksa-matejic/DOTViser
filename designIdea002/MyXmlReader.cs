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


namespace designIdea002
{
    class MyXmlReader
    {
        string fileName;
        static string teksti;
        

        public MyXmlReader(string fileName)
        {
            this.fileName = fileName;
            teksti = "";
        }
        public string getTeksti()
        {
            return teksti;
        }
        public void openFile()
        {
        }
        //public void closeFile();
        public async void getText()
        {
            StorageFolder folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = await folder.GetFileAsync(@fileName);
            Stream stream = await file.OpenStreamForReadAsync();
            StreamReader sReader = new StreamReader(stream);
            teksti = sReader.ReadToEnd();
            
            
            
            
            
            //PROBLEM KOD PONOVNOG OTVARANJA FAJLA! NE VIDI TEKST!
        }

        public int getMaxId(string t="")
        {
            if (t != "") teksti = t;
            int id;
            using (XmlReader reader = XmlReader.Create(new StringReader(teksti)))
            {

                reader.ReadToFollowing("vest");
                reader.MoveToFirstAttribute();
                id = Convert.ToInt32(reader.Value);
            }
            return id;
        }
        public StudentskeVesti action(int idPageSent, string xxml = "")
        {
            string xml = "";
            if (xxml != "") xml = @xxml + "";
            else xml = @teksti + "";
            
            string naslov = "", opis = "", velikaSlika = "", malaSlika = "", tekst = "";
            int id;
            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
            {
                
                reader.ReadToFollowing("vest");
                reader.MoveToFirstAttribute();
                id = Convert.ToInt32(reader.Value);
                int brojac = id;
                for (int i = 0; i < brojac; i++)
                {
                    if (id == idPageSent)
                    {
                        reader.ReadToFollowing("naslov");
                        naslov = reader.ReadElementContentAsString();

                        reader.ReadToFollowing("opis");
                        opis = reader.ReadElementContentAsString();

                        reader.ReadToFollowing("tekst");
                        tekst = reader.ReadElementContentAsString();

                        reader.ReadToFollowing("mSlika");
                        malaSlika = reader.ReadElementContentAsString();

                        reader.ReadToFollowing("vSlika");
                        velikaSlika = reader.ReadElementContentAsString();
                        break;
                    }

                    reader.ReadToFollowing("vest");
                    reader.MoveToFirstAttribute();
                    id = Convert.ToInt32(reader.Value);
                }
            }
            StudentskeVesti vest1 = new StudentskeVesti(naslov, tekst, velikaSlika, malaSlika, opis, id);
            return vest1;
        }
        //public void writeNews();
        //public void writeNewsList();
    };

}


