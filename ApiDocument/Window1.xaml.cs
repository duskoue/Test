using Microsoft.Reporting.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace ApiDocument
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        MainWindow mainW;
        CApiDoc API = new CApiDoc();
        CPomocniParametri ApiParametri = new CPomocniParametri();
        public bool snimipdf = true;
        public Window1()
        {
            InitializeComponent();

            prikaz();
            CPositionHelper.SetSize(this);
            mainW = System.Windows.Application.Current.MainWindow as MainWindow;
            string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string foldername = System.IO.Path.Combine(ProjectPath, "Konfiguracija");
            string putanjadofajla = Convert.ToString(System.IO.Path.Combine(foldername, "Default.ini"));
            CIniFile Ini = new CIniFile(putanjadofajla);
            ApiParametri.PutanjadoInifajla = Ini.Read("PutanjaDoIniFajla", "ApiDocument");


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnPDF.Visibility = Visibility.Hidden;

            CIzvestaj.listaparametaraizv.ToList();
            CIzvestaj.l1.ToList();
            CIzvestaj.listaSpeculparam.ToList();
            CIzvestaj.listaTabulparamizv.ToList();
            CIzvestaj.listaGreskeIzv.ToList();
            CIzvestaj.Listaizlaznevr.ToList();
            CIzvestaj.SPltizlazvr.ToList();
            CIzvestaj.listaTabizlparamizvestaj.ToList();

            Izvestaj.LocalReport.DataSources.Clear();
            Izvestaj.LocalReport.ReportEmbeddedResource = "ApiDocument.Report1.rdlc";
            //string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
            string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string putanja = System.IO.Path.Combine(path, "Report1.rdlc");

       
            Izvestaj.ShowExportButton = true;

            Izvestaj.LocalReport.ReportPath = putanja;
            Izvestaj.AutoSize = false;


            Izvestaj.ZoomMode = ZoomMode.PageWidth;

            Microsoft.Reporting.WinForms.ReportDataSource datasetCApiDoc = new Microsoft.Reporting.WinForms.ReportDataSource("CApiOpisDS", CIzvestaj.l1);
            Microsoft.Reporting.WinForms.ReportDataSource dataset1 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetUP", CIzvestaj.listaparametaraizv);
            Microsoft.Reporting.WinForms.ReportDataSource dataset2 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetSPULP", CIzvestaj.listaSpeculparam);
            Microsoft.Reporting.WinForms.ReportDataSource dataset3 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetTABUP", CIzvestaj.listaTabulparamizv);
            Microsoft.Reporting.WinForms.ReportDataSource dataset4 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetGR", CIzvestaj.listaGreskeIzv);
            Microsoft.Reporting.WinForms.ReportDataSource dataset5 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetTIZLVR", CIzvestaj.Listaizlaznevr);
            Microsoft.Reporting.WinForms.ReportDataSource dataset6 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", CIzvestaj.SPltizlazvr);
            Microsoft.Reporting.WinForms.ReportDataSource dataset7 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetDodatak", CIzvestaj.listaDodatakizv);
            Microsoft.Reporting.WinForms.ReportDataSource dataset8 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSettabizlazniparam", CIzvestaj.listaDodatakizv);



            Izvestaj.LocalReport.DataSources.Add(datasetCApiDoc);
            Izvestaj.LocalReport.DataSources.Add(dataset1);
            Izvestaj.LocalReport.DataSources.Add(dataset2);
            Izvestaj.LocalReport.DataSources.Add(dataset3);
            Izvestaj.LocalReport.DataSources.Add(dataset4);
            Izvestaj.LocalReport.DataSources.Add(dataset5);
            Izvestaj.LocalReport.DataSources.Add(dataset6);
            Izvestaj.LocalReport.DataSources.Add(dataset7);
            Izvestaj.LocalReport.DataSources.Add(dataset8);



            datasetCApiDoc.Value = CIzvestaj.l1;
            dataset1.Value = CIzvestaj.listaparametaraizv;
            dataset2.Value = CIzvestaj.listaSpeculparam;
            dataset3.Value = CIzvestaj.listaTabulparamizv;
            dataset4.Value = CIzvestaj.listaGreskeIzv;
            dataset5.Value = CIzvestaj.Listaizlaznevr;
            dataset6.Value = CIzvestaj.SPltizlazvr;
            dataset7.Value = CIzvestaj.listaDodatakizv;
            dataset8.Value = CIzvestaj.listaTabizlparamizvestaj;


            Izvestaj.LocalReport.Refresh();
            Izvestaj.RefreshReport();
            if (snimipdf == false)
            {
                this.btnPDF_Click(this, null);
            }
           


        }

        public void prikaz()
        {

            FieldInfo info;

            foreach (RenderingExtension extension in Izvestaj.LocalReport.ListRenderingExtensions())
            {
                if (extension.Name != "PDF" && extension.Name != "EXCEL") // only PDF and Excel - remove the other options
                {
                    info = extension.GetType().GetField("m_isVisible", BindingFlags.Instance | BindingFlags.NonPublic);
                    info.SetValue(extension, false);
                }

                if (extension.Name == "EXCEL") // change "Excel" name on the list to "Excel 97-2003 Workbook"
                {
                    info = extension.GetType().GetField("m_localizedName", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (info != null) info.SetValue(extension, "Excel 97-2003 Workbook");
                }
            }
        }





        public string Putanja = AppDomain.CurrentDomain.BaseDirectory;



        public void CreatePDF()
        {
            try
            {
                CIniFile Ini = new CIniFile(ApiParametri.PutanjadoInifajla);

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = Izvestaj.LocalReport.Render(
                  "PDF", null, out mimeType, out encoding, out extension,
                  out streamids, out warnings);

              //  string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string projectPath = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string Dir = Ini.Read("Direktorijumzaobjavljenefunkcije", "ApiDocument");

                string folderName = System.IO.Path.Combine(projectPath, Dir.ToString());

                Ini.Write("Direktorijumzaobjavljenefunkcije", Dir.ToString(), "ApiDocument");
               
                

                

                Directory.CreateDirectory(folderName);

                string putanja = System.IO.Path.Combine(folderName, CIzvestaj.l1.ElementAt(0).NazivApiFunkcije + ".pdf");
                
                Ini.Write("PutanjacuvanjaPDF", putanja.ToString(), "ApiDocument");

                FileStream fs = new FileStream(putanja, FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
            catch (Exception ex )
            {

                System.Windows.MessageBox.Show(ex.Message.ToString());
            }
           
        }


        private void btnPDF_Click(object sender, RoutedEventArgs e)
        {
            CIniFile Ini = new CIniFile(ApiParametri.PutanjadoInifajla);

           // string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string projectPath = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string Dir = Ini.Read("Direktorijumzaobjavljenefunkcije", "ApiDocument");
            string folderName = System.IO.Path.Combine(projectPath, Dir.ToString());
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
           
            Directory.CreateDirectory(folderName);
            string putanja = System.IO.Path.Combine(folderName, CIzvestaj.l1.ElementAt(0).NazivApiFunkcije + ".pdf");
          
            if (File.Exists(putanja))
            {
               
                MessageBoxResult result = System.Windows.MessageBox.Show(
                    "Dokument pod ovim nazivom vec postoji. Da li zelite da objavite dokument ?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                   
                    if (File.Exists(putanja) == false)
                    {
                        try
                        { 
                        File.Delete(putanja);
                       // System.Windows.MessageBox.Show("Fajl je obrisan");
                        
                        CreatePDF();
                        ApiParametri.Dalijeobjavljenfajl = true;
                        DateTime creation1 = File.GetCreationTime(putanja);
                        ApiParametri.Vremekreiranjapdf = creation1.ToShortTimeString();
                     //  System.Windows.MessageBox.Show(ApiParametri.Vremekreiranjapdf.ToString());
                        File.SetLastWriteTime(putanja, DateTime.Now);
                        
                        
                            
                             Ini.KeyExists("Vremekreiranjapdf", "ApiDocument");
                           //  Ini.DeleteKey("Vremekreiranjapdf", "ApiDocument");
                            Ini.Write("Vremekreiranjapdf", ApiParametri.Vremekreiranjapdf.ToString(), "ApiDocument");
                        }
                        catch (Exception)
                        {

                            return;
                        }

                        
                    }
                    else
                    {

                      try
                        { 
                            File.Delete(putanja);
                           // System.Windows.MessageBox.Show("Fajl je obrisan");
                            CreatePDF();
                        File.SetLastWriteTime(putanja, DateTime.Now);
                     string creation1 =  File.GetLastWriteTime(putanja).ToShortTimeString();
                        //  System.Windows.MessageBox.Show(File.GetLastWriteTime(putanja).ToString());

                        
                        
                           
                            Ini.KeyExists("Vremekreiranjapdf", "ApiDocument");
                           // Ini.DeleteKey("Vremekreiranjapdf", "ApiDocument");
                            Ini.Write("Vremekreiranjapdf",creation1, "ApiDocument");
                        }
                        catch (Exception)
                        {

                            return;
                        }
                        
                        ApiParametri.Dalijeobjavljenfajl = true;
                     
                    }
                }
                else
                {
                    File.Delete(putanja);
                   
                    ApiParametri.Dalijeobjavljenfajl = false;
                    CreatePDF();
                    DateTime creation1 = File.GetCreationTime(putanja);
                    ApiParametri.Vremekreiranjapdf = creation1.ToShortTimeString();
                   
                    File.SetLastWriteTime(putanja, DateTime.Now);
                  string vrk =   File.GetLastWriteTime(putanja).ToShortTimeString();
                    try
                    {
                        
                        Ini.KeyExists("Vremekreiranjapdf", "ApiDocument");
                      //   Ini.DeleteKey("Vremekreiranjapdf", "ApiDocument");
                        Ini.Write("Vremekreiranjapdf", vrk, "ApiDocument");
                    }
                    catch (Exception)
                    {

                        return;
                    }
                    
                }
            }
            else
            {
                try
                { 
                File.Delete(putanja);
                CreatePDF();
                File.SetLastWriteTime(putanja, DateTime.Now);
                ApiParametri.Dalijeobjavljenfajl = true;
                
                DateTime creation1 = File.GetCreationTime(putanja);
                ApiParametri.Vremekreiranjapdf = creation1.ToShortTimeString();
                
                
                    
                    Ini.KeyExists("Vremekreiranjapdf", "ApiDocument");
                  //  Ini.DeleteKey("Vremekreiranjapdf", "ApiDocument");
                    Ini.Write("Vremekreiranjapdf", ApiParametri.Vremekreiranjapdf, "ApiDocument");
                }
                catch (Exception)
                {

                    return;
                }
                
               
            }
        }

       

        private void button_Click(object sender, RoutedEventArgs e)
        {
            CPositionHelper.SetSize(this);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            CPositionHelper.SaveSize(this);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CPositionHelper.SaveSize(this);
            /*  Proverava da li je vidljivo dugme objavi na formi u koliko jeste provarava 
             *  da li je kliknuto na njega ukoliko jeste dokument je objavljen i zatvara formu
             *  a ukoliko nije ponudice dialog da li zelis da objavis dokument
             */



            if (ApiParametri.Dalijeobjavljenfajl == false)
            {


                if (btnPDF.IsVisible == true)
                {

                    ApiParametri.Dalijeobjavljenfajl = false;
                }
                else
                {
                    ApiParametri.Dalijeobjavljenfajl = true;


                }
            }


            if (ApiParametri.Dalijeobjavljenfajl == false)
            {


                MessageBoxResult result = System.Windows.MessageBox.Show(
                       "Ovaj dokument nije objavljen kao zavrsen. Da li zelite da ga objavite?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    ApiParametri.Dalijeobjavljenfajl = true;
                    btnPDF_Click(this, null);
                }
            }
            else
            {
                ApiParametri.Dalijeobjavljenfajl = true;
            }
            Ocistiizvestaj();
        }

        public void Ocistiizvestaj()
        {
            CIzvestaj.l1.Clear();
            CIzvestaj.listaDodatakizv.Clear();
            CIzvestaj.listaGreskeIzv.Clear();
            CIzvestaj.Listaizlaznevr.Clear();
            CIzvestaj.listaparametaraizv.Clear();
            CIzvestaj.listaSpeculparam.Clear();
            CIzvestaj.listaTabizlparamizvestaj.Clear();
            CIzvestaj.listaTabulparamizv.Clear();
            CIzvestaj.SPltizlazvr.Clear();
        }

       

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            CIniFile Ini = new CIniFile(ApiParametri.PutanjadoInifajla.ToString());

            // string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string projectPath = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);

            string Dir = Ini.Read("Direktorijumzaobjavljenefunkcije", "ApiDocument");

            string folderName = System.IO.Path.Combine(projectPath, Dir.ToString());

            Directory.CreateDirectory(folderName);

            string putanja = System.IO.Path.Combine(folderName, CIzvestaj.l1.ElementAt(0).NazivApiFunkcije + ".pdf");

            if (File.Exists(putanja))
            {

                ApiParametri.Dalijeobjavljenfajl = true;
                DateTime creation = File.GetCreationTime(putanja);
                Ini.Write("Vremekreiranjapdf", creation.ToShortTimeString(), "ApiDocument");
            }
            else
            {
                ApiParametri.Dalijeobjavljenfajl = false;
            }
        }

        public void VremekreiranjaPDF()
        {
            CIniFile Ini = new CIniFile(ApiParametri.PutanjadoInifajla.ToString());

            KreirajPDF.Command.Execute(null);
            //string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string projectPath = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string Dir = Ini.Read("Direktorijumzaobjavljenefunkcije", "ApiDocument");
            string folderName = System.IO.Path.Combine(projectPath, Dir.ToString());
            Directory.CreateDirectory(folderName);
            string putanja = System.IO.Path.Combine(folderName, CIzvestaj.l1.ElementAt(0).NazivApiFunkcije + ".pdf");
           
            if (File.Exists(putanja))
            {

                ApiParametri.Dalijeobjavljenfajl = true;
                DateTime creation = File.GetCreationTime(putanja);
                ApiParametri.Vremekreiranjapdf = creation.ToShortTimeString();
                // System.Windows.MessageBox.Show("Vreme kreiranja" +":"+ ApiParametri.Vremekreiranjapdf.ToString());
               
                Ini.KeyExists("Vremekreiranjapdf", "ApiDocument");
              //  Ini.DeleteKey("Vremekreiranjapdf", "ApiDocument");
                Ini.Write("Vremekreiranjapdf", ApiParametri.Vremekreiranjapdf, "ApiDocument");
                
              
            }
            else
            {
                ApiParametri.Dalijeobjavljenfajl = false;
                

            }
        }



        private void KreirajPDF_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void KreirajPDF_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CIzvestaj.listaparametaraizv.ToList();
            CIzvestaj.l1.ToList();
            CIzvestaj.listaSpeculparam.ToList();
            CIzvestaj.listaTabulparamizv.ToList();
            CIzvestaj.listaGreskeIzv.ToList();
            CIzvestaj.Listaizlaznevr.ToList();
            CIzvestaj.SPltizlazvr.ToList();
            CIzvestaj.listaTabizlparamizvestaj.ToList();

            Izvestaj.LocalReport.DataSources.Clear();
            Izvestaj.LocalReport.ReportEmbeddedResource = "ApiDocument.Report1.rdlc";
            string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            //string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
            string putanja = System.IO.Path.Combine(path, "Report1.rdlc");
            Izvestaj.ShowExportButton = true;

            Izvestaj.LocalReport.ReportPath = putanja;
            Izvestaj.AutoSize = false;


            Izvestaj.ZoomMode = ZoomMode.PageWidth;

            Microsoft.Reporting.WinForms.ReportDataSource dataset = new Microsoft.Reporting.WinForms.ReportDataSource("CApiOpisDS", CIzvestaj.l1);
            Microsoft.Reporting.WinForms.ReportDataSource dataset1 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetUP", CIzvestaj.listaparametaraizv);
            Microsoft.Reporting.WinForms.ReportDataSource dataset2 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetSPULP", CIzvestaj.listaSpeculparam);
            Microsoft.Reporting.WinForms.ReportDataSource dataset3 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetTABUP", CIzvestaj.listaTabulparamizv);
            Microsoft.Reporting.WinForms.ReportDataSource dataset4 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetGR", CIzvestaj.listaGreskeIzv);
            Microsoft.Reporting.WinForms.ReportDataSource dataset5 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetTIZLVR", CIzvestaj.Listaizlaznevr);
            Microsoft.Reporting.WinForms.ReportDataSource dataset6 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", CIzvestaj.SPltizlazvr);
            Microsoft.Reporting.WinForms.ReportDataSource dataset7 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetDodatak", CIzvestaj.listaDodatakizv);
            Microsoft.Reporting.WinForms.ReportDataSource dataset8 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSettabizlazniparam", CIzvestaj.listaDodatakizv);



            Izvestaj.LocalReport.DataSources.Add(dataset);
            Izvestaj.LocalReport.DataSources.Add(dataset1);
            Izvestaj.LocalReport.DataSources.Add(dataset2);
            Izvestaj.LocalReport.DataSources.Add(dataset3);
            Izvestaj.LocalReport.DataSources.Add(dataset4);
            Izvestaj.LocalReport.DataSources.Add(dataset5);
            Izvestaj.LocalReport.DataSources.Add(dataset6);
            Izvestaj.LocalReport.DataSources.Add(dataset7);
            Izvestaj.LocalReport.DataSources.Add(dataset8);



            dataset.Value = CIzvestaj.l1;
            dataset1.Value = CIzvestaj.listaparametaraizv;
            dataset2.Value = CIzvestaj.listaSpeculparam;
            dataset3.Value = CIzvestaj.listaTabulparamizv;
            dataset4.Value = CIzvestaj.listaGreskeIzv;
            dataset5.Value = CIzvestaj.Listaizlaznevr;
            dataset6.Value = CIzvestaj.SPltizlazvr;
            dataset7.Value = CIzvestaj.listaDodatakizv;
            dataset8.Value = CIzvestaj.listaTabizlparamizvestaj;


            Izvestaj.LocalReport.Refresh();
            Izvestaj.RefreshReport();
            this.btnPDF_Click(this, null);

        }

        public void Autopdf()
        {
            CIniFile Ini = new CIniFile(ApiParametri.PutanjadoInifajla.ToString());
           
            //string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string projectPath = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string Dir = Ini.Read("Direktorijumzaobjavljenefunkcije", "ApiDocument");
            string folderName = System.IO.Path.Combine(projectPath, Dir.ToString());
            Directory.CreateDirectory(folderName);
            string putanja = System.IO.Path.Combine(folderName, CIzvestaj.l1.ElementAt(0).NazivApiFunkcije + ".pdf");

            if (File.Exists(putanja))
            {

                ApiParametri.Dalijeobjavljenfajl = true;
                  CIzvestaj.listaparametaraizv.ToList();
            CIzvestaj.l1.ToList();
            CIzvestaj.listaSpeculparam.ToList();
            CIzvestaj.listaTabulparamizv.ToList();
            CIzvestaj.listaGreskeIzv.ToList();
            CIzvestaj.Listaizlaznevr.ToList();
            CIzvestaj.SPltizlazvr.ToList();
            CIzvestaj.listaTabizlparamizvestaj.ToList();

            Izvestaj.LocalReport.DataSources.Clear();
            Izvestaj.LocalReport.ReportEmbeddedResource = "ApiDocument.Report1.rdlc";
            string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
           
            Izvestaj.ShowExportButton = true;

            Izvestaj.LocalReport.ReportPath = putanja;
            Izvestaj.AutoSize = false;


            Izvestaj.ZoomMode = ZoomMode.PageWidth;

            Microsoft.Reporting.WinForms.ReportDataSource dataset = new Microsoft.Reporting.WinForms.ReportDataSource("CApiOpisDS", CIzvestaj.l1);
            Microsoft.Reporting.WinForms.ReportDataSource dataset1 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetUP", CIzvestaj.listaparametaraizv);
            Microsoft.Reporting.WinForms.ReportDataSource dataset2 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetSPULP", CIzvestaj.listaSpeculparam);
            Microsoft.Reporting.WinForms.ReportDataSource dataset3 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetTABUP", CIzvestaj.listaTabulparamizv);
            Microsoft.Reporting.WinForms.ReportDataSource dataset4 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetGR", CIzvestaj.listaGreskeIzv);
            Microsoft.Reporting.WinForms.ReportDataSource dataset5 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetTIZLVR", CIzvestaj.Listaizlaznevr);
            Microsoft.Reporting.WinForms.ReportDataSource dataset6 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", CIzvestaj.SPltizlazvr);
            Microsoft.Reporting.WinForms.ReportDataSource dataset7 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetDodatak", CIzvestaj.listaDodatakizv);
            Microsoft.Reporting.WinForms.ReportDataSource dataset8 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSettabizlazniparam", CIzvestaj.listaDodatakizv);



            Izvestaj.LocalReport.DataSources.Add(dataset);
            Izvestaj.LocalReport.DataSources.Add(dataset1);
            Izvestaj.LocalReport.DataSources.Add(dataset2);
            Izvestaj.LocalReport.DataSources.Add(dataset3);
            Izvestaj.LocalReport.DataSources.Add(dataset4);
            Izvestaj.LocalReport.DataSources.Add(dataset5);
            Izvestaj.LocalReport.DataSources.Add(dataset6);
            Izvestaj.LocalReport.DataSources.Add(dataset7);
            Izvestaj.LocalReport.DataSources.Add(dataset8);



            dataset.Value = CIzvestaj.l1;
            dataset1.Value = CIzvestaj.listaparametaraizv;
            dataset2.Value = CIzvestaj.listaSpeculparam;
            dataset3.Value = CIzvestaj.listaTabulparamizv;
            dataset4.Value = CIzvestaj.listaGreskeIzv;
            dataset5.Value = CIzvestaj.Listaizlaznevr;
            dataset6.Value = CIzvestaj.SPltizlazvr;
            dataset7.Value = CIzvestaj.listaDodatakizv;
            dataset8.Value = CIzvestaj.listaTabizlparamizvestaj;


            Izvestaj.LocalReport.Refresh();
            Izvestaj.RefreshReport();
            this.btnPDF_Click(this, null);


            }
            else
            {
                ApiParametri.Dalijeobjavljenfajl = false;


            }
        }



    }
}
        
     
  

    

    

        
       

    





    
                   
                   
            
        
    


                
           
        
    

                

            
    

                
            
    

