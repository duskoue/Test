using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    /// Interaction logic for frmPodesavanja.xaml
    /// </summary>
    public partial class frmPodesavanja : Window
    {
        public CSettingsHelp csh = new CSettingsHelp();

        public ObservableCollection<string> ListaPutanja = new ObservableCollection<string>();
        public CApiDoc API = new CApiDoc();
        public CPomocniParametri APIParametari = new CPomocniParametri();
        MainWindow mainW;
        public frmPodesavanja()
        {
            InitializeComponent();

           

            mainW = System.Windows.Application.Current.MainWindow as MainWindow;

            
            string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string foldername = System.IO.Path.Combine(ProjectPath, "Konfiguracija");
            string putanjadofajla = Convert.ToString(System.IO.Path.Combine(foldername, "Default.ini"));
            CIniFile Ini = new CIniFile(putanjadofajla);
            APIParametari.PutanjadoInifajla = Ini.Read("PutanjaDoIniFajla", "ApiDocument");
            //slider.Value = Properties.Settings.Default.fsize;
            try
            {
                CIniFile MojIni = new CIniFile(APIParametari.PutanjadoInifajla.ToString());
                string VelicinaFonta = MojIni.Read("VelicinaFonta", "ApiDocument");
                slider.Value = Convert.ToInt32(VelicinaFonta);
            }
            catch (Exception)
            {

                return;
            }
            
            
        }
            
        
        private void button_Click(object sender, RoutedEventArgs e)
        {
            CSettingsHelp csh = new CSettingsHelp();
            System.Windows.Forms.FolderBrowserDialog folderDlg = new FolderBrowserDialog();

        }

       

        private void txtSizefont_TextChanged(object sender, TextChangedEventArgs e)
        {

            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            int k = (int)slider.Value;
            string value = Convert.ToString(k);
            CIniFile MojIni = new CIniFile(APIParametari.PutanjadoInifajla.ToString());
            MojIni.KeyExists("VelicinaFonta", "ApiDocument");
            MojIni.DeleteKey("VelicinaFonta", "ApiDocument");
            MojIni.Write("VelicinaFonta",value, "ApiDocument");
            //Properties.Settings.Default.fsize = k;
            //Properties.Settings.Default.Save();

            //  System.Windows.MessageBox.Show(Convert.ToString( API.Fsize.ToString()));

        }

        private void cbSamogrid_Click(object sender, RoutedEventArgs e)
        {
            if (cbSamogrid.IsChecked.Value == true)
            {
                APIParametari.PrikaziSamoGrid = true;
                CIniFile MojIni = new CIniFile(APIParametari.PutanjadoInifajla);
                MojIni.KeyExists("Prikazisamogrid", null);
                MojIni.DeleteKey("Prikazisamogrid", null);
                 MojIni.Write("Prikazisamogrid", APIParametari.PrikaziSamoGrid.ToString(), null);

                //Properties.Settings.Default.prikazisamogrid = true;
                //Properties.Settings.Default.Save();
            }
            else
            {
                APIParametari.PrikaziSamoGrid = false;

                CIniFile MojIni = new CIniFile(APIParametari.PutanjadoInifajla);
                MojIni.KeyExists("Prikazisamogrid", null);
                MojIni.DeleteKey("Prikazisamogrid", null);
                MojIni.Write("Prikazisamogrid", APIParametari.PrikaziSamoGrid.ToString(), null);

                //Properties.Settings.Default.prikazisamogrid = false;
                //Properties.Settings.Default.Save();
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           


            try
            {
                CIniFile MojIni = new CIniFile(APIParametari.PutanjadoInifajla);
                string PrikaziSamoGrid = MojIni.Read("Prikazisamogrid", "ApiDocument");




                if (PrikaziSamoGrid.ToString() == "True")
                {
                    cbSamogrid.IsChecked = true;
                }
                else
                {
                    cbSamogrid.IsChecked = false;
                }

            }
            catch (Exception)
            {

                return;
            }

           
        }

      
    }
}

