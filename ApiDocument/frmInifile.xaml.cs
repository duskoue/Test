using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ApiDocument
{
    /// <summary>
    /// Interaction logic for frmInifile.xaml
    /// </summary>
    public partial class frmInifile : Window
    {
       public CPomocniParametri ApiPatametri = new CPomocniParametri();
        MainWindow mw = new MainWindow();
        public ObservableCollection<string> LDOD ;

        public string PutanjaExplorera { get; set; }
     //   public List<string> Lista = new List<string>();
        public frmInifile()
        {
            InitializeComponent();

            try
            { 
            //// Putanja do ini fajla u kom se nalaze podrazumevana podesavanja
            string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string foldername = System.IO.Path.Combine(ProjectPath, "Konfiguracija");
            string putanjadofajla = Convert.ToString(System.IO.Path.Combine(foldername, "Default.ini"));

                CIniFile Mojini = new CIniFile(putanjadofajla);
                ApiPatametri.PutanjadoInifajla = Mojini.Read("PutanjaDoIniFajla", "ApiDocument");

                if (!File.Exists(putanjadofajla))
                {
                    File.Create(putanjadofajla);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

           
           


            cbPrivilegija.Content = "Privilegija fajla";
            btnObrisiGrupu.IsEnabled = false;
            btnObrisikljuc.IsEnabled = false;
            btnPrepisi.IsEnabled = false;
            btnProverikljuc.IsEnabled = false;
            btnUpisiuini.IsEnabled = false;
         
            cbNivoPristupa.IsChecked= false;


            if (cbNivoPristupa.IsChecked.Value == false)
            {
                cbNivoPristupa.Content = "Korisnik";
                txtGrupa.IsEnabled = false;
            }

            if (string.IsNullOrWhiteSpace(txtNazivFajla.Text))
            {
                btnKreiraj.IsEnabled = false;
            }





            foreach (DriveInfo driv in DriveInfo.GetDrives())
            {
                if (driv.IsReady) Explorer(driv.Name, driv.Name, folders, null, false);
            }


            try
            {
               
                string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string foldername = System.IO.Path.Combine(ProjectPath, "Konfiguracija");
                if (string.IsNullOrWhiteSpace(foldername))
                {
                    MessageBox.Show("Da");
                }
                else
                {
                    CIniFile inif = new CIniFile(ApiPatametri.PutanjadoInifajla);
                   // foldername = inif.Read("DirektorijumIniFajla", "ApiDocument");

                }
                DirectoryInfo direktorijumi = new DirectoryInfo(foldername);

                FileInfo[] Inifajlovi = direktorijumi.GetFiles("*.ini");

                foreach (FileInfo svifajlovi in Inifajlovi)
                {
                    listBox.Items.Add(svifajlovi);
                }
            }
            catch (Exception)
            {

                return;
            }

            }
          
        


        #region Explorer
        private void Explorer(string header, string tag, TreeView root, TreeViewItem child, bool isfile)
        {
            try
            {
                TreeViewItem diritem = new TreeViewItem();
                diritem.Tag = tag;
                diritem.Header = header;
                diritem.Expanded += new RoutedEventHandler(diritem_Expanded);
                if (!isfile)
                    diritem.Items.Add(new TreeViewItem());

                if (root != null)
                { root.Items.Add(diritem); }
                else { child.Items.Add(diritem); }
            }
            catch (Exception)
            {

                return;
            }
        }

        private void diritem_Expanded(object sender, RoutedEventArgs e)
        {
            try
            {
                TreeViewItem item = (TreeViewItem)sender;
                if (item.Items.Count == 1 && ((TreeViewItem)item.Items[0]).Header == null)
                {
                    item.Items.Clear();

                    foreach (string dir in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        DirectoryInfo dirinfo = new DirectoryInfo(dir);
                        Explorer(dirinfo.Name, dirinfo.FullName, null, item, false);
                    }

                    foreach (string dir in Directory.GetFiles(item.Tag.ToString()))
                    {
                        FileInfo dirinfo = new FileInfo(dir);
                        Explorer(dirinfo.Name, dirinfo.FullName, null, item, true);
                    }
                }
            }
            catch (Exception)
            {

                return;
            }
        }


             private void folders_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
              {
            try
            {
                TreeView tree = (TreeView)sender;
                TreeViewItem temp = ((TreeViewItem)tree.SelectedItem);

                if (temp == null)
                    return;

                PutanjaExplorera = "";

                string temp1 = "";
                string temp2 = "";

                while (true)
                {
                    temp1 = temp.Header.ToString();

                    if (temp1.Contains(@"\"))
                    {
                        temp2 = "";
                    }
                    PutanjaExplorera = temp1 + temp2 + PutanjaExplorera;

                    if (temp.Parent.GetType().Equals(typeof(TreeView)))
                    {
                        break;
                    }
                    temp = ((TreeViewItem)temp.Parent);
                    temp2 = @"\";
                }
            }

            catch (Exception)
            {

                return;

            }

            txtPutanja.Text = PutanjaExplorera.ToString();

            var provera = File.GetAttributes(txtPutanja.Text);
            switch (provera)
            {
                case FileAttributes.ReadOnly:
                    cbPrivilegija.IsChecked = true;
                    cbPrivilegija.Content = "ReadOnly";
                    break;

                case FileAttributes.Normal:
                    cbPrivilegija.IsChecked = false;
                    cbPrivilegija.Content = "Normal";

                    break;

                default:
                    break;
            }
        }

        private void folders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.Source is TreeViewItem && ((TreeViewItem)e.Source).IsSelected)
                {

                }

                txtPutanja.Text = PutanjaExplorera.ToString();
                // Process.Start(txtPutanja.Text);
                if (listBox1 != null)
                {
                    listBox1.Items.Clear();
                }


                string extenzija = System.IO.Path.GetExtension(txtPutanja.Text);

                if (extenzija == ".ini")
                {
                    var CitajFajl = File.ReadAllLines(txtPutanja.Text);
                    foreach (var item in CitajFajl)
                    {
                        listBox1.Items.Add(item);
                    }
                }
                else
                {
                    Process.Start(txtPutanja.Text);
                }
            }
            catch (Exception)
            {

                return;
            }




        }

        #endregion

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string foldername = System.IO.Path.Combine(ProjectPath, "Konfiguracija");

            FileInfo fajlovi = new FileInfo(foldername);
            string putanjadofoldera = fajlovi.ToString();
            string putanjadofajla = Convert.ToString( System.IO.Path.Combine(putanjadofoldera, listBox.SelectedItem.ToString()));
            //MessageBox.Show(putanjadofajla);
            txtPutanja.Text = putanjadofajla;
         
            
            CIniFile ini = new CIniFile(ApiPatametri.PutanjadoInifajla);

            ini.Write("SelektovaniIniFajl", putanjadofajla, "ApiDocument");
            ini.Write("DirektorijumIniFajla", foldername);

            if (string.IsNullOrWhiteSpace(txtPutanja.Text))
            {
                txtPutanja.Text = ini.Read("SelektovaniIniFajl");
            }
            if (listBox1 !=null)
            {
                listBox1.Items.Clear();
            }

            string extenzija = System.IO.Path.GetExtension(txtPutanja.Text);

            if (extenzija == ".ini")
            {
                var CitajFajl = File.ReadAllLines(txtPutanja.Text);
                foreach (var item in CitajFajl)
                {
                    listBox1.Items.Add(item);
                }
            }
            else
            {
                Process.Start(txtPutanja.Text);
            }

            var provera = File.GetAttributes(txtPutanja.Text);

          
            if (provera.ToString() == "ReadOnly")
            {

                cbPrivilegija.IsChecked = true;
            }
            else
            {

                cbPrivilegija.IsChecked = false;
            }
            switch (provera)
            {
                case FileAttributes.ReadOnly:

                    cbPrivilegija.IsChecked = true;
                    cbPrivilegija.Content = "ReadOnly";

                    break;



                case FileAttributes.Normal:
                    cbPrivilegija.IsChecked = false;
                    cbPrivilegija.Content = "Normal";
                    break;

                default:
                    break;
            }

        }

       

           


        #region SPISAK FAJLOVA IZ DIREKTORIKUMA
        public void SpisakFajlovaizDirektorijuma()
        {
            if (listBox.Items != null)
            {
                listBox.Items.Clear();
            }

            
                string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string foldername = System.IO.Path.Combine(ProjectPath, "Konfiguracija");

                DirectoryInfo direktorijumi = new DirectoryInfo(foldername);
                FileInfo[] Inifajlovi = direktorijumi.GetFiles("*.ini");

                foreach (FileInfo svifajlovi in Inifajlovi)
                {
                    listBox.Items.Add(svifajlovi);
                }
           }
            
           
            
        

        #endregion

       

        private void listBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string lgrupa = listBox1.SelectedItem.ToString().Substring(1);

            string grupa = lgrupa.Remove(lgrupa.Length - 1);
            txtGrupa.Text = grupa;
            if (grupa.Contains('='))
            {
                txtGrupa.Clear();
                btnUpisiuini.IsEnabled = false;
            }
            else
            {
               
            }

        }

        private void cbPrivilegija_Click(object sender, RoutedEventArgs e)
        {
            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string putanja = System.IO.Path.Combine(projectPath, txtPutanja.Text);

            if (cbPrivilegija.IsChecked.Value == true)
            {
                File.SetAttributes(putanja.ToString(), FileAttributes.ReadOnly);
                cbPrivilegija.Content = "ReadOnly";
            }
            else
            {
                File.SetAttributes(putanja.ToString(), FileAttributes.Normal);
                cbPrivilegija.Content = "Nornal";
            }
        }

        private void btnPrepisiugrupr_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void txtNazivFajla_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNazivFajla.Text != "")
            {
                btnKreiraj.IsEnabled = true;
            }
            else
            {
                btnKreiraj.IsEnabled = false;
            }
        }

        private void btnKreiraj_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string FolderName = System.IO.Path.Combine(ProjectPath, "Konfiguracija");
                string Putanja = System.IO.Path.Combine(FolderName, txtNazivFajla.Text + ".ini");

                var Mojini = new CIniFile(Putanja);
                Mojini.Write("TestoviK", "TestoviV", "TestoviS");
                
               Mojini.DeleteSection("TestoviS");
               Mojini.DeleteKey("TestoviK", "TestoviS");
                btnKreiraj.IsEnabled = false;
                txtNazivFajla.Clear();
                string extenzija = System.IO.Path.GetExtension(txtPutanja.Text);
                Prikazilistufajlova();
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void txtGrupa_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtGrupa.Text !="")
            {
                btnUpisiuini.IsEnabled = true;
                txtKey.IsEnabled = true;
                txtValue.IsEnabled = true;
                btnPrepisi.IsEnabled = true;
                btnObrisiGrupu.IsEnabled = true;
            }
            else
            {
                txtValue.IsEnabled = false;
                txtKey.IsEnabled = false;
                btnObrisiGrupu.IsEnabled = false;
            }
        }

        private void btnUpisiuini_Click(object sender, RoutedEventArgs e)
        {
            if (cbPrivilegija.IsChecked.Value == true)
            {
                MessageBox.Show("Trenutno nije dozvoljeno upisivanje i brisanje podataka u ovom fajlu."
                    + "Promenite privilegije fajla da omogucite ove izmene", "OBAVESTENJE", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (cbNivoPristupa.IsChecked.Value == false)
                {
                    UpisuPodrazGrupu();
                }
                else
                {
                    Upisuini();
                }
            }

        }

        public void Upisuini()
        {
            string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string Putanja = System.IO.Path.Combine(ProjectPath, txtPutanja.Text);
            var MojIni = new CIniFile(Putanja.ToString());
            MojIni.Write(txtKey.Text, txtValue.Text, txtGrupa.Text);
            txtKey.Clear();
            txtValue.Clear();
            string extenzija = System.IO.Path.GetExtension(txtPutanja.Text);

            if (extenzija == ".ini")
            {
                listBox1.Items.Clear();

                var CitajFajl = File.ReadAllLines(txtPutanja.Text);
                foreach (var item in CitajFajl)
                {
                    listBox1.Items.Add(item);
                }
            }
            else
            {
                Process.Start(txtPutanja.Text);
            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string putanja = System.IO.Path.Combine(projectPath, txtPutanja.Text);
                string text = listBox1.SelectedItem.ToString();

                string s = text.Remove(text.LastIndexOf('='));
                txtKey.Text = s.ToString();
                Prikazivrednostkljuca();
                if (txtValue.Text =="")
                {
                    MessageBox.Show("Kljuc se ne nalazi u ovoj grupi.");
                }
                
            }
            catch (Exception)
            {

                return;
            }

        }

       

        private void btnProcitajvrednost_Click(object sender, RoutedEventArgs e)
        {
            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string putanja = System.IO.Path.Combine(projectPath, txtPutanja.Text);
            var MyIni = new CIniFile(putanja.ToString());
            var DefaultVolume = MyIni.Read(txtKey.Text, txtGrupa.Text);
            txtValue.Text = DefaultVolume.ToString();





            MessageBox.Show(DefaultVolume.ToString());
        }

       

        public void Prikazivrednostkljuca()
        {
            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string putanja = System.IO.Path.Combine(projectPath, txtPutanja.Text);
            var MyIni = new CIniFile(putanja.ToString());
            var DefaultVolume = MyIni.Read(txtKey.Text, txtGrupa.Text);
            txtValue.Text = DefaultVolume.ToString();
        }
        public void Prikazilistufajlova()
        {
            if (listBox.Items != null)
            {
                listBox.Items.Clear();
            }
            string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string foldername = System.IO.Path.Combine(ProjectPath, "Konfiguracija");

            DirectoryInfo direktorijumi = new DirectoryInfo(foldername);
            FileInfo[] Inifajlovi = direktorijumi.GetFiles("*.ini");

            foreach (FileInfo svifajlovi in Inifajlovi)
            {
                listBox.Items.Add(svifajlovi);
            }
        }

        private void btnPrepisi_Click(object sender, RoutedEventArgs e)
        {
            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string putanja = System.IO.Path.Combine(projectPath, txtPutanja.Text);
            var MyIni = new CIniFile(putanja.ToString());

            MyIni.Write(txtKey.Text, txtValue.Text, txtNovaGrupa.Text);

            MyIni.DeleteKey(txtKey.Text, txtGrupa.Text);
            txtGrupa.Clear();
            txtKey.Clear();
            txtValue.Clear();

            string extenzija = System.IO.Path.GetExtension(txtPutanja.Text);

            if (extenzija == ".ini")
            {
                listBox1.Items.Clear();

                var CitajFajl = File.ReadAllLines(txtPutanja.Text);
                foreach (var item in CitajFajl)
                {
                    listBox1.Items.Add(item);
                }
                btnPrepisi.IsEnabled = false;
            }
        }

        private void btnObrisikljuc_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult brisanje = MessageBox.Show("Da li zelite da obrisete postojeci kljuc iz fajla ?","UPOZORENJE",MessageBoxButton.YesNo,MessageBoxImage.Warning);
            if (brisanje == MessageBoxResult.Yes)
            {
                if (cbNivoPristupa.IsChecked.Value == false)
                {
                    BrisanjePodrazumevanogKljuca();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(txtGrupa.Text))
                    {
                        MessageBox.Show("Morate uneti naziv grupe u koju je smesten kljuc koji zelite da obrisete.","OBAVESTENJE",MessageBoxButton.OK,MessageBoxImage.Asterisk);
                    }
                    BrisanjeKljuca();
                }
                
            }
            else
            {

            }

            if (cbPrivilegija.IsChecked.Value == true)
            {
                MessageBox.Show("Trenutno nije dozvoljeno upisivanje i brisanje podataka u ovom fajlu."
                    + "Promenite privilegije fajla da omogucite ove izmene", "OBAVESTENJE", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {

              
            }

        }

        public void BrisanjeKljuca()
        {
            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string putanja = System.IO.Path.Combine(projectPath, txtPutanja.Text);
            var MyIni = new CIniFile(putanja.ToString());

            // MyIni.Write(txtKey.Text, txtValue.Text, txtNovaGrupa.Text);

            MyIni.DeleteKey(txtKey.Text, txtGrupa.Text);
            txtGrupa.Clear();
            txtKey.Clear();
            txtValue.Clear();
            string extenzija = System.IO.Path.GetExtension(txtPutanja.Text);

            if (extenzija == ".ini")
            {
                listBox1.Items.Clear();

                var CitajFajl = File.ReadAllLines(txtPutanja.Text);
                foreach (var item in CitajFajl)
                {
                    listBox1.Items.Add(item);
                }
                btnPrepisi.IsEnabled = false;
            }
        }

        private void btnObrisiGrupu_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult brisanjegrupe = MessageBox.Show("Da li zelite da obrisete postojecU naziv grupu podataka iz fajla ?", "UPOZORENJE", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (brisanjegrupe == MessageBoxResult.Yes)
            {
                if (cbNivoPristupa.IsChecked.Value == false)
                {
                    ObrisiPodrazumevanuGrupu();
                }
                else
                {
                    ObrisiGrupuizfajla();
                }
            }
            else
            {

            }

                if (cbPrivilegija.IsChecked.Value ==true)
            {
                MessageBox.Show("Trenutno nije dozvoljeno upisivanje i brisanje podataka u ovom fajlu."
                    + "Promenite privilegije fajla da omogucite ove izmene","OBAVESTENJE",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
            else
            {
           

            }
        }


        public void ObrisiGrupuizfajla()
        {
            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string putanja = System.IO.Path.Combine(projectPath, txtPutanja.Text);
            var MyIni = new CIniFile(putanja.ToString());

            // MyIni.Write(txtKey.Text, txtValue.Text, txtNovaGrupa.Text);

            // MyIni.DeleteKey(txtKey.Text, txtGrupa.Text);

            MyIni.DeleteSection(txtGrupa.Text);

            txtGrupa.Clear();
            txtKey.Clear();
            txtValue.Clear();
            string extenzija = System.IO.Path.GetExtension(txtPutanja.Text);

            if (extenzija == ".ini")
            {
                listBox1.Items.Clear();

                var CitajFajl = File.ReadAllLines(txtPutanja.Text);
                foreach (var item in CitajFajl)
                {
                    listBox1.Items.Add(item);
                }
                btnPrepisi.IsEnabled = false;
            }

        }

        public void ObrisiPodrazumevanuGrupu()
        {

            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string putanja = System.IO.Path.Combine(projectPath, txtPutanja.Text);
            var MyIni = new CIniFile(putanja.ToString());

            // MyIni.Write(txtKey.Text, txtValue.Text, txtNovaGrupa.Text);

            // MyIni.DeleteKey(txtKey.Text, txtGrupa.Text);

            MyIni.DeleteSection(null);

            txtGrupa.Clear();
            txtKey.Clear();
            txtValue.Clear();
            string extenzija = System.IO.Path.GetExtension(txtPutanja.Text);

            if (extenzija == ".ini")
            {
                listBox1.Items.Clear();

                var CitajFajl = File.ReadAllLines(txtPutanja.Text);
                foreach (var item in CitajFajl)
                {
                    listBox1.Items.Add(item);
                }
                btnPrepisi.IsEnabled = false;
            }
        }
    
        private void btnProverikljuc_Click(object sender, RoutedEventArgs e)
        {
            if (cbNivoPristupa.IsChecked.Value == false)
            {
                ProveriKljucuPodrazumevanojGrupi();
            }
            else
            {
                ProveriKljucuGrupi();
            }
        }

        public void ProveriKljucuGrupi()
        {
            if (string.IsNullOrWhiteSpace(txtGrupa.Text))
            {
                MessageBox.Show("Morate uneti naziv grupe");
            }

            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string putanja = System.IO.Path.Combine(projectPath, txtPutanja.Text);

            var MyIni = new CIniFile(putanja.ToString());

            if (MyIni.KeyExists(txtKey.Text, txtGrupa.Text))
            {
              
                MessageBox.Show("Grupa:" + txtGrupa.Text + " " + "Key:" + txtKey.Text + " " + "Value:" + txtValue.Text);
                // MyIni.DeleteKey(txtkey.Text, txtGrupa.Text);

                LDOD.Add(txtValue.Text);
            }
            else
            {
                MessageBox.Show(" Kljuc se ne nalazi u odabranoj grupi parametara ");
                // MyIni.DeleteKey(txtKey.Text, txtGrupa.Text);

            }
        }

        public void ProveriKljucuPodrazumevanojGrupi()
        {
            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string putanja = System.IO.Path.Combine(projectPath, txtPutanja.Text);

            var MyIni = new CIniFile(putanja.ToString());


          var kljuc =  MyIni.KeyExists(txtKey.Text, "ApiDocument");
            MessageBox.Show("Ovak ljuc vec postoji ApiDocument grupi parametara. ");
           
        }
        

        public void UpisuPodrazGrupu()
        {
            string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string Putanja = System.IO.Path.Combine(ProjectPath, txtPutanja.Text);
            var MojIni = new CIniFile(Putanja.ToString());
            MojIni.Write(txtKey.Text, txtValue.Text);
            txtKey.Clear();
            txtValue.Clear();
            string extenzija = System.IO.Path.GetExtension(txtPutanja.Text);

            if (extenzija == ".ini")
            {
                listBox1.Items.Clear();

                var CitajFajl = File.ReadAllLines(txtPutanja.Text);
                foreach (var item in CitajFajl)
                {
                    listBox1.Items.Add(item);
                }
            }
            else
            {
                Process.Start(txtPutanja.Text);
            }
        }
        private void txtKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtKey.Text !="")
            {
               
                btnProverikljuc.IsEnabled = true;
                btnUpisiuini.IsEnabled = true;
                btnObrisikljuc.IsEnabled = true;
            }
            else
            {
                btnProverikljuc.IsEnabled = false;
                btnUpisiuini.IsEnabled = false;
                btnObrisikljuc.IsEnabled = false;
            }
        }

        

        public void BrisanjePodrazumevanogKljuca()
        {
            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string putanja = System.IO.Path.Combine(projectPath, txtPutanja.Text);
            var MyIni = new CIniFile(putanja.ToString());

           

            MyIni.DeleteKey(txtKey.Text);
            txtGrupa.Clear();
            txtKey.Clear();
            txtValue.Clear();
            string extenzija = System.IO.Path.GetExtension(txtPutanja.Text);

            if (extenzija == ".ini")
            {
                listBox1.Items.Clear();

                var CitajFajl = File.ReadAllLines(txtPutanja.Text);
                foreach (var item in CitajFajl)
                {
                    listBox1.Items.Add(item);
                }
                btnPrepisi.IsEnabled = false;
            }
        }

      

       

     
        private void cbNivoPristupa_Click(object sender, RoutedEventArgs e)
        {
            if (cbNivoPristupa.IsChecked.Value == true)
            {
                cbNivoPristupa.Content = "Administrator";
                txtGrupa.IsEnabled = true;
            }
            else
            {
                cbNivoPristupa.Content = "Korisnik";
            }
        }

        private void txtNovaGrupa_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNovaGrupa.Text !="")
            {
                btnPrepisi.IsEnabled = true;
            }
            else
            {
                btnPrepisi.IsEnabled = false;
            }
        }

        private void cbPutanjaInifajla_Click(object sender, RoutedEventArgs e)
        {
            if (cbPutanjaInifajla.IsChecked.Value == true)
            {
                string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string foldername = System.IO.Path.Combine(ProjectPath, "Konfiguracija");

                FileInfo fajlovi = new FileInfo(foldername);
                string putanjadofoldera = fajlovi.ToString();
                string putanjadofajla = Convert.ToString(System.IO.Path.Combine(putanjadofoldera, listBox.SelectedItem.ToString()));

                ApiPatametri.PutanjadoInifajla = putanjadofajla.ToString();
                MessageBox.Show(ApiPatametri.PutanjadoInifajla.ToString());
                string Podrazumevaniinifajl = System.IO.Path.Combine(foldername, "Default.ini");
                CIniFile MojIni = new CIniFile(Podrazumevaniinifajl);
                MojIni.Write("PutanjaDoIniFajla", ApiPatametri.PutanjadoInifajla.ToString(), null);
                MojIni.Write("PutanjainiDirektorijuma", foldername);

            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string exePath = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            //Assembly.GetExecutingAssembly().Location;
            string ext = ".afd";
            RegistryKey key = Registry.CurrentUser.CreateSubKey(ext);
           
            key.SetValue("", "ApiDokumentacija");
            key.Close();

            key = Registry.CurrentUser.CreateSubKey(ext + "\\Shell\\Open\\command");
            key = key.CreateSubKey("command");

            key.SetValue("", "\"" + exePath + "\" \"%L\"");
            key.Close();

            key = Registry.CurrentUser.CreateSubKey(ext + "\\DefaultIcon");
            key.SetValue("", exePath + "\\ikonica.ico");
            key.Close();
            MessageBox.Show("extensoja upisana");

        }

       

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string Putanja = System.IO.Path.Combine(ProjectPath, txtPutanja.Text);
            var MojIni = new CIniFile(Putanja.ToString());
            MojIni.Write(txtKey.Text, txtValue.Text);
            txtKey.Clear();
            txtValue.Clear();
            string extenzija = System.IO.Path.GetExtension(txtPutanja.Text);

            if (extenzija == ".ini")
            {
                listBox1.Items.Clear();

                var CitajFajl = File.ReadAllLines(txtPutanja.Text);
                foreach (var item in CitajFajl)
                {
                    listBox1.Items.Add(item);
                }
            }
            else
            {
                Process.Start(txtPutanja.Text);
            }
        }
    }
}













