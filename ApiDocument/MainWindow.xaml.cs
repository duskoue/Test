using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace ApiDocument
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        frmInifile frmini;
        frmPodesavanja podesavanja = new frmPodesavanja();
        public CApiDoc API = new CApiDoc();
        public CPomocniParametri ApiParametri = new CPomocniParametri();
        public List<string> TipDodataka;

        #region POMOCNE LISTE ZA AUTO KOREKCIJU
        public List<string> SG = new List<string>();
        public List<string> ALULP = new List<string>();
        public List<string> SPECULP = new List<string>();
        public List<string> TABULPARAM = new List<string>();
        public List<string> IZLAZNEVR = new List<string>();
        public List<string> SPECIZLAZNEVR = new List<string>();
        public List<string> TABIZLVR = new List<string>();
        #endregion

        public MainWindow()
        {
            CPositionHelper.SetSize(this);


            this.Title = "Api dokumentacija InfoSys Užice";
            // this.KeyDown += new System.Windows.Input.KeyEventHandler(MainWindow_KeyDown);
            frmini = System.Windows.Application.Current.MainWindow as frmInifile;

            //string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string ProjectPath = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);

            string foldername = System.IO.Path.Combine(ProjectPath, "Konfiguracija");
            string putanjadofajla = Convert.ToString(System.IO.Path.Combine(foldername, "Default.ini"));
            CIniFile Ini = new CIniFile(putanjadofajla);
            ApiParametri.PutanjadoInifajla = Ini.Read("PutanjaDoIniFajla", "ApiDocument");
            ApiParametri.VelicinaFonta = Ini.Read("VelicinaFonta", "ApiDocument");
            Ini.Write("sacuvanfajl", "False", "ApiDocument");
            string sacuvanfajl = Ini.Read("sacuvanfajl", "ApiDocument");
            ApiParametri.ForsiraniParametri = Ini.Read("Forsiranapolja", "ApiDocument");
            ApiParametri.SinhroniyacijaListi = false;

            #region CITANJE PARAMETARA TIP DODATKA IZ INI FAJLA
            ApiParametri.SlikaJPG = Ini.Read("Tip0", "ListaTipovaDodataka");
            ApiParametri.SlikaPNG = Ini.Read("Tip1", "ListaTipovaDodataka");
            ApiParametri.SlikaGif = Ini.Read("Tip2", "ListaTipovaDodataka");
            ApiParametri.Text = Ini.Read("Tip3", "ListaTipovaDodataka");
            ApiParametri.ZIP = Ini.Read("Tip4", "ListaTipovaDodataka");

            ApiParametri.Ext1 = Ini.Read("Tip5", "ListaTipovaDodataka");
            ApiParametri.Ext2 = Ini.Read("Tip6", "ListaTipovaDodataka");
            ApiParametri.Ext3 = Ini.Read("Tip7", "ListaTipovaDodataka");
            ApiParametri.Ext4 = Ini.Read("Tip8", "ListaTipovaDodataka");
            ApiParametri.Ext5 = Ini.Read("Tip9", "ListaTipovaDodataka");
            ApiParametri.Ext6 = Ini.Read("Tip10", "ListaTipovaDodataka");
            #endregion

            #region UCITAVANJE LISTE ZA AUTO KOREKCIJU STATUSI GRESAKA

            try
            {

                string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string PutanjadoAutoListeSG = Convert.ToString(System.IO.Path.Combine(path, "AutoListaSTATUSIGRESAKA.txt"));

                string[] readTextsg = File.ReadAllLines(PutanjadoAutoListeSG);

                foreach (string sg in readTextsg)
                {
                    SG.Add(sg);

                }
                txtIdentifgreske.TextChanged += txtIdentifgreske_TextChanged;
            }
            catch (Exception)
            {

                return;
            }


            // ulazni parametri


            try
            {
                string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string PutanjadoAutoListeulp = Convert.ToString(System.IO.Path.Combine(path, "AutoListaUlazniparametri.txt"));

                string[] readText1 = File.ReadAllLines(PutanjadoAutoListeulp);
                foreach (string sg in readText1)
                {
                    ALULP.Add(sg);

                }
                txtNazivulparam.TextChanged += txtNazivulparam_TextChanged;
            }
            catch (Exception)
            {

                return;
            }

            // specijalni ulazni parametri

            try
            {
                string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string PutanjadoAutoListespulparam = Convert.ToString(System.IO.Path.Combine(path, "AutolistaSpeculparam.txt"));
                string[] Procitajtext = File.ReadAllLines(PutanjadoAutoListespulparam);
                foreach (string item in Procitajtext)
                {
                    SPECULP.Add(item);
                }
                txtNazivspeculparam.TextChanged += txtNazivspeculparam_TextChanged;
            }
            catch (Exception)
            {

                return;
            }

            //tabelarni ulazni parametri AutoSugestijatabelarniizlazniparametri

            try
            {
                string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string PutanjadoAutoListetabulparam = Convert.ToString(System.IO.Path.Combine(path, "AutoListaTabeleanuUlazniparametri.txt"));
                string[] Procitajtext = File.ReadAllLines(PutanjadoAutoListetabulparam);
                foreach (string item in Procitajtext)
                {
                    TABULPARAM.Add(item);
                }
                txtPoletabulparam.TextChanged += txtPoletabizlazniparam_TextChanged;
            }
            catch (Exception)
            {

                return;
            }

            // izlazne vredosti

            try
            {
                string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string PutanjadoAutoListeizlaznevr = Convert.ToString(System.IO.Path.Combine(path, "AutoListaIzlazneVrednosti.txt"));
                string[] Procitajtext = File.ReadAllLines(PutanjadoAutoListeizlaznevr);
                foreach (string item in Procitajtext)
                {
                    IZLAZNEVR.Add(item);
                }
            }
            catch (Exception)
            {

                return;
            }

            //specijalne Izlazne vr
            try
            {
                string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string PutanjadoAutoListespecizlaznevr = Convert.ToString(System.IO.Path.Combine(path, "AutoListaSpecizlaznevr.txt"));
                string[] Procitajtext = File.ReadAllLines(PutanjadoAutoListespecizlaznevr);
                foreach (string item in Procitajtext)
                {
                    SPECIZLAZNEVR.Add(item);
                }
            }
            catch (Exception)
            {

                return;
            }
            //AutoSugestijatabelarniizlazniparametri

            try
            {
                string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string PutanjadoAutoListetabizlazniparam = Convert.ToString(System.IO.Path.Combine(path, "AutoSugestijatabelarniizlazniparametri.txt"));
                string[] Procitajtext = File.ReadAllLines(PutanjadoAutoListetabizlazniparam);
                foreach (string item in Procitajtext)
                {
                    TABIZLVR.Add(item);
                }
                txtPoletabizlazniparam.TextChanged += txtPoletabizlazniparam_TextChanged;
            }
            catch (Exception)
            {

                return;
            }

        }






        #endregion

            #region KOLEKCIJE

        public ObservableCollection<CUIParam> Lista = new ObservableCollection<CUIParam>();

        //2**********************
        public ObservableCollection<CSpecUlparam> ListaSpeculparam = new ObservableCollection<CSpecUlparam>();

        //3**************** lista
        public ObservableCollection<CTabUlparam> ListaTabulparam = new ObservableCollection<CTabUlparam>();

        //***************
        public ObservableCollection<CStatusGreske> ListaGreske = new ObservableCollection<CStatusGreske>();

        //*******************
        public ObservableCollection<CIzlaznevr> Listaizlaznihvr = new ObservableCollection<CIzlaznevr>();

        //*****************
        public ObservableCollection<CSpizlazvr> Listaspizlaznihvr = new ObservableCollection<CSpizlazvr>();

        //**********************************
        public ObservableCollection<CDodatak> Listadodataka = new ObservableCollection<CDodatak>();

        //************************************

        public ObservableCollection<CApiDoc> listaAPI = new ObservableCollection<CApiDoc>();
        //***********************************
        public ObservableCollection<CTabIzlazniParametri> listaTabizlparam = new ObservableCollection<CTabIzlazniParametri>();
        //************************************
        public ObservableCollection<CSaradnici> Listasaradnika = new ObservableCollection<CSaradnici>();

        #endregion

            #region KOMANDE

        #region KOMANDA SAVE AS SERIALIZACIJA

        // KOMANDE**************************************************************
        public bool StanjeSaveAsKomande = false;
        public bool StatusForsirano = false;
        private void SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            CIniFile ini = new CIniFile(ApiParametri.PutanjadoInifajla);
            string statusforsiranihpolja = ini.Read("Forsiranapolja", "ApiDocument");
            e.CanExecute = ApiParametri.VidljivostSaveAs;

            if (lscenarioforsiran == true || lforsiraniparametarmodul==true
                || lForsiraniparametarOpisfunkcije==true|| lForsiraniparametriOpislogike==true
                || lForsiraniparametriimesaradnika == true || string.IsNullOrWhiteSpace(txtnazivfunkcija.Text))
                
            {
                StatusForsirano = true;
            }
            else
            {
                StatusForsirano = false;
            }

            if (statusforsiranihpolja == "True" && StatusForsirano == true )
            {
                e.CanExecute = false;
            }
            else
            {
                if (ApiParametri.VidljivostSaveAs == true)
                {
                    e.CanExecute = true;

                }
                else
                {
                    e.CanExecute = false;
                }

            }
        }

        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                ApiParametri.VidljivostSaveAs = true;
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = txtnazivfunkcija.Text;
                dlg.DefaultExt = ".afd";
                dlg.Filter = "ApiFunctionsDocument (.afd)|*.afd| Text Document (*.txt)|*.txt|All Files (*.*)|*.*";

                if (dlg.ShowDialog() == true)
                {
                    TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvano"];

                    string savePath = dlg.InitialDirectory + dlg.FileName;
                    if (string.IsNullOrWhiteSpace(txtnazivfunkcija.Text))
                    {
                        txtnazivfunkcija.Text = Path.GetFileName(dlg.FileName);
                    }


                    API.NazivApiFunkcije = txtnazivfunkcija.Text;
                    API.Modul = txtModul.Text;
                    API.Opisfunkcije = new TextRange(rbOpisfunkcije.Document.ContentStart, rbOpisfunkcije.Document.ContentEnd).Text;
                    API.Opislogikefunkcije = new TextRange(rbopisLogikefunkcije.Document.ContentStart, rbopisLogikefunkcije.Document.ContentEnd).Text;
                    API.Ogrucestalostifunkcija = new TextRange(rbOgrucestfunkc.Document.ContentStart, rbOgrucestfunkc.Document.ContentEnd).Text;
                    API.Scenario = new TextRange(rbScenario.Document.ContentStart, rbScenario.Document.ContentEnd).Text;

                    string ULP = JsonConvert.SerializeObject(Lista, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                    { ContractResolver = new DynamicContractResolver("ulazn") });

                    API.ListaUlaznihParametara = ULP;
                    //2************** serializtj po parametru
                    string SPULP = JsonConvert.SerializeObject(ListaSpeculparam, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                    { ContractResolver = new DynamicContractResolver("s") });

                    API.ListaSpecijalizovanihUlaznihParametara = SPULP;
                    //3 ************************************
                    string TULP = JsonConvert.SerializeObject(ListaTabulparam, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                    { ContractResolver = new DynamicContractResolver("ulparam") });
                    API.ListaTabelarnihUlaznihParametara = TULP;
                    //3 ************************************
                    string SGR = JsonConvert.SerializeObject(ListaGreske, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                    { ContractResolver = new DynamicContractResolver("g") });
                    API.ListaParametaraStatusiGresaka = SGR;

                    //4************************
                    string IZLVR = JsonConvert.SerializeObject(Listaizlaznihvr, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                    { ContractResolver = new DynamicContractResolver("z") });
                    API.ListaIzlaznihParametara = IZLVR;

                    //********************************
                    string SPIZLVR = JsonConvert.SerializeObject(Listaspizlaznihvr, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                    { ContractResolver = new DynamicContractResolver("s") });
                    API.ListaSpecijalizovanihIzlaznihParametara = SPIZLVR;
                    //**********************************
                    string DOD = JsonConvert.SerializeObject(Listadodataka, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                    { ContractResolver = new DynamicContractResolver("k") });
                    API.ListaPazametarazaDodatke = DOD;
                    //***********************************

                    string TABIZLPARAM = JsonConvert.SerializeObject(listaTabizlparam, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                    { ContractResolver = new DynamicContractResolver("izlaz") });
                    API.ListaTabelarnihIzlaznihParametara = TABIZLPARAM;
                    //**************************************************
                    string SARAD = JsonConvert.SerializeObject(Listasaradnika, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                    { ContractResolver = new DynamicContractResolver("saradnika") });
                    API.ListaSaradnika = SARAD;

                    //**************************************************
                    API.Save(savePath);
                    CIniFile ini = new CIniFile(ApiParametri.PutanjadoInifajla);
                    ini.Write("sacuvanfajl", "True", "ApiDocument");
                    ApiParametri.VidljivostSaveAs = false;
                    StanjeSaveAsKomande = false;
                    ApiParametri.VidljivostkomandeSave = false;
                    string sacuvanfajl = ini.Read("sacuvanfajl", "ApiDocument");
                    string autopdf = ini.Read("Autopdf", "ApiDocument");
                    if (autopdf == "True")
                    {

                        this.Izvestaj_Executed(null, null);
                    }

                    Process.GetCurrentProcess().Kill();
                }
            }
            catch (Exception)
            {

                return;
            }



        }
        #endregion

        #region KOMANDA OPEN DESERIJALIZACIJA
        // ukoliko nije ucitan fajl omogucava open komandu
        public bool ucitanfajl = true;
        public bool prikaz = true;


        private void OpenFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ucitanfajl;

            if (e.CanExecute != ucitanfajl)
            {

                e.CanExecute = false;
            }
        }

        private void OpenFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CApiDoc API1 = new CApiDoc();
            ApiParametri.VidljivostkomandeSave = false;
            StanjeSaveAsKomande = false;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "ApiFunctionsDocument (.afd)|*.afd| Text Document (*.txt)|*.txt|All Files (*.*)|*.*";


            if (openFileDialog.ShowDialog() == true)
            {

                CIniFile INI = new CIniFile(ApiParametri.PutanjadoInifajla);
                INI.Write("sacuvanfajl", Convert.ToString(ApiParametri.Sacuvano = true), "ApiDocument");

                ucitanfajl = false;


                this.Tabitem.Header = txtnazivfunkcija.Text;
                string strfilename = openFileDialog.InitialDirectory + openFileDialog.FileName;

                //////////////////////////////////////////////////////////////////
                ApiParametri.PutanjaSacuvanogDokumenta = strfilename.ToString();

                Objavljeno_Executed(null, null);

                CApiDoc API = CApiDoc.LoadFromFile(strfilename);
                ApiParametri.VidljivostkomandeSave = false;


                txtnazivfunkcija.Background = Brushes.White;

                txtModul.TextChanged -= txtModul_TextChanged;
                txtModul.BorderBrush = Brushes.Blue;
                txtModul.BorderThickness = new Thickness(0.5);

                //deserializuj listu
                try
                {
                    string ULP1 = API.ListaUlaznihParametara;
                    var UlazniParametri = JsonConvert.DeserializeObject<IEnumerable<CUIParam>>(ULP1);


                    foreach (var Deserialprop in UlazniParametri)
                    {

                        txtNazivulparam.Text = Deserialprop.Nazivulazniparametri;
                        rbOpisulp.AppendText(Deserialprop.Opisulazniparam);
                        txtdefulparam.Text = Deserialprop.Obaveznostulaznogparam;
                        txtobavulparam.Text = Deserialprop.Defaultulaznogparam;
                        txtTipUlaznogparametra.Text = Deserialprop.Tipulaznogparametra;
                        txtDuzinaUlaznogparametra.Text = Deserialprop.Duzinaulaznogparametra;


                        Lista.Add(Deserialprop);

                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                      " " + "Proverite  listu ulaznih parametara");
                }

                ////////
                txtNazivulparam.Clear();
                rbOpisulp.Document.Blocks.Clear();
                txtdefulparam.Clear();
                txtobavulparam.Clear();

                txtTipUlaznogparametra.Clear();
                txtDuzinaUlaznogparametra.Clear();

                dgulparam.ItemsSource = Lista;

                //Deserijalizuj listu saradnika

                try
                {
                    string SARAD1 = API.ListaSaradnika;

                    var svisaradnici = JsonConvert.DeserializeObject<IEnumerable<CSaradnici>>(SARAD1);
                    foreach (var Deserialsarad in svisaradnici)
                    {

                        txtIme.Text = Deserialsarad.Sifrasaradnika;




                        Listasaradnika.Add(Deserialsarad);

                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                       " " + "Proverite  listu specijalizovanih ulaznih parametara");
                }



                dgSarad.ItemsSource = Listasaradnika;
                /////////


                txtIme.Clear();






                //2*******************************
                try
                {
                    string SPULP1 = API.ListaSpecijalizovanihUlaznihParametara;

                    var SpecUlazniParametri = JsonConvert.DeserializeObject<IEnumerable<CSpecUlparam>>(SPULP1);
                    foreach (var Deserialspulp in SpecUlazniParametri)
                    {

                        txtNazivspeculparam.Text = Deserialspulp.specNazivulazniparametri;
                        rbOpisSpeculparam.AppendText(Deserialspulp.specOpisulazniparam);
                        txtObavezSpeculparam.Text = Deserialspulp.specObaveznostulaznogparam;
                        txtDefspulparam.Text = Deserialspulp.specDefaultulaznogparam;
                        ListaSpeculparam.Add(Deserialspulp);

                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                       " " + "Proverite  listu specijalizovanih ulaznih parametara");
                }



                dgSpeculparam.ItemsSource = ListaSpeculparam;
                /////////
                txtNazivspeculparam.Clear();
                rbOpisSpeculparam.Document.Blocks.Clear();
                txtObavezSpeculparam.Clear();
                txtDefspulparam.Clear();


                //**************************

                try
                {
                    string TULP1 = API.ListaTabelarnihUlaznihParametara;
                    var Tabelarniularametri = JsonConvert.DeserializeObject<IEnumerable<CTabUlparam>>(TULP1);
                    foreach (var Deserialtulp1 in Tabelarniularametri)
                    {
                        txtNazivtabulparam.Text = Deserialtulp1.Nazivtabelarniulparam;
                        txtPoletabulparam.Text = Deserialtulp1.Poljetabelarniulparam;
                        txtdefulparam.Text = Deserialtulp1.Tipulparam;
                        txtduztabulparam.Text = Deserialtulp1.Duzinapoljaulparam;
                        rbOpistabulparam.AppendText(Deserialtulp1.Opistabelarniulparam);
                        txtInfosys.Text = Deserialtulp1.Infosusulparam;


                        ListaTabulparam.Add(Deserialtulp1);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                        " " + "Proverite  listu tabelarnih ulaznih parametara");
                }

                dgTabulparam.ItemsSource = ListaTabulparam;
                //////////
                txtNazivtabulparam.Clear();
                txtPoletabulparam.Clear();
                txtdefulparam.Clear();
                txtduztabulparam.Clear();
                rbOpistabulparam.Document.Blocks.Clear();
                txtInfosys.Clear();


                //******************************
                try
                {
                    string SGR1 = API.ListaParametaraStatusiGresaka;
                    var StatGr = JsonConvert.DeserializeObject<IEnumerable<CStatusGreske>>(SGR1);
                    foreach (var Desergreske in StatGr)
                    {
                        txtIdentifgreske.Text = Desergreske.Identigikatorgreske;
                        txtTiogreske.Text = Desergreske.Tipgreske;

                        rbopisgreske.AppendText(Desergreske.Opisgreske);


                        ListaGreske.Add(Desergreske);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                        " " + "Proverite  listu statusa greski");
                }


                dgstatusgreske.ItemsSource = ListaGreske;
                ///////
                txtIdentifgreske.Clear();
                txtTiogreske.Clear();
                rbopisgreske.Document.Blocks.Clear();
                //****************************
                try
                {
                    string IZLVR1 = API.ListaIzlaznihParametara;
                    var Izlazvrednost = JsonConvert.DeserializeObject<IEnumerable<CIzlaznevr>>(IZLVR1);
                    foreach (var Deserizlvr in Izlazvrednost)
                    {
                        txtIzlvrn.Text = Deserizlvr.Nazivizlaznevr;


                        rbopisizlvr.AppendText(Deserizlvr.Opisizlaznevr);


                        Listaizlaznihvr.Add(Deserizlvr);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                       " " + "Proverite  listu izlaznih parametara");

                }
                dgIzlaznevr.ItemsSource = Listaizlaznihvr;
                //////////
                txtIzlvrn.Clear();

                rbopisizlvr.Document.Blocks.Clear();



                //**********************************************
                try
                {
                    string SPIZLVR1 = API.ListaSpecijalizovanihIzlaznihParametara;
                    var spIzlazvrednost = JsonConvert.DeserializeObject<IEnumerable<CSpizlazvr>>(SPIZLVR1);
                    foreach (var Deserspizlvr in spIzlazvrednost)
                    {
                        txtnazspizlvr.Text = Deserspizlvr.Nazivspizlaznevr;
                        txtTipspecizlaznevr.Text = Deserspizlvr.TipSpecijalneIzlaznevrednosti.ToString();

                        rbspizlvr.AppendText(Deserspizlvr.Opisspizlaznevr);


                        Listaspizlaznihvr.Add(Deserspizlvr);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                        " " + "Proverite  listu specijalizovanih izlaznih parametara");
                }
                dgspizlazvre.ItemsSource = Listaspizlaznihvr;
                ////////
                txtnazspizlvr.Clear();
                rbspizlvr.Document.Blocks.Clear();
                txtTipspecizlaznevr.Clear();
                //*************************************
                string DOD1 = API.ListaPazametarazaDodatke;
                var propdodatka = JsonConvert.DeserializeObject<IEnumerable<CDodatak>>(DOD1);
                foreach (var Deserdod in propdodatka)
                {
                    txtNazivDodatka.Text = Deserdod.Dodataknaziv;
                    txtTipDodatka.Text = Deserdod.Dodataktip;
                    rbOpisdodatka.AppendText(Deserdod.Opisdodatka);
                    rbDodatakNapomena.AppendText(Deserdod.NapomeanaDodatak);
                    Listadodataka.Add(Deserdod);
                }
                //////////
                dgDodatak.ItemsSource = Listadodataka;
                txtNazivDodatka.Clear();
                txtTipDodatka.Clear();
                rbOpisdodatka.Document.Blocks.Clear();
                rbDodatakNapomena.Document.Blocks.Clear();
                cbTipdodatak.Items[0].ToString();
                //****************************************

                try
                {
                    string TABIZLPARAM1 = API.ListaTabelarnihIzlaznihParametara;
                    var propptabizlaznevr = JsonConvert.DeserializeObject<IEnumerable<CTabIzlazniParametri>>(TABIZLPARAM1);
                    foreach (var Desertabizlvr in propptabizlaznevr)
                    {
                        txtNazivtabizlazniparam.Text = Desertabizlvr.Nazivtabelarniizlazniparam;
                        txtPoletabizlazniparam.Text = Desertabizlvr.Poljetabelarniizlazniparam;
                        txttipizlazniparam.Text = Desertabizlvr.Tipizlazniparam;
                        txtduztabizlazniparam.Text = Desertabizlvr.Duzinapoljaizlazniparam;
                        txttzlaznuInfosys.Text = Desertabizlvr.Infosysizlazniparam;
                        rbOpistabizlazniparam.AppendText(Desertabizlvr.Opistabelarniizlazniparam);
                        listaTabizlparam.Add(Desertabizlvr);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                          " " + "Proverite  listutabelarnih izlaznih parametara");
                }
                ////////////////////////////////
                dgTabizlazniparam.ItemsSource = listaTabizlparam;
                txtNazivtabizlazniparam.Clear();
                txtPoletabizlazniparam.Clear();
                txttipizlazniparam.Clear();
                txtduztabizlazniparam.Clear();
                txttzlaznuInfosys.Clear();
                rbOpistabizlazniparam.Document.Blocks.Clear();


                //***************************************


                //***************************************

                txtModul.Text = API.Modul;
                txtnazivfunkcija.Text = API.NazivApiFunkcije;
                rbOpisfunkcije.AppendText(API.Opisfunkcije);
                rbopisLogikefunkcije.AppendText(API.Opislogikefunkcije);
                rbOgrucestfunkc.AppendText(API.Ogrucestalostifunkcija);
                rbScenario.AppendText(API.Scenario);
                ApiParametri.VidljivostkomandeSave = false;

                if (ApiParametri.VidljivostSaveAs == false)
                {
                    TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvano"];
                }
                else
                {
                    TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];
                }
                ApiParametri.Sacuvano = false;
                StatusKesiranja();
                ValidacijanaExp();
                Validacijaexpopis();
                ValidacijaScenarioexp();
            }


        }

        #endregion

        #region KOMANDA PRIKAZI DOKUMENT
        private void Izvestaj_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;

        }

        private void Izvestaj_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Zaljucajsve_Executed(null, null);

            API.NazivApiFunkcije = txtnazivfunkcija.Text;
            API.Modul = txtModul.Text;
            API.Opisfunkcije = new TextRange(rbOpisfunkcije.Document.ContentStart, rbOpisfunkcije.Document.ContentEnd).Text;
            API.Opislogikefunkcije = new TextRange(rbopisLogikefunkcije.Document.ContentStart, rbopisLogikefunkcije.Document.ContentEnd).Text;
            API.Ogrucestalostifunkcija = new TextRange(rbOgrucestfunkc.Document.ContentStart, rbOgrucestfunkc.Document.ContentEnd).Text;
            API.Scenario = new TextRange(rbScenario.Document.ContentStart, rbScenario.Document.ContentEnd).Text;
            CIzvestaj.l1.Add(API);

            if (ApiParametri.StanjedugmetaObavljeno == true)
            {
                Window1 w1 = new Window1();
                w1.btnPDF.Visibility = Visibility.Hidden;
                w1.snimipdf = true;

                w1.Show();
            }
            else
            {
                Window1 w1 = new Window1();
                w1.btnPDF.Visibility = Visibility.Visible;
                w1.snimipdf = false;
                w1.Show();
            }

        }


        #endregion

        #region  KOMANDA IZLAZ
        private void Izlaz_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Izlaz_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            CPositionHelper.SaveSize(this);

            if (ApiParametri.VidljivostSaveAs == true)
            {

                this.SaveAs_Executed(null, null);
                Process.GetCurrentProcess().Kill();
            }
          

            if (ApiParametri.VidljivostkomandeSave == true)
            {

                this.Save_Executed(null, null);
                Process.GetCurrentProcess().Kill();
            }
          

            if (ApiParametri.VidljivostSaveAs == false && ApiParametri.VidljivostkomandeSave == false)
            {
                Process.GetCurrentProcess().Kill();
            }



        }
        #endregion

        #region KOMANDA ZAKLJUCAVANJA

        private void Zaljucajsve_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Zaljucajsve_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            Zakljucaj.IsChecked = true;
            Zakljucajspulp.IsChecked = true;
            Zakljucajtabulp.IsChecked = true;
            rbZakljucajstatus.IsChecked = true;
            rbZakljucajizvr.IsChecked = true;
            Zakljucajspizlazvr.IsChecked = true;
            ZakljucajDodatak.IsChecked = true;
            Zakljucajtabizlaznip.IsChecked = true;

            Zakljucaj.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Zakljucajspulp.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Zakljucajtabulp.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            rbZakljucajstatus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            rbZakljucajizvr.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Zakljucajspizlazvr.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            ZakljucajDodatak.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Zakljucajtabizlaznip.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        }

        #endregion

        #region KOMANDA SAVE

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ApiParametri.VidljivostkomandeSave;

            CIniFile ini = new CIniFile(ApiParametri.PutanjadoInifajla);
            string statusforsiranihpolja = ini.Read("Forsiranapolja", "ApiDocument");


            if (lscenarioforsiran == true || lforsiraniparametarmodul == true
                || lForsiraniparametarOpisfunkcije == true || lForsiraniparametriOpislogike == true
                || lForsiraniparametriimesaradnika == true || string.IsNullOrWhiteSpace(txtnazivfunkcija.Text))

            {
                StatusForsirano = true;
            }
            else
            {
                StatusForsirano = false;
            }

            /////////////////////
            if (statusforsiranihpolja == "True" && StatusForsirano == true)
            {
                e.CanExecute = false;
            }
            else
            {
                if (ApiParametri.VidljivostkomandeSave == true)
                {
                    e.CanExecute = true;

                }
                else
                {
                    e.CanExecute = false;
                }

                //////////////////////////
            }

        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = false;
            MessageBoxResult result = System.Windows.MessageBox.Show(
                    "Da li zelite da sacuvate izmene", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                DateTime vremeizmene = DateTime.Now;
                CIniFile Ini = new CIniFile(ApiParametri.PutanjadoInifajla);
                Ini.Write("VremePoslednjeIzmenefajla", vremeizmene.ToShortTimeString(), "ApiDocument");

                ApiParametri.Vremeposlednjeizmenefajla = Ini.Read("VremePoslednjeIzmenefajla", "ApiDocument");



                if (ApiParametri.Vremeposlednjeizmenefajla != ApiParametri.Vremekreiranjapdf)
                {

                    btnzavrseno.BorderBrush = Brushes.Red;

                }
                else
                {
                    btnzavrseno.BorderBrush = Brushes.Green;
                }

                ApiParametri.VidljivostkomandeSave = true;
                API.NazivApiFunkcije = txtnazivfunkcija.Text;
                API.Modul = txtModul.Text;
                API.Opisfunkcije = new TextRange(rbOpisfunkcije.Document.ContentStart, rbOpisfunkcije.Document.ContentEnd).Text;
                API.Opislogikefunkcije = new TextRange(rbopisLogikefunkcije.Document.ContentStart, rbopisLogikefunkcije.Document.ContentEnd).Text;
                API.Ogrucestalostifunkcija = new TextRange(rbOgrucestfunkc.Document.ContentStart, rbOgrucestfunkc.Document.ContentEnd).Text;
                API.Scenario = new TextRange(rbScenario.Document.ContentStart, rbScenario.Document.ContentEnd).Text;

                string ULP = JsonConvert.SerializeObject(Lista, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                { ContractResolver = new DynamicContractResolver("ulazn") });

                API.ListaUlaznihParametara = ULP;
                //2************** serializtj po parametru
                string SPULP = JsonConvert.SerializeObject(ListaSpeculparam, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                { ContractResolver = new DynamicContractResolver("s") });

                API.ListaSpecijalizovanihUlaznihParametara = SPULP;
                //3 ************************************
                string TULP = JsonConvert.SerializeObject(ListaTabulparam, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                { ContractResolver = new DynamicContractResolver("ulparam") });
                API.ListaTabelarnihUlaznihParametara = TULP;
                //3 ************************************
                string SGR = JsonConvert.SerializeObject(ListaGreske, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                { ContractResolver = new DynamicContractResolver("g") });
                API.ListaParametaraStatusiGresaka = SGR;

                //4************************
                string IZLVR = JsonConvert.SerializeObject(Listaizlaznihvr, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                { ContractResolver = new DynamicContractResolver("z") });
                API.ListaIzlaznihParametara = IZLVR;

                //********************************
                string SPIZLVR = JsonConvert.SerializeObject(Listaspizlaznihvr, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                { ContractResolver = new DynamicContractResolver("s") });
                API.ListaSpecijalizovanihIzlaznihParametara = SPIZLVR;
                //**********************************
                string DOD = JsonConvert.SerializeObject(Listadodataka, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                { ContractResolver = new DynamicContractResolver("k") });
                API.ListaPazametarazaDodatke = DOD;
                //***********************************

                string TABIZLPARAM = JsonConvert.SerializeObject(listaTabizlparam, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                { ContractResolver = new DynamicContractResolver("izlaz") });
                API.ListaTabelarnihIzlaznihParametara = TABIZLPARAM;
                //**************************************************
                string SARAD = JsonConvert.SerializeObject(Listasaradnika, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                { ContractResolver = new DynamicContractResolver("saradnika") });
                API.ListaSaradnika = SARAD;

                //**************************************************

                string savePath = label1.Content.ToString();
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvano"];
                API.Save(ApiParametri.PutanjaTrenutnoPokrenutogFajla.ToString());
                API.Save(ApiParametri.PutanjaSacuvanogDokumenta.ToString());
                ApiParametri.VidljivostkomandeSave = false;

            }

        }
        #endregion

        #region SINHRONIZACIJA AUTO KOREKTIVNIH LISTI
        private void Sinhronizacija_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Sinhronizacija_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            //statusi greski****************************************************************
            string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string PutanjadoAutoListeTRSG = Convert.ToString(System.IO.Path.Combine(path, "Trenutno.txt"));

            string PutanjadoAutoListeSG = Convert.ToString(System.IO.Path.Combine(path, "AutoListaSTATUSIGRESAKA.txt"));
            List<string> LlistaAutoSG = new List<string>();

            if (!File.Exists(PutanjadoAutoListeTRSG))
            {
                File.Create(PutanjadoAutoListeTRSG);

            }
            string[] trenutnosg = File.ReadAllLines(PutanjadoAutoListeTRSG);

            foreach (string trulp in trenutnosg)
            {
                LlistaAutoSG.Add(trulp);

            }

            foreach (var paramsg in ListaGreske)
            {
                SG.Add(paramsg.Identigikatorgreske + ";" + paramsg.Tipgreske + ";" + paramsg.Opisgreske + ";");
            }

            IEnumerable<string> obeliste11 = SG.Union(LlistaAutoSG);

            File.WriteAllLines(PutanjadoAutoListeTRSG, obeliste11.Distinct().ToArray());
            var fileNameizlvr = PutanjadoAutoListeTRSG;
            FileInfo fiizlvr = new FileInfo(fileNameizlvr);
            float sizeizlvr = fiizlvr.Length / 1024f;
            string VelicinaFajlaizlvr = sizeizlvr.ToString("N") + " KB";
            // MessageBox.Show(VelicinaFajlaizlvr.ToString());

            if (sizeizlvr > 0)
            {
                string[] lines = File.ReadAllLines(PutanjadoAutoListeTRSG);
                File.WriteAllLines(PutanjadoAutoListeSG, lines.Distinct().ToArray());
                File.WriteAllText(PutanjadoAutoListeTRSG, String.Empty);
            }

            //    string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string PutanjadoAutoListeTRTABIZLVR = Convert.ToString(System.IO.Path.Combine(path, "TrenutnoTabelarniIzlazniParametri.txt"));
            string PutanjadoAutoListeATBIZLVR = Convert.ToString(System.IO.Path.Combine(path, "AutoSugestijatabelarniizlazniparametri.txt"));
            List<string> LlistaAutoTABIzlvr = new List<string>();

            string[] trenutnoizlvr = File.ReadAllLines(PutanjadoAutoListeATBIZLVR);
            foreach (string trulp in trenutnoizlvr)
            {
                LlistaAutoTABIzlvr.Add(trulp);

            }

            foreach (var tabizlazniparam in listaTabizlparam)
            {
                TABIZLVR.Add(tabizlazniparam.Poljetabelarniizlazniparam + ";" + tabizlazniparam.Tipizlazniparam + ";" + tabizlazniparam.Duzinapoljaizlazniparam + ";" + tabizlazniparam.Infosysizlazniparam + ";" + tabizlazniparam.Opistabelarniizlazniparam + ";");
            }

            IEnumerable<string> obeliste111 = TABIZLVR.Union(LlistaAutoTABIzlvr);

            File.WriteAllLines(PutanjadoAutoListeTRTABIZLVR, obeliste111.Distinct().ToArray());
            var fileNameizlvr1 = PutanjadoAutoListeTRTABIZLVR;
            FileInfo fiizlvr1 = new FileInfo(fileNameizlvr1);
            float sizeizlvr1 = fiizlvr1.Length / 1024f;
            string VelicinaFajlaizlvr1 = sizeizlvr.ToString("N") + " KB";
            // MessageBox.Show(VelicinaFajlaizlvr.ToString());



            if (sizeizlvr > 0)
            {
                string[] lines = File.ReadAllLines(PutanjadoAutoListeTRTABIZLVR);
                File.WriteAllLines(PutanjadoAutoListeATBIZLVR, lines.Distinct().ToArray());
                File.WriteAllText(PutanjadoAutoListeTRTABIZLVR, String.Empty);


            }


            // string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string PutanjadoAutoListeTRSPECIZLVR = Convert.ToString(System.IO.Path.Combine(path, "TrenutnoSpizlvr.txt"));
            string PutanjadoAutoListeSPECIZLVR = Convert.ToString(System.IO.Path.Combine(path, "AutoListaSpecizlaznevr.txt"));
            List<string> LlistaAutoSpecIzlVr = new List<string>();

            string[] trenutnospizvr = File.ReadAllLines(PutanjadoAutoListeSPECIZLVR);
            foreach (string trulp in trenutnospizvr)
            {
                LlistaAutoSpecIzlVr.Add(trulp);

            }

            foreach (var izlazne in Listaspizlaznihvr)
            {
                SPECIZLAZNEVR.Add(izlazne.Nazivspizlaznevr + ";" + izlazne.TipSpecijalneIzlaznevrednosti + ";" + izlazne.Opisspizlaznevr + ";");
            }

            IEnumerable<string> obeliste0 = SPECIZLAZNEVR.Union(LlistaAutoSpecIzlVr);

            File.WriteAllLines(PutanjadoAutoListeTRSPECIZLVR, obeliste0.Distinct().ToArray());
            var fileNamespizl = PutanjadoAutoListeTRSPECIZLVR;
            FileInfo fispizlvr = new FileInfo(fileNamespizl);
            float sizespiz = fispizlvr.Length / 1024f;
            string VelicinaFajlaspiz = sizespiz.ToString("N") + " KB";
            // MessageBox.Show(VelicinaFajlaspiz.ToString());

            if (sizespiz > 0)
            {
                string[] lines = File.ReadAllLines(PutanjadoAutoListeTRSPECIZLVR);
                File.WriteAllLines(PutanjadoAutoListeSPECIZLVR, lines.Distinct().ToArray());
                File.WriteAllText(PutanjadoAutoListeTRSPECIZLVR, String.Empty);
            }
            else
            {
                // MessageBox.Show("Lista parametara je prazna");
            }

            //   string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string PutanjadoAutoListeTRIZLVR = Convert.ToString(System.IO.Path.Combine(path, "TrenutnoIzlaznevrednosti.txt"));
            string PutanjadoAutoListeIZLVR = Convert.ToString(System.IO.Path.Combine(path, "AutoListaIzlazneVrednosti.txt"));
            List<string> LlistaAutoOzlvr = new List<string>();

            string[] trenutnoii = File.ReadAllLines(PutanjadoAutoListeIZLVR);
            foreach (string trulp in trenutnoii)
            {
                LlistaAutoOzlvr.Add(trulp);

            }

            foreach (var izlazne in Listaizlaznihvr)
            {
                IZLAZNEVR.Add(izlazne.Nazivizlaznevr + ";" + izlazne.Opisizlaznevr + ";");
            }

            IEnumerable<string> obelisteii = IZLAZNEVR.Union(LlistaAutoOzlvr);

            File.WriteAllLines(PutanjadoAutoListeTRIZLVR, obelisteii.Distinct().ToArray());
            var fileNameii = PutanjadoAutoListeTRIZLVR;
            FileInfo fiii = new FileInfo(fileNameii);
            float sizeii = fiii.Length / 1024f;
            string VelicinaFajlaii = sizeii.ToString("N") + " KB";
            //  MessageBox.Show(VelicinaFajlaii.ToString());

            if (sizeii > 0)
            {
                string[] lines = File.ReadAllLines(PutanjadoAutoListeTRIZLVR);
                File.WriteAllLines(PutanjadoAutoListeIZLVR, lines.Distinct().ToArray());
                File.WriteAllText(PutanjadoAutoListeTRIZLVR, String.Empty);
            }
            else
            {
                // MessageBox.Show("Lista parametara je prazna");
            }


            //string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string PutanjadoAutoListeTRTABULP = Convert.ToString(System.IO.Path.Combine(path, "TrenutnoTabelarniUlazniParametri.txt"));
            string PutanjadoAutoListeATBULP = Convert.ToString(System.IO.Path.Combine(path, "AutoListaTabeleanuUlazniparametri.txt"));
            List<string> LlistaAutoTABULp = new List<string>();

            string[] trenutnotup = File.ReadAllLines(PutanjadoAutoListeATBULP);
            foreach (string trulp in trenutnotup)
            {
                LlistaAutoTABULp.Add(trulp);

            }

            foreach (var tabulparam in ListaTabulparam)
            {
                TABULPARAM.Add(tabulparam.Poljetabelarniulparam + ";" + tabulparam.Tipulparam + ";" + tabulparam.Duzinapoljaulparam + ";" + tabulparam.Infosusulparam + ";" + tabulparam.Opistabelarniulparam + ";");
            }

            IEnumerable<string> obelistetup = TABULPARAM.Union(LlistaAutoTABULp);

            File.WriteAllLines(PutanjadoAutoListeTRTABULP, obelistetup.Distinct().ToArray());
            var fileNametup = PutanjadoAutoListeTRTABULP;
            FileInfo fitup = new FileInfo(fileNametup);
            float sizetup = fitup.Length / 1024f;
            string VelicinaFajlatup = sizetup.ToString("N") + " KB";
            //   MessageBox.Show(VelicinaFajlatup.ToString());

            if (sizetup > 0)
            {
                string[] lines = File.ReadAllLines(PutanjadoAutoListeTRTABULP);
                File.WriteAllLines(PutanjadoAutoListeATBULP, lines.Distinct().ToArray());
                File.WriteAllText(PutanjadoAutoListeTRTABULP, String.Empty);
            }
            else
            {
                // MessageBox.Show("Lista parametara je prazna");
            }

            //  string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string PutanjadoAutoListeTRSPECULP = Convert.ToString(System.IO.Path.Combine(path, "TrenutnoSpeculp.txt"));
            string PutanjadoAutoListeSPECULP = Convert.ToString(System.IO.Path.Combine(path, "AutolistaSpeculparam.txt"));
            List<string> LlistaAutoSPECULp = new List<string>();

            string[] trenutnosup = File.ReadAllLines(PutanjadoAutoListeSPECULP);
            foreach (string trulp in trenutnosup)
            {
                LlistaAutoSPECULp.Add(trulp);

            }

            foreach (var item in ListaSpeculparam)
            {
                SPECULP.Add(item.specNazivulazniparametri + ";" + item.specTipulparam + ";" + item.specOpisulazniparam + ";");
            }

            IEnumerable<string> obelistesup = SPECULP.Union(LlistaAutoSPECULp);

            File.WriteAllLines(PutanjadoAutoListeTRSPECULP, obelistesup.Distinct().ToArray());
            var fileNamesup = PutanjadoAutoListeTRSPECULP;
            FileInfo fisup = new FileInfo(fileNamesup);
            float sizesup = fisup.Length / 1024f;
            string VelicinaFajlasup = sizesup.ToString("N") + " KB";
            //   MessageBox.Show(VelicinaFajlasup.ToString());

            if (sizesup > 0)
            {
                string[] lines = File.ReadAllLines(PutanjadoAutoListeTRSPECULP);
                File.WriteAllLines(PutanjadoAutoListeSPECULP, lines.Distinct().ToArray());
                File.WriteAllText(PutanjadoAutoListeTRSPECULP, String.Empty);
            }
            else
            {
                // MessageBox.Show("Lista parametara je prazna");
            }

            // string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string PutanjadoAutoListeTRULP = Convert.ToString(System.IO.Path.Combine(path, "TrenutnoUlazniparametri.txt"));
            string PutanjadoAutoListeULP = Convert.ToString(System.IO.Path.Combine(path, "AutoListaUlazniparametri.txt"));
            List<string> LlistaAutoULp = new List<string>();

            string[] trenutnoup = File.ReadAllLines(PutanjadoAutoListeULP);
            foreach (string trulp in trenutnoup)
            {
                LlistaAutoULp.Add(trulp);

            }

            foreach (var item in Lista)
            {
                ALULP.Add(item.Nazivulazniparametri + ";" + item.Tipulaznogparametra + ";" + item.Duzinaulaznogparametra + ";" + item.Opisulazniparam + ";" + item.Obaveznostulaznogparam + ";" + item.Defaultulaznogparam + ";");
            }

            IEnumerable<string> obeliste1 = ALULP.Union(LlistaAutoULp);

            File.WriteAllLines(PutanjadoAutoListeTRULP, obeliste1.Distinct().ToArray());
            var fileNameup = PutanjadoAutoListeTRULP;
            FileInfo fiup = new FileInfo(fileNameup);
            float sizeup = fiup.Length / 1024f;
            string VelicinaFajlaup = sizeup.ToString("N") + " KB";
            //    MessageBox.Show(VelicinaFajlaup.ToString());

            if (sizeup > 0)
            {
                string[] lines = File.ReadAllLines(PutanjadoAutoListeTRULP);
                File.WriteAllLines(PutanjadoAutoListeULP, lines.Distinct().ToArray());
                File.WriteAllText(PutanjadoAutoListeTRULP, String.Empty);
            }
            else
            {
                //MessageBox.Show("Lista parametara je prazna");
            }

            ApiParametri.SinhroniyacijaListi = true;

            try
            {
                string path1 = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string PutanjadoAutoListeulp = Convert.ToString(System.IO.Path.Combine(path1, "AutoListaUlazniparametri.txt"));

                string[] readText1 = File.ReadAllLines(PutanjadoAutoListeulp);
                ALULP.Clear();
                foreach (string sg in readText1)
                {
                    ALULP.Add(sg);

                }

                txtNazivulparam.TextChanged += txtNazivulparam_TextChanged;
            }
            catch (Exception)
            {

                return;
            }

            try
            {
                //string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string PutanjadoAutoListespeculp = Convert.ToString(System.IO.Path.Combine(path, "AutolistaSpeculparam.txt"));

                string[] readText1 = File.ReadAllLines(PutanjadoAutoListespeculp);
                SPECULP.Clear();
                foreach (string sg in readText1)
                {
                    SPECULP.Add(sg);

                }
                txtNazivspeculparam.TextChanged += txtNazivspeculparam_TextChanged;
            }
            catch (Exception)
            {

                return;
            }

            try
            {
                //  string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string PutanjadoAutoListetabulparam = Convert.ToString(System.IO.Path.Combine(path, "AutoListaTabeleanuUlazniparametri.txt"));
                string[] Procitajtext = File.ReadAllLines(PutanjadoAutoListetabulparam);
                TABULPARAM.Clear();
                foreach (string item in Procitajtext)
                {
                    TABULPARAM.Add(item);
                }
                txtPoletabulparam.TextChanged += txtPoletabizlazniparam_TextChanged;
            }
            catch (Exception)
            {

                return;
            }


            try
            {
                //  string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string PutanjadoAutoListeizlaznevr = Convert.ToString(System.IO.Path.Combine(path, "AutoListaIzlazneVrednosti.txt"));
                string[] Procitajtext = File.ReadAllLines(PutanjadoAutoListeizlaznevr);
                IZLAZNEVR.Clear();
                foreach (string item in Procitajtext)
                {
                    IZLAZNEVR.Add(item);
                }
            }
            catch (Exception)
            {

                return;
            }

            try
            {
                // string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string PutanjadoAutoListespecizlaznevr = Convert.ToString(System.IO.Path.Combine(path, "AutoListaSpecizlaznevr.txt"));
                string[] Procitajtext = File.ReadAllLines(PutanjadoAutoListespecizlaznevr);
                SPECIZLAZNEVR.Clear();
                foreach (string item in Procitajtext)
                {
                    SPECIZLAZNEVR.Add(item);
                }
            }
            catch (Exception)
            {

                return;
            }

            try
            {
                // string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string PutanjadoAutoListetabizlazniparam = Convert.ToString(System.IO.Path.Combine(path, "AutoSugestijatabelarniizlazniparametri.txt"));
                string[] Procitajtext = File.ReadAllLines(PutanjadoAutoListetabizlazniparam);
                TABIZLVR.Clear();
                foreach (string item in Procitajtext)
                {
                    TABIZLVR.Add(item);
                }
                txtPoletabizlazniparam.TextChanged += txtPoletabizlazniparam_TextChanged;
            }
            catch (Exception)
            {

                return;
            }

            try
            {
                string path1 = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string PutanjadoAutoListesg = Convert.ToString(System.IO.Path.Combine(path1, "AutoListaSTATUSIGRESAKA.txt"));

                string[] readText1 = File.ReadAllLines(PutanjadoAutoListesg);
                SG.Clear();
                foreach (string sg in readText1)
                {
                    SG.Add(sg);

                }

                txtIdentifgreske.TextChanged += txtIdentifgreske_TextChanged;
            }
            catch (Exception)
            {

                return;
            }
            try
            {
                //  string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string PutanjadoAutoListeizlaznevr = Convert.ToString(System.IO.Path.Combine(path, "AutoListaIzlazneVrednosti.txt"));
                string[] Procitajtext = File.ReadAllLines(PutanjadoAutoListeizlaznevr);
                IZLAZNEVR.Clear();
                foreach (string item in Procitajtext)
                {
                    IZLAZNEVR.Add(item);
                }


            }
            catch (Exception)
            {

                return;
            }
        }
        //**********************************************************************************
        #endregion


        #region KOMANDA NEW
        private void New_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            IsptazniFormu();
            ApiParametri.VidljivostkomandeSave = false;
            StanjeSaveAsKomande = true;
            btnsaveas.IsEnabled = true;
            ucitanfajl = true;
            Sinhronizacija_Executed(null, null);
        }

        #endregion

        #region KOMANDA SKUPI EXPANDERE
        private void Skupiexp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Skupiexp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Opisfunkcije.IsExpanded = false;
            ulazniparametri.IsExpanded = false;
            Scenario.IsExpanded = false;
            Spulparam.IsExpanded = false;
            Tabulparam.IsExpanded = false;
            Statusigreski.IsExpanded = false;
            Izlaznevrednosti.IsExpanded = false;
            Specijalizovaneizlaznevr.IsExpanded = false;
            Tabelarneizlaznevr.IsExpanded = false;
            dodaci.IsExpanded = false;



        }

        #endregion

        #region KOMANDA PRIKAZI EKSPANDERE
        private void Prikaziiexp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Prikaziiexp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Opisfunkcije.IsExpanded = true;
            ulazniparametri.IsExpanded = true;
            Scenario.IsExpanded = true;
            Spulparam.IsExpanded = true;
            Tabulparam.IsExpanded = true;
            Statusigreski.IsExpanded = true;
            Izlaznevrednosti.IsExpanded = true;
            Specijalizovaneizlaznevr.IsExpanded = true;
            Tabelarneizlaznevr.IsExpanded = true;
            dodaci.IsExpanded = true;
        }

        #endregion

        #region KOMANDA OTKLJUCAJ KONTROLE ZA IZMENE
        private void OtkljucajSve_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OtkljucajSve_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Otkljucano.IsChecked = true;
            Otkljucanospulp.IsChecked = true;
            Otkljucanotabulp.IsChecked = true;
            rbOtkljucajstatus.IsChecked = true;
            rbotkljucanoizlazvr.IsChecked = true;
            Otkljucanjspizlvr.IsChecked = true;
            Otkljucanotabizlaznip.IsChecked = true;
            OtkljucajDodatak.IsChecked = true;


            Otkljucano.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Otkljucanospulp.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Otkljucanotabulp.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            rbOtkljucajstatus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            rbotkljucanoizlazvr.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Otkljucanjspizlvr.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Otkljucanotabizlaznip.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            OtkljucajDodatak.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
        #endregion

        #region DODAVANJE PARAMETARA

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string pathULP = Path.Combine(path, "TrenutnoUlazniparametri.txt");
            CUIParam ulparam = new CUIParam();
            ulparam.Nazivulazniparametri = txtNazivulparam.Text;
            ulparam.Opisulazniparam = new TextRange(rbOpisulp.Document.ContentStart, rbOpisulp.Document.ContentEnd).Text;
            ulparam.Defaultulaznogparam = txtdefulparam.Text;
            ulparam.Obaveznostulaznogparam = txtobavulparam.Text;

            ulparam.Tipulaznogparametra = txtTipUlaznogparametra.Text;
            ulparam.Duzinaulaznogparametra = txtDuzinaUlaznogparametra.Text;
            List<string> trulp = new List<string>();
            foreach (var item in Lista)
            {
                trulp.Add(item.Nazivulazniparametri + " " + ";" + item.Tipulaznogparametra + " " + ";" + item.Duzinaulaznogparametra + " " + ";" + item.Opisulazniparam + " " + ";" + item.Obaveznostulaznogparam + " " + ";" + item.Defaultulaznogparam + " " + ";");
            }

            File.WriteAllText(pathULP, String.Empty);
            File.WriteAllLines(pathULP, trulp);


            Lista.Add(ulparam);
            CIzvestaj.listaparametaraizv.Add(ulparam);


            dgulparam.ItemsSource = Lista;
            txtNazivulparam.Clear();
            rbOpisulp.Document.Blocks.Clear();
            txtdefulparam.Clear();
            txtobavulparam.Clear();
            txtTipUlaznogparametra.Clear();
            txtDuzinaUlaznogparametra.Clear();

            Da.IsChecked = false;
            Ne.IsChecked = false;
            txtNazivulparam.Focus();
            ApiParametri.SinhroniyacijaListi = true;

            if (ApiParametri.SinhroniyacijaListi == true)
            {
                Sinhronizacija_Executed(null, null);
            }


        }

        private void btnDodajSpulparam_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string pathspecULP = Path.Combine(path, "TrenutnoSpeculp.txt");
            CSpecUlparam spulparam = new CSpecUlparam();
            spulparam.specNazivulazniparametri = txtNazivspeculparam.Text;
            spulparam.specOpisulazniparam = new TextRange(rbOpisSpeculparam.Document.ContentStart, rbOpisSpeculparam.Document.ContentEnd).Text;
            spulparam.specDefaultulaznogparam = txtDefspulparam.Text;
            spulparam.specObaveznostulaznogparam = txtObavezSpeculparam.Text;
            spulparam.specTipulparam = txtTipspeculparam.Text;
            ListaSpeculparam.Add(spulparam);
            CIzvestaj.listaSpeculparam.Add(spulparam);
            dgSpeculparam.ItemsSource = ListaSpeculparam;
            List<string> tr = new List<string>();
            foreach (var item in ListaSpeculparam)
            {
                tr.Add(item.specNazivulazniparametri + ";" + item.specTipulparam + ";" + item.specOpisulazniparam + ";");
            }

            File.WriteAllText(pathspecULP, String.Empty);
            File.WriteAllLines(pathspecULP, tr);




            txtNazivspeculparam.Clear();
            rbOpisSpeculparam.Document.Blocks.Clear();
            txtDefspulparam.Clear();
            txtObavezSpeculparam.Clear();
            txtTipspeculparam.Clear();
            cbspulpne.IsChecked = false;
            cbspulpda.IsChecked = false;

            ApiParametri.PagingSelectedPage = true;

            ApiParametri.SinhroniyacijaListi = true;
            if (ApiParametri.SinhroniyacijaListi == true)
            {
                Sinhronizacija_Executed(null, null);
            }
            txtNazivspeculparam.Focus();

        }



        private void btnDodajtabulparam_Click(object sender, RoutedEventArgs e)
        {

            string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string pathspectabULP = Path.Combine(path, "TrenutnoTabelarniUlazniParametri.txt");

            CTabUlparam tabulparam = new CTabUlparam();
            tabulparam.Nazivtabelarniulparam = txtNazivtabulparam.Text;

            tabulparam.Tipulparam = txttipolparam.Text;
            tabulparam.Poljetabelarniulparam = txtPoletabulparam.Text;
            tabulparam.Duzinapoljaulparam = txtduztabulparam.Text;
            tabulparam.Opistabelarniulparam = new TextRange(rbOpistabulparam.Document.ContentStart, rbOpistabulparam.Document.ContentEnd).Text;
            tabulparam.Infosusulparam = txtInfosys.Text;
            ListaTabulparam.Add(tabulparam);

            dgTabulparam.ItemsSource = ListaTabulparam;

            List<string> tr = new List<string>();
            foreach (var item in ListaTabulparam)
            {
                tr.Add(item.Poljetabelarniulparam + ";" + item.Tipulparam + ";" + item.Duzinapoljaulparam + ";" + item.Infosusulparam + ";" + item.Opistabelarniulparam + ";");
            }

            File.WriteAllText(pathspectabULP, String.Empty);
            File.WriteAllLines(pathspectabULP, tr);

            txttipolparam.Clear();
            txtPoletabulparam.Clear();
            txtduztabulparam.Clear();
            rbOpistabulparam.Document.Blocks.Clear();
            txtInfosys.Clear();
            txtPoletabulparam.Focus();

            ApiParametri.SinhroniyacijaListi = true;
            if (ApiParametri.SinhroniyacijaListi == true)
            {
                Sinhronizacija_Executed(null, null);
            }

        }

        //********************************************************************************

        private void btnDodajizlvr_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string pathizlvr = Path.Combine(path, "TrenutnoIzlaznevrednosti.txt");

            CIzlaznevr izlazvr = new CIzlaznevr();
            izlazvr.Nazivizlaznevr = txtIzlvrn.Text;
            izlazvr.Opisizlaznevr = new TextRange(rbopisizlvr.Document.ContentStart, rbopisizlvr.Document.ContentEnd).Text;
            Listaizlaznihvr.Add(izlazvr);
            CIzvestaj.Listaizlaznevr.Add(izlazvr);
            dgIzlaznevr.ItemsSource = Listaizlaznihvr;

            List<string> tr = new List<string>();
            foreach (var item in Listaizlaznihvr)
            {
                tr.Add(item.Nazivizlaznevr + ";" + item.Opisizlaznevr + ";");
            }

            File.WriteAllText(pathizlvr, String.Empty);
            File.WriteAllLines(pathizlvr, tr);

            txtIzlvrn.Clear();
            rbopisizlvr.Document.Blocks.Clear();
            txtIzlvrn.Focus();

            ApiParametri.SinhroniyacijaListi = true;
            if (ApiParametri.SinhroniyacijaListi == true)
            {
                Sinhronizacija_Executed(null, null);
            }
        }

        #endregion

        #endregion

            #region ISPRAZNI FORMU
        public void IsptazniFormu()
        {
            txtDuzinaUlaznogparametra.Clear();
            txtIme.Clear();


            txtTipspeculparam.Clear();
            txtTipspecizlaznevr.Clear();
            txtnazivfunkcija.Clear();
            txtModul.Clear();
            txtModul.Clear();
            rbOpistabizlazniparam.Document.Blocks.Clear();
            rbDodatakNapomena.Document.Blocks.Clear();
            rbOpisfunkcije.Document.Blocks.Clear();
            rbopisLogikefunkcije.Document.Blocks.Clear();
            rbOgrucestfunkc.Document.Blocks.Clear();
            rbScenario.Document.Blocks.Clear();
            Tabitem.Header = txtnazivfunkcija.Text;

            txtNazivulparam.Clear();
            rbOpisulp.Document.Blocks.Clear();
            txtdefulparam.Clear();

            txtTipUlaznogparametra.Clear();
            Lista.Clear();

            txtNazivspeculparam.Clear();
            rbOpisSpeculparam.Document.Blocks.Clear();
            txtDefspulparam.Clear();
            ListaSpeculparam.Clear();
            //tabekarniulparam
            txtNazivtabulparam.Clear();
            txtPoletabulparam.Clear();
            txttipolparam.Clear();
            txtduztabulparam.Clear();
            txtInfosys.Clear();
            rbOpistabulparam.Document.Blocks.Clear();
            ListaTabulparam.Clear();
            //greske
            txtIdentifgreske.Clear();
            txtTiogreske.Clear();
            rbopisgreske.Document.Blocks.Clear();
            ListaGreske.Clear();
            //izlaznevr
            txtIzlvrn.Clear();
            rbopisizlvr.Document.Blocks.Clear();
            Listaizlaznihvr.Clear();
            txtnazspizlvr.Clear();
            txtDefizlazvr.Clear();
            rbspizlvr.Document.Blocks.Clear();
            Listaspizlaznihvr.Clear();
            //tabizlvr
            txtNazivtabizlazniparam.Clear();
            txtPoletabizlazniparam.Clear();
            txttipizlazniparam.Clear();
            txtduztabizlazniparam.Clear();
            txttzlaznuInfosys.Clear();
            Listaspizlaznihvr.Clear();
            //dodaak
            txtNazivDodatka.Clear();
            txtTipDodatka.Clear();
            rbOpisdodatka.Document.Blocks.Clear();
            Listadodataka.Clear();
            listaTabizlparam.Clear();


        }
        #endregion

            #region KOMANDA LOAD
        private void Load_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Load_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CIniFile Ini33 = new CIniFile(ApiParametri.PutanjadoInifajla);
            ApiParametri.PutanjaTrenutnoPokrenutogFajla = Ini33.Read("PutanjaTrenutnoPokrenutogFajla", "ApiDocument");
            ApiParametri.Sacuvano = true;
            string extenzijafajla = System.IO.Path.GetExtension(ApiParametri.PutanjaTrenutnoPokrenutogFajla.ToString());
            //ApiParametri.Extensija = System.IO.Path.GetExtension(ApiParametri.PutanjaSacuvanogDokumenta.ToString());




            if (extenzijafajla == ".afd")
            {


                string strfilename = ApiParametri.PutanjaTrenutnoPokrenutogFajla.ToString();
                // string strfilename = txtpath.Text;

                CApiDoc API = CApiDoc.LoadFromFile(strfilename);
                ApiParametri.VidljivostkomandeSave = false;

                txtnazivfunkcija.Background = Brushes.White;

                txtModul.TextChanged -= txtModul_TextChanged;
                txtModul.BorderBrush = Brushes.Blue;
                txtModul.BorderThickness = new Thickness(0.5);

                //deserializuj listu
                try
                {
                    string ULP1 = API.ListaUlaznihParametara;
                    var UlazniParametri = JsonConvert.DeserializeObject<IEnumerable<CUIParam>>(ULP1);


                    foreach (var Deserialprop in UlazniParametri)
                    {

                        txtNazivulparam.Text = Deserialprop.Nazivulazniparametri;

                        rbOpisulp.AppendText(Deserialprop.Opisulazniparam);
                        txtdefulparam.Text = Deserialprop.Obaveznostulaznogparam;
                        txtobavulparam.Text = Deserialprop.Defaultulaznogparam;
                        txtDuzinaUlaznogparametra.Text = Deserialprop.Duzinaulaznogparametra;
                        txtTipUlaznogparametra.Text = Deserialprop.Tipulaznogparametra;
                        Lista.Add(Deserialprop);

                    }
                    if (dgulparam.Items.Count > 0)
                    {
                        ulazniparametri.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    else
                    {
                        ulazniparametri.Foreground = new SolidColorBrush(Colors.Red);
                    }

                }
                catch (Exception)
                {

                    //   MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                    //    " " + "Proverite  listu ulaznih parametara");
                    CIniFile ini = new CIniFile(ApiParametri.PutanjadoInifajla);
                    ini.DeleteKey("PutanjaTrenutnoPokrenutogFajla", "ApiDocument");

                }

                ////////
                txtNazivulparam.Clear();
                rbOpisulp.Document.Blocks.Clear();
                txtdefulparam.Clear();
                txtobavulparam.Clear();
                txtTipUlaznogparametra.Clear();
                txtDuzinaUlaznogparametra.Clear();


                dgulparam.ItemsSource = Lista;
                if (dgulparam.Items.Count > 0)
                {
                    ulazniparametri.Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    ulazniparametri.Foreground = new SolidColorBrush(Colors.Red);
                }

                //2*******************************
                try
                {
                    string SPULP1 = API.ListaSpecijalizovanihUlaznihParametara;

                    var SpecUlazniParametri = JsonConvert.DeserializeObject<IEnumerable<CSpecUlparam>>(SPULP1);
                    foreach (var Deserialspulp in SpecUlazniParametri)
                    {

                        txtNazivspeculparam.Text = Deserialspulp.specNazivulazniparametri;
                        rbOpisSpeculparam.AppendText(Deserialspulp.specOpisulazniparam);
                        txtObavezSpeculparam.Text = Deserialspulp.specObaveznostulaznogparam;
                        txtDefspulparam.Text = Deserialspulp.specDefaultulaznogparam;
                        ListaSpeculparam.Add(Deserialspulp);

                    }
                    if (dgSpeculparam.Items.Count > 1)
                    {
                        Spulparam.Foreground = new SolidColorBrush(Colors.Black);

                    }
                    else
                    {
                        Spulparam.Foreground = new SolidColorBrush(Colors.Red);
                    }

                }
                catch (Exception)
                {

                    //MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                    //    " " + "Proverite  listu specijalizovanih ulaznih parametara");
                }

                txtNazivspeculparam.Clear();
                rbOpisSpeculparam.Document.Blocks.Clear();
                txtObavezSpeculparam.Clear();
                txtDefspulparam.Clear();

                dgSpeculparam.ItemsSource = ListaSpeculparam;
                /////////

                if (dgSpeculparam.Items.Count > 0)
                {
                    Spulparam.Foreground = new SolidColorBrush(Colors.Black);

                }
                else
                {
                    Spulparam.Foreground = new SolidColorBrush(Colors.Red);
                }

                //**************************

                try
                {
                    string TULP1 = API.ListaTabelarnihUlaznihParametara;
                    var Tabelarniularametri = JsonConvert.DeserializeObject<IEnumerable<CTabUlparam>>(TULP1);
                    foreach (var Deserialtulp1 in Tabelarniularametri)
                    {
                        txtNazivtabulparam.Text = Deserialtulp1.Nazivtabelarniulparam;
                        txtPoletabulparam.Text = Deserialtulp1.Poljetabelarniulparam;
                        txtdefulparam.Text = Deserialtulp1.Tipulparam;
                        txtduztabulparam.Text = Deserialtulp1.Duzinapoljaulparam;
                        rbOpistabulparam.AppendText(Deserialtulp1.Opistabelarniulparam);
                        txtInfosys.Text = Deserialtulp1.Infosusulparam;
                        ListaTabulparam.Add(Deserialtulp1);

                    }
                    ValidacijanaExp();


                }
                catch (Exception)
                {

                    //MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                    //    " " + "Proverite  listu tabelarnih ulaznih parametara");
                }


                //////////

                txtNazivtabulparam.Clear();
                txtPoletabulparam.Clear();
                txtdefulparam.Clear();
                txtduztabulparam.Clear();
                rbOpistabulparam.Document.Blocks.Clear();
                txtInfosys.Clear();
                dgTabulparam.ItemsSource = ListaTabulparam;
                ValidacijanaExp();
                //******************************



                try
                {
                    string SGR1 = API.ListaParametaraStatusiGresaka;
                    var StatGr = JsonConvert.DeserializeObject<IEnumerable<CStatusGreske>>(SGR1);
                    foreach (var Desergreske in StatGr)
                    {
                        txtIdentifgreske.Text = Desergreske.Identigikatorgreske;
                        txtTiogreske.Text = Desergreske.Tipgreske;
                        rbopisgreske.AppendText(Desergreske.Opisgreske);
                        ListaGreske.Add(Desergreske);
                    }
                    ValidacijanaExp();
                }
                catch (Exception)
                {

                    //  MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                    //      " " + "Proverite  listu statusa greski");
                }

                dgstatusgreske.ItemsSource = ListaGreske;
                ValidacijanaExp();

                ///////
                txtIdentifgreske.Clear();
                txtTiogreske.Clear();
                rbopisgreske.Document.Blocks.Clear();
                //****************************
                try
                {
                    string IZLVR1 = API.ListaIzlaznihParametara;
                    var Izlazvrednost = JsonConvert.DeserializeObject<IEnumerable<CIzlaznevr>>(IZLVR1);
                    foreach (var Deserizlvr in Izlazvrednost)
                    {
                        txtIzlvrn.Text = Deserizlvr.Nazivizlaznevr;
                        rbopisizlvr.AppendText(Deserizlvr.Opisizlaznevr);

                        Listaizlaznihvr.Add(Deserizlvr);


                    }
                    ValidacijanaExp();
                    Validacijaexpopis();
                    ValidacijaScenarioexp();
                }
                catch (Exception)
                {
                    //    MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                    //      " " + "Proverite  listu izlaznih parametara");

                }
                dgIzlaznevr.ItemsSource = Listaizlaznihvr;
                //////////
                txtIzlvrn.Clear();

                rbopisizlvr.Document.Blocks.Clear();
                ValidacijanaExp();
                Validacijaexpopis();
                ValidacijaScenarioexp();
                //**********************************************
                try
                {
                    string SPIZLVR1 = API.ListaSpecijalizovanihIzlaznihParametara;
                    var spIzlazvrednost = JsonConvert.DeserializeObject<IEnumerable<CSpizlazvr>>(SPIZLVR1);
                    foreach (var Deserspizlvr in spIzlazvrednost)
                    {
                        txtnazspizlvr.Text = Deserspizlvr.Nazivspizlaznevr;
                        txtTipspecizlaznevr.Text = Deserspizlvr.TipSpecijalneIzlaznevrednosti.ToString();

                        rbspizlvr.AppendText(Deserspizlvr.Opisspizlaznevr);


                        Listaspizlaznihvr.Add(Deserspizlvr);
                    }
                    ValidacijanaExp();
                    Validacijaexpopis();
                    ValidacijaScenarioexp(); ;

                }
                catch (Exception)
                {

                    //  MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                    //     " " + "Proverite  listu specijalizovanih izlaznih parametara");
                }
                dgspizlazvre.ItemsSource = Listaspizlaznihvr;
                ////////
                txtnazspizlvr.Clear();
                rbspizlvr.Document.Blocks.Clear();

                //*************************************
                try
                {
                    string DOD1 = API.ListaPazametarazaDodatke;
                    var propdodatka = JsonConvert.DeserializeObject<IEnumerable<CDodatak>>(DOD1);
                    foreach (var Deserdod in propdodatka)
                    {
                        txtNazivDodatka.Text = Deserdod.Dodataknaziv;
                        txtTipDodatka.Text = Deserdod.Dodataktip;
                        rbOpisdodatka.AppendText(Deserdod.Opisdodatka);
                        Listadodataka.Add(Deserdod);
                    }
                    //////////
                    dgDodatak.ItemsSource = Listadodataka;
                    txtNazivDodatka.Clear();
                    txtTipDodatka.Clear();
                    rbOpisdodatka.Document.Blocks.Clear();
                    ValidacijanaExp();
                    Validacijaexpopis();
                    ValidacijaScenarioexp();

                }
                catch (Exception)
                {
                    return;
                }
                //****************************************

                try
                {
                    string TABIZLPARAM1 = API.ListaTabelarnihIzlaznihParametara;
                    var propptabizlaznevr = JsonConvert.DeserializeObject<IEnumerable<CTabIzlazniParametri>>(TABIZLPARAM1);
                    foreach (var Desertabizlvr in propptabizlaznevr)
                    {
                        txtNazivtabizlazniparam.Text = Desertabizlvr.Nazivtabelarniizlazniparam;
                        txtPoletabizlazniparam.Text = Desertabizlvr.Poljetabelarniizlazniparam;
                        txttipizlazniparam.Text = Desertabizlvr.Tipizlazniparam;
                        txtduztabizlazniparam.Text = Desertabizlvr.Duzinapoljaizlazniparam;
                        txttzlaznuInfosys.Text = Desertabizlvr.Infosysizlazniparam;
                        rbOpistabizlazniparam.AppendText(Desertabizlvr.Opistabelarniizlazniparam);
                        listaTabizlparam.Add(Desertabizlvr);
                    }
                    ValidacijanaExp();
                    Validacijaexpopis();
                    ValidacijaScenarioexp(); ;

                }
                catch (Exception)
                {

                    //   MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                    //         " " + "Proverite  listutabelarnih izlaznih parametara");
                }
                ////////////////////////////////
                dgTabizlazniparam.ItemsSource = listaTabizlparam;
                txtNazivtabizlazniparam.Clear();
                txtPoletabizlazniparam.Clear();
                txttipizlazniparam.Clear();
                txtduztabizlazniparam.Clear();
                txttzlaznuInfosys.Clear();
                rbOpistabizlazniparam.Document.Blocks.Clear();

                //****************************************
                //Deserijalizuj listu saradnika
                try
                {
                    string SARAD1 = API.ListaSaradnika;
                    var svisaradnici = JsonConvert.DeserializeObject<IEnumerable<CSaradnici>>(SARAD1);
                    foreach (var Deserialsarad in svisaradnici)
                    {

                        txtIme.Text = Deserialsarad.Sifrasaradnika;

                        Listasaradnika.Add(Deserialsarad);

                    }
                    ValidacijanaExp();
                    Validacijaexpopis();
                    ValidacijaScenarioexp();

                }
                catch (Exception)
                {

                    MessageBox.Show("Fajl koji ste ucitali" + " " + API.NazivApiFunkcije.ToString() + ".afd" + " " + " ne sadrzi odgovarajuce parametre" +
                       " " + "Proverite  listu specijalizovanih ulaznih parametara");
                }

                dgSarad.ItemsSource = Listasaradnika;
                /////////

                txtIme.Clear();
                //***************************************


                txtModul.Text = API.Modul;
                txtnazivfunkcija.Text = API.NazivApiFunkcije;
                rbOpisfunkcije.AppendText(API.Opisfunkcije);
                rbopisLogikefunkcije.AppendText(API.Opislogikefunkcije);
                rbOgrucestfunkc.AppendText(API.Ogrucestalostifunkcija);
                rbScenario.AppendText(API.Scenario);
                ApiParametri.VidljivostkomandeSave = false;
                ApiParametri.Sacuvano = false;
                Ini33.DeleteKey("PutanjaTrenutnoPokrenutogFajla", "ApiDocument");

                ValidacijanaExp();
                Validacijaexpopis();
                ValidacijaScenarioexp();
            }

            else
            {
                try
                {
                    Process.Start(txtpath.Text);
                }
                catch (Exception)
                {

                    return;
                }
                frmInifile inufile = new frmInifile();
                try
                {
                    if (inufile.listBox.Items != null)
                    {
                        inufile.listBox.Items.Clear();
                    }
                    // string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                    string ProjectPath = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                    string foldername = System.IO.Path.Combine(ProjectPath, "Konfiguracija");

                    DirectoryInfo direktorijumi = new DirectoryInfo(foldername);

                    FileInfo[] Inifajlovi = direktorijumi.GetFiles("*.ini");

                    foreach (FileInfo svifajlovi in Inifajlovi)
                    {
                        inufile.listBox.Items.Add(svifajlovi);
                    }
                }
                catch (Exception)
                {

                    return;
                }

            }
        }

        #endregion

            #region WINDOW LOADED
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            if (string.IsNullOrWhiteSpace(txtModul.Text))
            {
                txtModul.BorderBrush = Brushes.Red;
                txtModul.BorderThickness = new Thickness(1);
            }
            else
            {
                txtModul.BorderBrush = Brushes.Black;
                txtModul.BorderThickness = new Thickness(1);
            }


            CIniFile Ini = new CIniFile(ApiParametri.PutanjadoInifajla.ToString());
            string projectPath1 = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);

            string dirgdejeobjavljeno = Ini.Read("Direktorijumzaobjavljenefunkcije", "ApiDocument");

           
            Sinhronizacija_Executed(null, null);
            Zaarhivu();
            ProveraIniParametarra();
            this.Load_Executed(null, null);
            Tabitem.Header = txtnazivfunkcija.Text;

            #region UCUTAVANJE PARAMETARA U COMBOBOX TIP DODATKA

            cbTipdodatak.Items.Add(ApiParametri.SlikaJPG.ToString());
            cbTipdodatak.Items.Add(ApiParametri.SlikaPNG.ToString());
            cbTipdodatak.Items.Add(ApiParametri.SlikaGif.ToString());
            cbTipdodatak.Items.Add(ApiParametri.Text.ToString());
            cbTipdodatak.Items.Add(ApiParametri.ZIP.ToString());
            if (ApiParametri.Ext1.Length > 2)
            {
                cbTipdodatak.Items.Add(ApiParametri.Ext1.ToString());
            }
            if (ApiParametri.Ext2.Length > 2)
            {
                cbTipdodatak.Items.Add(ApiParametri.Ext2.ToString());
            }

            if (ApiParametri.Ext3.Length > 2)
            {
                cbTipdodatak.Items.Add(ApiParametri.Ext3.ToString());
            }
            if (ApiParametri.Ext4.Length > 2)
            {
                cbTipdodatak.Items.Add(ApiParametri.Ext4.ToString());
            }
            if (ApiParametri.Ext5.Length > 2)
            {
                cbTipdodatak.Items.Add(ApiParametri.Ext5.ToString());
            }
            if (ApiParametri.Ext6.Length > 2)
            {
                cbTipdodatak.Items.Add(ApiParametri.Ext6.ToString());
            }


            #endregion

            #region FORMATIRANJE GRIDA
            //************************************************
            dgSarad.AutoGenerateColumns = false;
            DataGridTextColumn dgsarad = new DataGridTextColumn();
            dgsarad.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            dgsarad.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            dgsarad.Header = "ŠIFRA SARADNIKA";
            dgsarad.Binding = new Binding("Sifrasaradnika");

            dgSarad.Columns.Add(dgsarad);




            //************************************************
            #region POMOCNI MENI

            MenuItem cmdDodajsaradnika = new MenuItem();
            cmdDodajsaradnika.Header = "Dodaj parametar";
            cmdDodajsaradnika.Click += CmdDodajsaradnika_Click;


            ///////////////////////////////////////////////
            MenuItem cmdDeletesarad = new MenuItem();
            cmdDeletesarad.Header = "Delete";
            cmdDeletesarad.Click += CmdDeletesarad_Click;

            //////////////////////////////////////////////////
            MenuItem cmdSaradZatvoriformu = new MenuItem();
            cmdSaradZatvoriformu.Header = "Zatvori formu";
            cmdSaradZatvoriformu.Click += CmdSaradZatvoriformu_Click;



            /////////////////////////////////////////////////


            MenuItem cmdPrikaziformu = new MenuItem();
            cmdPrikaziformu.Header = "Prikazi formu";
            cmdPrikaziformu.Click += CmdPrikaziformu_Click; ;


            ///////////////////////////////////////
            ContextMenu cmsarad = new ContextMenu();

            cmsarad.Items.Add(cmdDodajsaradnika);
            cmsarad.Items.Add(cmdDeletesarad);
            cmsarad.Items.Add(cmdSaradZatvoriformu);
            cmsarad.Items.Add(cmdPrikaziformu);
            dgSarad.ContextMenu = cmsarad;
            #endregion

            #endregion

            #region FORSIRANI PARAMETRI

            if (ApiParametri.ForsiraniParametri == "True")
            {

                if (lscenarioforsiran == true)
                {

                    Scenario.Header = "SCENARIO *OBAVEZNO POLJE*";

                    Scenario.Foreground = new SolidColorBrush(Colors.Red);
                    


                }
                else
                {
                    Scenario.Header = "SCENARIO ";
                    Scenario.Foreground = new SolidColorBrush(Colors.Black);
                    
                }

                if (lforsiraniparametarmodul == true)
                {


                    Opisfunkcije.Header = "OPIS FUNKCIJE *OBAVEZNO POLJE*";
                    Opisfunkcije.Foreground = new SolidColorBrush(Colors.Red);
                    
                }
                else
                {
                    Opisfunkcije.Header = "OPIS FUNKCIJE ";
                    Opisfunkcije.Foreground = new SolidColorBrush(Colors.Black);
                   
                }
                if (lForsiraniparametarOpisfunkcije == true)
                {
                    Opisfunkcije.Header = "OPIS FUNKCIJE *OBAVEZNO POLJE*";
                    Opisfunkcije.Foreground = new SolidColorBrush(Colors.Red);
                   
                  
                }
                else
                {
                    Opisfunkcije.Header = "OPIS FUNKCIJE ";
                    Opisfunkcije.Foreground = new SolidColorBrush(Colors.Black);
                  
                }
                if (lForsiraniparametriOpislogike == true)
                {
                    Opisfunkcije.Header = "OPIS FUNKCIJE *OBAVEZNO POLJE*";
                    Opisfunkcije.Foreground = new SolidColorBrush(Colors.Red);
                  
                }
                else
                {
                    Opisfunkcije.Header = "OPIS FUNKCIJE ";
                    Opisfunkcije.Foreground = new SolidColorBrush(Colors.Black);
                   
                }

                if (lForsiraniparametriimesaradnika == true)
                {
                    saradnici.Header = "SARADNICI *OBAVEZNO POLJE*";
                    saradnici.Foreground = new SolidColorBrush(Colors.Red);
                   
                }
                else
                {
                    saradnici.Header = "SARADNICI ";
                    saradnici.Foreground = new SolidColorBrush(Colors.Black);
                 
                }

            }
            else
            {
                lscenarioforsiran = false;
                lforsiraniparametarmodul = false;
                lForsiraniparametarOpisfunkcije = false;
                lForsiraniparametriOpislogike = false;
                lForsiraniparametriimesaradnika = false;
            }



            #endregion

            #region Explorer prikaz

            bool explorer = true;

            if (explorer == true)
            {

                folders.Visibility = Visibility.Hidden;
                Favorites.Visibility = Visibility.Hidden;
                txtpath.Visibility = Visibility.Hidden;
                btnpath.Visibility = Visibility.Hidden;
                treeView.Margin = new Thickness(0, 0, 0, 0);
                treeView.Visibility = Visibility.Hidden;
                expander.Visibility = Visibility.Hidden;
                tabkontrola.Margin = new Thickness(20, 100, 0, 0);
            }
            else
            {

                folders.Visibility = Visibility.Visible;
                Favorites.Visibility = Visibility.Visible;
                txtpath.Visibility = Visibility.Visible;
                btnpath.Visibility = Visibility.Visible;
                treeView.Margin = new Thickness(0, 400, 0, 0);
                treeView.Visibility = Visibility.Visible;
                expander.Visibility = Visibility.Visible;

            }

            #endregion

            #region FORMATIRANJE GRODA TABELARNI IZLAZNI PARAMETRI

            dgTabizlazniparam.AutoGenerateColumns = false;
            DataGridTextColumn nazivtabizlavr = new DataGridTextColumn();
            nazivtabizlavr.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            nazivtabizlavr.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            nazivtabizlavr.Header = "NAZIV TABELE";
            nazivtabizlavr.Binding = new Binding("Nazivtabelarniizlazniparam");
            nazivtabizlavr.Width = 150;
            dgTabizlazniparam.Columns.Add(nazivtabizlavr);


            DataGridTextColumn poljetabizlazparam = new DataGridTextColumn();
            poljetabizlazparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            poljetabizlazparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            poljetabizlazparam.Header = "POLJE";
            poljetabizlazparam.Binding = new Binding("Poljetabelarniizlazniparam");
            poljetabizlazparam.Width = 150;

            dgTabizlazniparam.Columns.Add(poljetabizlazparam);




            DataGridTextColumn tiptabizlazparam = new DataGridTextColumn();
            tiptabizlazparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            tiptabizlazparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            tiptabizlazparam.Header = "TIP";
            tiptabizlazparam.Binding = new Binding("Tipizlazniparam");
            tiptabizlazparam.Width = 70;

            dgTabizlazniparam.Columns.Add(tiptabizlazparam);

            DataGridTextColumn duzinatabizlazparam = new DataGridTextColumn();
            duzinatabizlazparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            duzinatabizlazparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            duzinatabizlazparam.Header = "DUZINA";
            duzinatabizlazparam.Binding = new Binding("Duzinapoljaizlazniparam");
            duzinatabizlazparam.Width = 75;

            dgTabizlazniparam.Columns.Add(duzinatabizlazparam);

            DataGridTextColumn infotabizlazparam = new DataGridTextColumn();
            infotabizlazparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            infotabizlazparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            infotabizlazparam.Header = "INFOSYS";
            infotabizlazparam.Binding = new Binding("Infosysizlazniparam");
            infotabizlazparam.Width = 190;

            dgTabizlazniparam.Columns.Add(infotabizlazparam);


            DataGridTextColumn opistabizlazparam = new DataGridTextColumn();
            opistabizlazparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            opistabizlazparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            opistabizlazparam.Header = "OPIS";
            opistabizlazparam.Binding = new Binding("Opistabelarniizlazniparam");
            opistabizlazparam.Width = 270;

            dgTabizlazniparam.Columns.Add(opistabizlazparam);


            #region POMOCNI MENI

            MenuItem cmDodajtabelarniizlvre = new MenuItem();
            cmDodajtabelarniizlvre.Header = "Dodaj parametar";
            cmDodajtabelarniizlvre.Click += CmDodajtabelarniizlvre_Click;


            ///////////////////////////////////////////////
            MenuItem cmdeletetabizlvr = new MenuItem();
            cmdeletetabizlvr.Header = "Delete";
            cmdeletetabizlvr.Click += Cmdeletetabizlvr_Click;

            //////////////////////////////////////////////////
            MenuItem cmzatvoritabizlvr = new MenuItem();
            cmzatvoritabizlvr.Header = "Zatvori formu";
            cmzatvoritabizlvr.Click += Cmzatvoritabizlvr_Click;



            /////////////////////////////////////////////////


            MenuItem prikazitabelarneizlaznevrednosti = new MenuItem();
            prikazitabelarneizlaznevrednosti.Header = "Prikazi formu";
            prikazitabelarneizlaznevrednosti.Click += Prikazitabelarneizlaznevrednosti_Click;


            ///////////////////////////////////////
            ContextMenu cmtabizparm = new ContextMenu();

            cmtabizparm.Items.Add(cmDodajtabelarniizlvre);
            cmtabizparm.Items.Add(cmdeletetabizlvr);
            cmtabizparm.Items.Add(cmzatvoritabizlvr);
            cmtabizparm.Items.Add(prikazitabelarneizlaznevrednosti);
            dgTabizlazniparam.ContextMenu = cmtabizparm;
            #endregion

            #endregion

            #region formdodatak

            dgDodatak.AutoGenerateColumns = false;
            DataGridTextColumn nazivdodatak = new DataGridTextColumn();
            nazivdodatak.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            nazivdodatak.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            nazivdodatak.Header = "DODATAK";
            nazivdodatak.Binding = new Binding("Dodataknaziv");
            nazivdodatak.Width = 150;
            dgDodatak.Columns.Add(nazivdodatak);

            DataGridTextColumn tipdodatak = new DataGridTextColumn();
            tipdodatak.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            tipdodatak.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            tipdodatak.Header = "TIP";
            tipdodatak.Binding = new Binding("Dodataktip");
            tipdodatak.Width = 150;
            dgDodatak.Columns.Add(tipdodatak);

            DataGridTextColumn opisdodatak = new DataGridTextColumn();
            opisdodatak.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            opisdodatak.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            opisdodatak.Header = "OPIS";
            opisdodatak.Binding = new Binding("Opisdodatka");
            opisdodatak.Width = 150;
            dgDodatak.Columns.Add(opisdodatak);


            DataGridTextColumn napomenadodatak = new DataGridTextColumn();
            napomenadodatak.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            napomenadodatak.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            napomenadodatak.Header = "NAPOMENA";
            napomenadodatak.Binding = new Binding("NapomeanaDodatak");
            napomenadodatak.Width = 150;
            dgDodatak.Columns.Add(napomenadodatak);




            #region POMOCNI MENI

            MenuItem deleted = new MenuItem();
            deleted.Header = "Delete";
            deleted.Click += Deleted_Click;


            ////////////////
            MenuItem dodajd = new MenuItem();
            dodajd.Header = "Dodaj parametar";
            dodajd.Click += Dodajd_Click;

            ////////////
            MenuItem prikazisdodp = new MenuItem();
            prikazisdodp.Header = "Prikazi formu";
            prikazisdodp.Click += Prikazisdodp_Click;

            ////////////
            MenuItem sklonidod = new MenuItem();
            sklonidod.Header = "Zatvori formu";
            sklonidod.Click += Sklonidod_Click;


            ContextMenu contextMendodz = new ContextMenu();
            contextMendodz.Items.Add(dodajd);
            contextMendodz.Items.Add(deleted);
            contextMendodz.Items.Add(prikazisdodp);
            contextMendodz.Items.Add(sklonidod);

            dgDodatak.ContextMenu = contextMendodz;

            //***********************************************************************************
            StatusKesiranja();


            if (treeView.Items != null)
            {
                treeView.Items.Clear();
            }
            CIniFile Ini11 = new CIniFile(ApiParametri.PutanjadoInifajla.ToString());

            string projectPath11 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string Dir = Ini11.Read("Direktorijumzaobjavljenefunkcije", "ApiDocument");
            string folderName11 = System.IO.Path.Combine(projectPath11, Dir.ToString());
            if (Directory.Exists(folderName11))
            {
                DirectoryInfo Direktorijum = new DirectoryInfo(folderName11);
                FileInfo[] PdfFajlovi = Direktorijum.GetFiles("*.pdf");
                foreach (FileInfo objavljenipdf in PdfFajlovi)
                {
                    treeView.Items.Add(objavljenipdf);
                }
            }
            else
            {
                //  MessageBox.Show("nema");
            }

            //  CIniFile Ini = new CIniFile(ApiParametri.PutanjadoInifajla);
            string Prikazigrid = Ini.Read("Prikazisamogrid", "ApiDocument");


            if (Prikazigrid.ToString() == "True")
            {
                // Da li ce podrazumevano biti prikazani gridovi
                PrikaziGridove();
            }

            ApiParametri.PutanjaSacuvanogDokumenta = "";

            if (string.IsNullOrEmpty(ApiParametri.PutanjaSacuvanogDokumenta))
            {
                StanjeSaveAsKomande = true;
            }



            if (txtnazivfunkcija.Text == "")

            {
                ApiParametri.Sacuvano = false;

            }


            cbprikazi.Content = "Prikazi sve";


            ApiParametri.ZapamcenaPutanjaExplorer = Ini.Read("PoslednjeSacuvanaPutanjaustabli", "ApiDocument");

            Favorites.Items.Add(ApiParametri.ZapamcenaPutanjaExplorer.ToString());

            txtnazivfunkcija.AddHandler(TextBox.TextChangedEvent, new RoutedEventHandler(Textchetest));

            foreach (DriveInfo driv in DriveInfo.GetDrives())
            {

                if (driv.IsReady) Explorer(driv.Name, driv.Name, folders, null, false);

            }
            //************************************************************

            try
            {
                string autopdf = Ini.Read("AutoPDF", "ApiDocument");
                bool autosave = Convert.ToBoolean(autopdf);
                //  MessageBox.Show(autosave.ToString());
                cbautopdf2.IsChecked = autosave;
            }
            catch (Exception)
            {

                return;
            }


            #region POZVANE METODE


            CPositionHelper.SetSize(this);
            ApiParametri.VidljivostkomandeSave = false;



            #endregion

            #region PODESAVANJE VELICINE I NAZIVA FONTA
            //opis funkcije***********************************************

            try
            {
                CIniFile MojIni = new CIniFile(ApiParametri.PutanjadoInifajla.ToString());
                ApiParametri.VelicinaFonta = MojIni.Read("VelicinaFonta", "ApiDocument");


                //  MojIni.Write("FontFamily", "Arial", "ApiDocument");
                // ApiParametri.FontFamily = MojIni.Read("FontFamily", "ApiDocument");

                podesavanja.slider.Value = Convert.ToInt32(ApiParametri.VelicinaFonta);
                // MessageBox.Show(podesavanja.slider.Value.ToString());
                txtnazivfunkcija.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                txtnazivfunkcija.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                //   txtnazivfunkcija.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());

                txtModul.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                txtModul.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                // txtModul.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                rbOpisfunkcije.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                // rbOpisfunkcije.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                rbopisLogikefunkcije.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //  rbopisLogikefunkcije.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                rbOgrucestfunkc.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //  rbOgrucestfunkc.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());

                rbOpisulp.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                // rbOpisulp.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                txtdefulparam.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                txtdefulparam.Height = Convert.ToInt32(ApiParametri.VelicinaFonta);
                // txtdefulparam.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                //label

                //*******************************************************
                txtNazivulparam.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                txtNazivulparam.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                //  txtNazivulparam.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                rbOpisulp.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                // rbOpisulp.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                txtdefulparam.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtdefulparam.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //  txtdefulparam.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                //Lbel


                //*************************************
                rbScenario.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                // rbScenario.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                //*************************************
                txtNazivspeculparam.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtNazivspeculparam.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                // txtNazivspeculparam.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                rbOpisSpeculparam.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                txtDefspulparam.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtDefspulparam.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                // txtDefspulparam.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                //Lbel

                //**************************************************************** tabulparan
                txtNazivtabulparam.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtNazivtabulparam.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //  txtNazivtabulparam.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                txtPoletabulparam.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtPoletabulparam.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //  txtPoletabulparam.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                txttipolparam.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txttipolparam.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                // txttipolparam.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                txtduztabulparam.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtduztabulparam.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //  txtduztabulparam.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                txtInfosys.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtInfosys.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //  txtInfosys.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                // brNovatabela.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //  brNovatabela.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                //labele


                txtTipUlaznogparametra.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtTipUlaznogparametra.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);

                //**************************************************************
                txtIdentifgreske.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtIdentifgreske.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                // txtIdentifgreske.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                txtTiogreske.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtTiogreske.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //  txtTiogreske.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());

                rbopisgreske.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //    rbopisgreske.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                //label 


                //***************************************************************
                txtIzlvrn.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtIzlvrn.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //   txtIzlvrn.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                rbopisizlvr.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //   rbopisizlvr.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                //label

                //********************************************************************
                txtnazspizlvr.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                txtnazspizlvr.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                //    txtnazspizlvr.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                txtDefizlazvr.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                txtDefizlazvr.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                //     txtDefizlazvr.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                rbspizlvr.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //   rbspizlvr.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                //label


                //*********************************************************************
                txtNazivtabizlazniparam.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtNazivtabizlazniparam.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //  txtNazivtabizlazniparam.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                txtPoletabizlazniparam.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtPoletabizlazniparam.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //   txtPoletabizlazniparam.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                txttipizlazniparam.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txttipizlazniparam.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //    txttipizlazniparam.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                txtduztabizlazniparam.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtduztabizlazniparam.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //    txtduztabizlazniparam.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                txttzlaznuInfosys.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txttzlaznuInfosys.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //   txttzlaznuInfosys.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                //brNovatabelaizlazni.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //     brNovatabelaizlazni.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                //labele

                //******************************************************************
                txtNazivDodatka.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtNazivDodatka.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //   txtNazivDodatka.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                txtTipDodatka.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtTipDodatka.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //  txtTipDodatka.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                rbOpisdodatka.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
                //    rbOpisdodatka.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());
                //label


                txtTipspeculparam.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtTipspecizlaznevr.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                //  txtTipspecizlaznevr.FontFamily = new FontFamily(ApiParametri.FontFamily.ToString());

                txtDuzinaUlaznogparametra.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                txtDuzinaUlaznogparametra.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);

                cbTipdodatak.Height = Convert.ToInt32(ApiParametri.VelicinaFonta) + 10;
                cbTipdodatak.FontSize = Convert.ToInt32(ApiParametri.VelicinaFonta);
            }
            catch (Exception)
            {

                return;
            }


            #endregion

            #region FORMATIRANJE GRIDOVA

            #region FORMATIRANJE DATA GRIDA ULAZNI PARAMETRI
            // data grid ulazni parametri

            dgulparam.AutoGenerateColumns = false;
            DataGridTextColumn naziv = new DataGridTextColumn();
            naziv.Header = "NAZIV";
            naziv.Binding = new Binding("Nazivulazniparametri");
            naziv.Width = 150;
            naziv.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            naziv.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));

            dgulparam.Columns.Add(naziv);


            DataGridTextColumn tipilp = new DataGridTextColumn();
            tipilp.Header = "TIP";
            tipilp.Binding = new Binding("Tipulaznogparametra");
            tipilp.Width = 70;
            tipilp.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            tipilp.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));

            dgulparam.Columns.Add(tipilp);

            DataGridTextColumn duzinaulaznogparam = new DataGridTextColumn();
            duzinaulaznogparam.Header = "DUZINA";
            duzinaulaznogparam.Binding = new Binding("Duzinaulaznogparametra");
            duzinaulaznogparam.Width = 75;
            duzinaulaznogparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            duzinaulaznogparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));

            dgulparam.Columns.Add(duzinaulaznogparam);

            DataGridTextColumn opis = new DataGridTextColumn();
            opis.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            opis.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            opis.Header = "OPIS";
            opis.Width = 306;
            opis.Binding = new Binding("Opisulazniparam");
            dgulparam.Columns.Add(opis);

            DataGridTextColumn obavezno = new DataGridTextColumn();
            obavezno.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            obavezno.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            obavezno.Header = "OBAV.";
            obavezno.Width = 60;


            obavezno.Binding = new Binding("Obaveznostulaznogparam");
            dgulparam.Columns.Add(obavezno);

            DataGridTextColumn def = new DataGridTextColumn();
            def.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            def.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            def.Header = "DEFAULT";
            def.Width = 228;
            def.Binding = new Binding("Defaultulaznogparam");
            dgulparam.Columns.Add(def);
            /////////////////////////////////

            #region POMOCNI MENI

            try
            {
                MenuItem delete = new MenuItem();
                delete.Header = "Delete";
                delete.Click += Delete_Click;

                MenuItem zatvoriformu = new MenuItem();
                zatvoriformu.Header = "Zatvori formu";
                zatvoriformu.Click += Zatvoriformu_Click;


                MenuItem dodajulazniparametar = new MenuItem();
                dodajulazniparametar.Header = "Dodaj parametar";
                dodajulazniparametar.Click += Dodajulazniparametar_Click;

                MenuItem prikaziformu = new MenuItem();
                prikaziformu.Header = "Prikazi formu";
                prikaziformu.Click += Prikaziformu_Click;


                //    string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;


                ContextMenu contextMenu = new ContextMenu();

                contextMenu.Items.Add(dodajulazniparametar);
                contextMenu.Items.Add(delete);
                contextMenu.Items.Add(zatvoriformu);
                contextMenu.Items.Add(prikaziformu);
                dgulparam.ContextMenu = contextMenu;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }

            #endregion


            #endregion

            #region FORMATIRANJE GRIDA SPECIJALIZOVANI ULAZNI PARAMETRI
            // specijalizovani ulazni parametri

            dgSpeculparam.AutoGenerateColumns = false;
            DataGridTextColumn nazivspulparam = new DataGridTextColumn();
            nazivspulparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            nazivspulparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            nazivspulparam.Header = "NAZIV";
            nazivspulparam.Binding = new Binding("specNazivulazniparametri");
            nazivspulparam.Width = 230;

            dgSpeculparam.Columns.Add(nazivspulparam);

            DataGridTextColumn tipspeculparam = new DataGridTextColumn();
            tipspeculparam.ElementStyle = new Style(typeof(TextBlock));
            tipspeculparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            tipspeculparam.Header = "TIP";
            tipspeculparam.Binding = new Binding("specTipulparam");
            tipspeculparam.Width = 50;
            dgSpeculparam.Columns.Add(tipspeculparam);


            DataGridTextColumn opisspulparam = new DataGridTextColumn();
            opisspulparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            opisspulparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            opisspulparam.Header = "OPIS";
            opisspulparam.Binding = new Binding("specOpisulazniparam");
            opisspulparam.Width = 419;

            dgSpeculparam.Columns.Add(opisspulparam);

            DataGridTextColumn obavspulparam = new DataGridTextColumn();
            obavspulparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            obavspulparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            obavspulparam.Header = "OBAV.";
            obavspulparam.Binding = new Binding("specObaveznostulaznogparam");
            obavspulparam.Width = 55;
            dgSpeculparam.Columns.Add(obavspulparam);

            DataGridTextColumn defaultspulparam = new DataGridTextColumn();
            defaultspulparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            defaultspulparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            defaultspulparam.Header = "DEFAULT";
            defaultspulparam.Binding = new Binding("specDefaultulaznogparam");
            defaultspulparam.Width = 150;
            dgSpeculparam.Columns.Add(defaultspulparam);






            #region POMOCNI MENI

            MenuItem deletespulp = new MenuItem();
            deletespulp.Header = "Delete";
            deletespulp.Click += Deletespulp_Click;

            //string projectPathspulp = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            //string imagePathspulp = System.IO.Path.Combine(projectPathspulp, @"icon\Delete.png");
            //Image iconspulp = new Image();
            //iconspulp.Source = new BitmapImage(new Uri(imagePathspulp, UriKind.Absolute));
            //deletespulp.Icon = iconspulp;
            ////////////////
            MenuItem dodajspulp = new MenuItem();
            dodajspulp.Header = "Dodaj parametar";
            dodajspulp.Click += Dodajspulp_Click;
            //string imagePathspulpd = System.IO.Path.Combine(projectPathspulp, @"icon\Add.png");
            //Image iconspulpd = new Image();
            //iconspulpd.Source = new BitmapImage(new Uri(imagePathspulpd, UriKind.Absolute));
            //dodajspulp.Icon = iconspulpd;
            ////////////
            MenuItem prikazispulp = new MenuItem();
            prikazispulp.Header = "Prikazi formu";
            prikazispulp.Click += Prikazispulp_Click;
            //string imagePathspulpp = System.IO.Path.Combine(projectPathspulp, @"icon\strelicaotvori16x16.png");
            //Image iconspulpp = new Image();
            //iconspulpp.Source = new BitmapImage(new Uri(imagePathspulpp, UriKind.Absolute));
            //prikazispulp.Icon = iconspulpp;
            ////////////
            MenuItem sklonispulp = new MenuItem();
            sklonispulp.Header = "Zatvori formu";
            sklonispulp.Click += Sklonispulp_Click;
            //string imagePathspulps = System.IO.Path.Combine(projectPathspulp, @"icon\strelica16x16.png");
            //Image iconspulps = new Image();
            //iconspulps.Source = new BitmapImage(new Uri(imagePathspulps, UriKind.Absolute));
            //sklonispulp.Icon = iconspulps;

            ContextMenu contextMenuspulp = new ContextMenu();
            contextMenuspulp.Items.Add(dodajspulp);
            contextMenuspulp.Items.Add(deletespulp);
            contextMenuspulp.Items.Add(prikazispulp);
            contextMenuspulp.Items.Add(sklonispulp);

            dgSpeculparam.ContextMenu = contextMenuspulp;
            #endregion

            #endregion

            #region FORMATIRANJE GRIDA TABELARNI ULAZNI PARAMETRI
            //tabelarni ulazni parametri

            dgTabulparam.AutoGenerateColumns = false;
            DataGridTextColumn nazivtabulparam = new DataGridTextColumn();
            nazivtabulparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            nazivtabulparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            nazivtabulparam.Header = "NAZIV TABELE";
            nazivtabulparam.Binding = new Binding("Nazivtabelarniulparam");
            nazivtabulparam.Width = 150;
            dgTabulparam.Columns.Add(nazivtabulparam);


            DataGridTextColumn poljetabulparam = new DataGridTextColumn();
            poljetabulparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            poljetabulparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            poljetabulparam.Header = "POLJE";
            poljetabulparam.Binding = new Binding("Poljetabelarniulparam");
            poljetabulparam.Width = 150;

            dgTabulparam.Columns.Add(poljetabulparam);

            DataGridTextColumn tiptabulparam = new DataGridTextColumn();
            tiptabulparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            tiptabulparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            tiptabulparam.Header = "TIP";
            tiptabulparam.Binding = new Binding("Tipulparam");
            tiptabulparam.Width = 150;

            dgTabulparam.Columns.Add(tiptabulparam);

            DataGridTextColumn opistabulparam = new DataGridTextColumn();
            opistabulparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            opistabulparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            opistabulparam.Header = "OPIS";
            opistabulparam.Binding = new Binding("Opistabelarniulparam");
            opistabulparam.Width = 150;

            dgTabulparam.Columns.Add(opistabulparam);

            DataGridTextColumn duzinatabulparam = new DataGridTextColumn();
            duzinatabulparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            duzinatabulparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            duzinatabulparam.Header = "DUZINA";
            duzinatabulparam.Binding = new Binding("Duzinapoljaulparam");
            duzinatabulparam.Width = 150;

            dgTabulparam.Columns.Add(duzinatabulparam);

            DataGridTextColumn infotabulparam = new DataGridTextColumn();
            infotabulparam.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            infotabulparam.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            infotabulparam.Header = "INFOSYS";
            infotabulparam.Binding = new Binding("Infosusulparam");
            infotabulparam.Width = 150;

            dgTabulparam.Columns.Add(infotabulparam);

            #region POMOCNI MENI
            MenuItem cmDodajtabelarniulp = new MenuItem();
            cmDodajtabelarniulp.Header = "Dodaj parametar";
            cmDodajtabelarniulp.Click += CmDodajtabelarniulp_Click;
            // string projectPathtabulp = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            // string imagePathtabulpd = System.IO.Path.Combine(projectPathspulp, @"icon\Add.png");
            // Image icontabulpd = new Image();
            // icontabulpd.Source = new BitmapImage(new Uri(imagePathtabulpd, UriKind.Absolute));
            // cmDodajtabelarniulp.Icon = icontabulpd;

            // /////////////////////////////////////////////
            MenuItem cmDeletetabelarniulp = new MenuItem();
            cmDeletetabelarniulp.Header = "Delete";
            cmDeletetabelarniulp.Click += CmDeletetabelarniulp_Click;
            //// string projectPathtabulp = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            // string imagePathtabulp = System.IO.Path.Combine(projectPathspulp, @"icon\Delete.png");
            // Image icontabulp = new Image();
            // icontabulp.Source = new BitmapImage(new Uri(imagePathtabulp, UriKind.Absolute));
            // cmDeletetabelarniulp.Icon = icontabulp;
            // ////////////////////////////////////////////////
            MenuItem cmZatvoritabelarniulp = new MenuItem();
            cmZatvoritabelarniulp.Header = "Zatvori formu";
            cmZatvoritabelarniulp.Click += CmZatvoritabelarniulp_Click;
            // // string projectPathtabulp = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            // string imagePathtabulpz = System.IO.Path.Combine(projectPathspulp, @"icon\strelica16x16.png");
            // Image icontabulpz = new Image();
            // icontabulpz.Source = new BitmapImage(new Uri(imagePathtabulpz, UriKind.Absolute));
            // cmZatvoritabelarniulp.Icon = icontabulpz;


            // ///////////////////////////////////////////////


            MenuItem prikazitabformu = new MenuItem();
            prikazitabformu.Header = "Prikazi formu";
            prikazitabformu.Click += Prikazitabformu_Click;
            // string imagePathpiz = System.IO.Path.Combine(projectPathspulp, @"icon\strelica16x16.png");
            // Image iconpiz = new Image();
            // iconpiz.Source = new BitmapImage(new Uri(imagePathpiz, UriKind.Absolute));
            // prikazitabformu.Icon = iconpiz;


            // /////////////////////////////////////
            ContextMenu cmtabulp = new ContextMenu();

            cmtabulp.Items.Add(cmDodajtabelarniulp);
            cmtabulp.Items.Add(cmDeletetabelarniulp);
            cmtabulp.Items.Add(cmZatvoritabelarniulp);
            cmtabulp.Items.Add(prikazitabformu);
            dgTabulparam.ContextMenu = cmtabulp;



            #endregion


            #endregion


            #region FORMATIRANJE  GRIDA STATUSI GRESKE
            // statusi greske

            dgstatusgreske.AutoGenerateColumns = false;

            DataGridTextColumn identstatgreske = new DataGridTextColumn();
            identstatgreske.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            identstatgreske.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            identstatgreske.Header = "IDENTIFIKATOR";
            identstatgreske.Binding = new Binding("Identigikatorgreske");
            identstatgreske.Width = 250;
            dgstatusgreske.Columns.Add(identstatgreske);

            DataGridTextColumn tipgreske = new DataGridTextColumn();
            tipgreske.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            tipgreske.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            tipgreske.Header = "TIP";
            tipgreske.Binding = new Binding("Tipgreske");
            tipgreske.Width = 200;
            dgstatusgreske.Columns.Add(tipgreske);

            DataGridTextColumn opisgreske = new DataGridTextColumn();
            opisgreske.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            opisgreske.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            opisgreske.Header = "OPIS GRESKE";
            opisgreske.Binding = new Binding("Opisgreske");
            opisgreske.Width = 460;
            dgstatusgreske.Columns.Add(opisgreske);

            #region POMOCNI MENI

            MenuItem deletesgr = new MenuItem();
            deletesgr.Header = "Delete";
            deletesgr.Click += Deletesgr_Click;

            //string projectPathsg = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            //string imagePathsg = System.IO.Path.Combine(projectPathspulp, @"icon\Delete.png");
            //Image iconsg = new Image();
            //iconsg.Source = new BitmapImage(new Uri(imagePathsg, UriKind.Absolute));
            //deletesgr.Icon = iconsg;
            ////////////////
            MenuItem dodajsgr = new MenuItem();
            dodajsgr.Header = "Dodaj parametar";
            dodajsgr.Click += Dodajsgr_Click;
            //string imagePathsgd = System.IO.Path.Combine(projectPathspulp, @"icon\Add.png");
            //Image iconsgd = new Image();
            //iconsgd.Source = new BitmapImage(new Uri(imagePathsgd, UriKind.Absolute));
            //dodajsgr.Icon = iconsgd;
            ////////////
            MenuItem prikazisgr = new MenuItem();
            prikazisgr.Header = "prikazi formu";
            prikazisgr.Click += Prikazisgr_Click;
            //string imagePathsgp = System.IO.Path.Combine(projectPathspulp, @"icon\strelicaotvori16x16.png");
            //Image iconsgp = new Image();
            //iconsgp.Source = new BitmapImage(new Uri(imagePathsgp, UriKind.Absolute));
            //prikazisgr.Icon = iconsgp;
            ////////////
            MenuItem sklonisgr = new MenuItem();
            sklonisgr.Header = "Zatvori formu";
            sklonisgr.Click += Sklonisgr_Click;
            //string imagePathsgz = System.IO.Path.Combine(projectPathspulp, @"icon\strelica16x16.png");
            //Image iconsgz = new Image();
            //iconsgz.Source = new BitmapImage(new Uri(imagePathsgz, UriKind.Absolute));
            //sklonisgr.Icon = iconsgz;

            ContextMenu cmstatusigreski = new ContextMenu();
            cmstatusigreski.Items.Add(dodajsgr);
            cmstatusigreski.Items.Add(deletesgr);
            cmstatusigreski.Items.Add(prikazisgr);
            cmstatusigreski.Items.Add(sklonisgr);

            dgstatusgreske.ContextMenu = cmstatusigreski;


            #endregion



            #endregion

            #region FORMATIRANJE GRIDA IZLAZNE VREDNOSTI
            // izlazne vrednosti
            dgIzlaznevr.AutoGenerateColumns = false;
            DataGridTextColumn nazivizlvr = new DataGridTextColumn();
            nazivizlvr.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            nazivizlvr.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            nazivizlvr.Header = "NAZIV";
            nazivizlvr.Binding = new Binding("Nazivizlaznevr");
            nazivizlvr.Width = 300;
            dgIzlaznevr.Columns.Add(nazivizlvr);

            DataGridTextColumn OPISizlvr = new DataGridTextColumn();
            OPISizlvr.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            OPISizlvr.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            OPISizlvr.Header = "OPIS";
            OPISizlvr.Binding = new Binding("Opisizlaznevr");
            OPISizlvr.Width = 605;
            dgIzlaznevr.Columns.Add(OPISizlvr);

            #region POMOCNI MENI

            MenuItem deleteiz = new MenuItem();
            deleteiz.Header = "Delete";
            deleteiz.Click += Deleteiz_Click;

            // string projectPathiz = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            // string imagePathiz = System.IO.Path.Combine(projectPathiz, @"icon\Delete.png");
            // Image iconiz = new Image();
            // iconiz.Source = new BitmapImage(new Uri(imagePathiz, UriKind.Absolute));
            // deleteiz.Icon = iconiz;
            // //////////////
            MenuItem dodajiz = new MenuItem();
            dodajiz.Header = "Dodaj parametar";
            dodajiz.Click += Dodajiz_Click;
            // string imagePathizd = System.IO.Path.Combine(projectPathspulp, @"icon\Add.png");
            // Image iconizd = new Image();
            // iconizd.Source = new BitmapImage(new Uri(imagePathizd, UriKind.Absolute));
            // dodajiz.Icon = iconizd;
            // //////////
            MenuItem prikaziizp = new MenuItem();
            prikaziizp.Header = "Prikazi formu";
            prikaziizp.Click += Prikaziizp_Click;
            // string imagePathizp = System.IO.Path.Combine(projectPathspulp, @"icon\strelicaotvori16x16.png");
            // Image iconsizp = new Image();
            // iconsizp.Source = new BitmapImage(new Uri(imagePathizp, UriKind.Absolute));
            // prikaziizp.Icon = iconsizp;
            // //////////
            MenuItem skloniiz = new MenuItem();
            skloniiz.Header = "Zatvori formu";
            skloniiz.Click += Skloniiz_Click;
            // string imagePathizz = System.IO.Path.Combine(projectPathspulp, @"icon\strelica16x16.png");
            // Image iconizz = new Image();
            // iconizz.Source = new BitmapImage(new Uri(imagePathizz, UriKind.Absolute));
            // skloniiz.Icon = iconizz;


            ContextMenu contextMeizg = new ContextMenu();
            contextMeizg.Items.Add(dodajiz);
            contextMeizg.Items.Add(deleteiz);
            contextMeizg.Items.Add(prikaziizp);
            contextMeizg.Items.Add(skloniiz);

            dgIzlaznevr.ContextMenu = contextMeizg;


            #endregion


            #endregion

            #region FORMATIRANJE GRIDA SPECIJALIZOVANE IZLAZNE VREDNOSTI

            // specijalizovane izlazne vrednosti

            dgspizlazvre.AutoGenerateColumns = false;
            DataGridTextColumn spnazivizlvr = new DataGridTextColumn();
            spnazivizlvr.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            spnazivizlvr.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            spnazivizlvr.Header = "NAZIV";
            spnazivizlvr.Binding = new Binding("Nazivspizlaznevr");
            spnazivizlvr.Width = 150;
            dgspizlazvre.Columns.Add(spnazivizlvr);

            DataGridTextColumn tipspecizlaznevr = new DataGridTextColumn();
            tipspecizlaznevr.ElementStyle = new Style(typeof(TextBlock));
            tipspecizlaznevr.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            tipspecizlaznevr.Header = "TIP";
            tipspecizlaznevr.Binding = new Binding("TipSpecijalneIzlaznevrednosti");
            tipspecizlaznevr.Width = 50;
            dgspizlazvre.Columns.Add(tipspecizlaznevr);


            DataGridTextColumn spopisizlaznevr = new DataGridTextColumn();
            spopisizlaznevr.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            spopisizlaznevr.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            spopisizlaznevr.Header = "OPIS ";
            spopisizlaznevr.Binding = new Binding("Opisspizlaznevr");
            spopisizlaznevr.Width = 475;
            dgspizlazvre.Columns.Add(spopisizlaznevr);

            DataGridTextColumn spobaveznostizlaznevr = new DataGridTextColumn();
            spobaveznostizlaznevr.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            spobaveznostizlaznevr.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            spobaveznostizlaznevr.Header = "OBAV.";
            spobaveznostizlaznevr.Binding = new Binding("Obaveznost");
            spobaveznostizlaznevr.Width = 70;
            dgspizlazvre.Columns.Add(spobaveznostizlaznevr);

            DataGridTextColumn spdefaultizlaznevr = new DataGridTextColumn();
            spdefaultizlaznevr.ElementStyle = new System.Windows.Style(typeof(TextBlock));
            spdefaultizlaznevr.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            spdefaultizlaznevr.Header = "DEFAULT ";
            spdefaultizlaznevr.Binding = new Binding("Defizlazvr");
            spdefaultizlaznevr.Width = 150;
            dgspizlazvre.Columns.Add(spdefaultizlaznevr);


            #region POMOCNI MENI

            MenuItem deletespiz = new MenuItem();
            deletespiz.Header = "Delete";
            deletespiz.Click += Deletespiz_Click;

            //string projectPathspiz = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            //string imagePathspiz = System.IO.Path.Combine(projectPathspiz, @"icon\Delete.png");
            //Image iconspiz = new Image();
            //iconspiz.Source = new BitmapImage(new Uri(imagePathspiz, UriKind.Absolute));
            //deletespiz.Icon = iconspiz;
            ////////////////
            MenuItem dodajspiz = new MenuItem();
            dodajspiz.Header = "Dodaj parametar";
            dodajspiz.Click += Dodajspiz_Click;
            //string imagePathspizd = System.IO.Path.Combine(projectPathspulp, @"icon\Add.png");
            //Image iconspizd = new Image();
            //iconspizd.Source = new BitmapImage(new Uri(imagePathspizd, UriKind.Absolute));
            //dodajspiz.Icon = iconspizd;
            ////////////
            MenuItem prikazispizp = new MenuItem();
            prikazispizp.Header = "Prikazi formu";
            prikazispizp.Click += Prikazispizp_Click;
            //string imagePathspizp = System.IO.Path.Combine(projectPathspulp, @"icon\strelicaotvori16x16.png");
            //Image iconspizp = new Image();
            //iconspizp.Source = new BitmapImage(new Uri(imagePathspizp, UriKind.Absolute));
            //prikazispizp.Icon = iconspizp;
            ////////////
            MenuItem sklonispiz = new MenuItem();
            sklonispiz.Header = "Zatvori formu";
            sklonispiz.Click += Sklonispiz_Click;
            //string imagePathspizz = System.IO.Path.Combine(projectPathspulp, @"icon\strelica16x16.png");
            //Image iconspizz = new Image();
            //iconspizz.Source = new BitmapImage(new Uri(imagePathspizz, UriKind.Absolute));
            //sklonispiz.Icon = iconspizz;

            ContextMenu contextMenispiz = new ContextMenu();
            contextMenispiz.Items.Add(dodajspiz);
            contextMenispiz.Items.Add(deletespiz);
            contextMenispiz.Items.Add(prikazispizp);
            contextMenispiz.Items.Add(sklonispiz);

            dgspizlazvre.ContextMenu = contextMenispiz;
        }

        private void CmdPrikaziformu_Click(object sender, RoutedEventArgs e)
        {
            Prikaziformusaradnicii();
        }

        private void CmdSaradZatvoriformu_Click(object sender, RoutedEventArgs e)
        {
            Zatvoriformusaradnika();
        }

        private void CmdDeletesarad_Click(object sender, RoutedEventArgs e)
        {
            btnobrisiSaradnika_Click(null, null);
        }

        private void CmdDodajsaradnika_Click(object sender, RoutedEventArgs e)
        {
            btnDodajsaradnika_Click(null, null);
        }



        #endregion

        #region SelectionChanged
        private void dgspizlazvre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtnazspizlvr.Text = (dgspizlazvre.SelectedItem as CSpizlazvr).Nazivspizlaznevr.ToString();
                txtTipspecizlaznevr.Text = (dgspizlazvre.SelectedItem as CSpizlazvr).TipSpecijalneIzlaznevrednosti.ToString();
                rbspizlvr.Document.Blocks.Clear();
                rbspizlvr.AppendText((dgspizlazvre.SelectedItem as CSpizlazvr).Opisspizlaznevr);
                txtDefizlazvr.Text = (dgspizlazvre.SelectedItem as CSpizlazvr).Defizlazvr.ToString();
                txtobaveznospizlazvr.Text = (dgspizlazvre.SelectedItem as CSpizlazvr).Obaveznost.ToString();
                switch (txtobaveznospizlazvr.Text)
                {
                    case "Da":

                        cbspizlazDa.IsChecked = true;
                        break;
                    case "Ne":
                        cbspizlazne.IsChecked = true;

                        break;
                    case " ":


                        break;

                }
            }
            catch (Exception)
            {

                return;
            }





        }

        #endregion

        #endregion



        #endregion

        #region PRIKAZI FORME POMOCNI MENI
        private void Prikaziizp_Click(object sender, RoutedEventArgs e)
        {
            Prikaziformuizlaznevrednosti();
        }

        private void Skloniiz_Click(object sender, RoutedEventArgs e)
        {
            Zatvoriformuizlaznevrednosti();
        }

        private void Dodajiz_Click(object sender, RoutedEventArgs e)
        {
            btnDodajizlvr_Click(null, null);
        }

        private void Deleteiz_Click(object sender, RoutedEventArgs e)
        {
            btnUkloniizlvr_Click(null, null);
        }

        private void Sklonidod_Click(object sender, RoutedEventArgs e)
        {
            Zatvoriformudodatak();
        }

        private void Prikazisdodp_Click(object sender, RoutedEventArgs e)
        {
            Prikaziformudodatak();
        }

        private void Dodajd_Click(object sender, RoutedEventArgs e)
        {
            btnDodajdodatak_Click(null, null);
        }

        private void Deleted_Click(object sender, RoutedEventArgs e)
        {
            btnobrisidodatak_Click(null, null);
        }

        private void Cmzatvoritabizlvr_Click(object sender, RoutedEventArgs e)
        {
            Zatvoriformutabelarniizlazniparametri();
        }

        private void CmDodajtabelarniizlvre_Click(object sender, RoutedEventArgs e)
        {
            btnDodajtabizlazniparam_Click(null, null);
        }

        private void Cmdeletetabizlvr_Click(object sender, RoutedEventArgs e)
        {
            btnobrisitabizlaznip_Click(null, null);
        }

        private void Prikazitabelarneizlaznevrednosti_Click(object sender, RoutedEventArgs e)
        {
            PrikaziformuTabelarniizlazniparametara();
        }

        private void Sklonispiz_Click(object sender, RoutedEventArgs e)
        {
            ZatvoriformuSpecijalizovaneIzlaznevrednosti();
        }

        private void Prikazispizp_Click(object sender, RoutedEventArgs e)
        {

            PrikaziformuSpecijalizovaniizlazniparametri();
        }

        private void Dodajspiz_Click(object sender, RoutedEventArgs e)
        {
            btnobrisispulp_Click(null, null);
        }

        private void Deletespiz_Click(object sender, RoutedEventArgs e)
        {
            btnobrisispulp_Click(null, null);
        }

        private void Prikazisgr_Click(object sender, RoutedEventArgs e)
        {
            PrikazifirmuStatusigreski();
        }

        private void Sklonisgr_Click(object sender, RoutedEventArgs e)
        {
            ZatvorifirmuStatusigreski();
        }

        private void Dodajsgr_Click(object sender, RoutedEventArgs e)
        {
            btnDodajstgreske_Click(null, null);
        }

        private void Deletesgr_Click(object sender, RoutedEventArgs e)
        {
            obrisisatgr_Click(null, null);
        }

        private void Dodajsg_Click(object sender, RoutedEventArgs e)
        {
            btnDodajtabulparam_Click(null, null);
        }

        private void Prikazitabformu_Click(object sender, RoutedEventArgs e)
        {
            PrikaziformuTabelarnihulaznihparametara();
        }

        private void CmZatvoritabelarniulp_Click(object sender, RoutedEventArgs e)
        {
            SkloniformuTabelarniulazniparametri();
        }

        private void CmDodajtabelarniulp_Click(object sender, RoutedEventArgs e)
        {
            btnDodajtabulparam_Click(null, null);
            txtNazivtabulparam.Clear();
        }

        private void CmDeletetabelarniulp_Click(object sender, RoutedEventArgs e)
        {
            this.btnobrisitabulp_Click(null, null);
        }

        private void Deletetbulp_Click(object sender, RoutedEventArgs e)
        {

            this.btnobrisitabulp_Click(null, null);
        }

        private void Deletespulp_Click(object sender, RoutedEventArgs e)
        {
            this.btnUkloniSpulparam_Click(null, null);
        }

        private void Dodajspulp_Click(object sender, RoutedEventArgs e)
        {
            dgSpeculparam.CanUserAddRows = true;
            btnDodajSpulparam_Click(null, null);
        }

        private void Prikazispulp_Click(object sender, RoutedEventArgs e)
        {
            Prikazifornuspecijalizovaniulparameri();
        }

        private void Sklonispulp_Click(object sender, RoutedEventArgs e)
        {
            Skloniformuspulparam();
        }
        #endregion

        #endregion

        #endregion

        #region ULAZNI PARAMETRI

        #region DA LI JE OBAVEZAN ULAZNI PARAMETAR
        private void Da_Click(object sender, RoutedEventArgs e)
        {
            if (Da.IsChecked == true)
            {
                txtobavulparam.Text = "Da";
                Da.BorderBrush = Brushes.Red;
                Da.BorderThickness = new Thickness(1);


            }

        }
        private void txtobavulparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
            if (txtobavulparam.Text == "Da")
            {
                Da.IsChecked = true;
                Da.BorderBrush = Brushes.Red;
                Da.BorderThickness = new Thickness(1);
            }
            if (txtobavulparam.Text == "Ne")
            {
                Ne.IsChecked = true;
                Ne.BorderBrush = Brushes.Red;
                Ne.BorderThickness = new Thickness(1);
            }
        }


        #endregion

        #region DODAJ ULAZNI PARAMETAR

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            CUIParam ulparam = new CUIParam();
            ulparam.Nazivulazniparametri = txtNazivulparam.Text;
            ulparam.Opisulazniparam = new TextRange(rbOpisulp.Document.ContentStart, rbOpisulp.Document.ContentEnd).Text;
            ulparam.Defaultulaznogparam = txtdefulparam.Text;
            ulparam.Obaveznostulaznogparam = txtobavulparam.Text;
            Lista.Add(ulparam);

            dgulparam.ItemsSource = Lista;

        }
        #endregion

        #region ULAZNI PARANETRI SELECTIONCHANGED
        private void dgulparam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {

                string naziv = (dgulparam.SelectedItem as CUIParam).Nazivulazniparametri.ToString();
                txtNazivulparam.Text = naziv;
                rbOpisulp.Document.Blocks.Clear();
                rbOpisulp.AppendText((dgulparam.SelectedItem as CUIParam).Opisulazniparam);
                txtdefulparam.Text = (dgulparam.SelectedItem as CUIParam).Defaultulaznogparam.ToString();
                txtobavulparam.Text = (dgulparam.SelectedItem as CUIParam).Obaveznostulaznogparam.ToString();
                txtTipUlaznogparametra.Text = (dgulparam.SelectedItem as CUIParam).Tipulaznogparametra.ToString();
                txtDuzinaUlaznogparametra.Text = (dgulparam.SelectedItem as CUIParam).Duzinaulaznogparametra.ToString();
            }
            catch (Exception)
            {

                return;
            }


        }
        #endregion

        #region UKLONI ULAZNE PARAMETRE BUTTON
        private void btnUkloniulparam_Click(object sender, RoutedEventArgs e)
        {
            txtNazivulparam.Clear();
            rbOpisulp.Document.Blocks.Clear();
            txtobavulparam.Clear();
            txtdefulparam.Clear();
            txtDuzinaUlaznogparametra.Clear();
            txtTipUlaznogparametra.Clear();

            int index = dgulparam.SelectedIndex;
            DeleteULP(index);
            dgulparam.ItemsSource = Lista;
        }
        #endregion

        #region ZAKLJUCAVA ULAZNI PARAMETAR ZA IZMENE I DODAJE IH U IZVESTAJ
        private void Zakljucaj_Click(object sender, RoutedEventArgs e)
        {
            if (Zakljucaj.IsChecked == true)
            {
                CIzvestaj.listaparametaraizv.Clear();
                foreach (CUIParam dodajp in Lista)
                {
                    CIzvestaj.listaparametaraizv.Add(dodajp);
                }
                txtNazivulparam.IsEnabled = false;
                rbOpisulp.IsEnabled = false;
                txtobavulparam.IsEnabled = false;
                txtdefulparam.IsEnabled = false;
                dgulparam.IsEnabled = true;
                Zakljucaj.IsEnabled = false;
                Otkljucano.IsEnabled = false;
                btnDodajparameter.IsEnabled = true;
                btnUkloniulparam.IsEnabled = true;
            }

        }
        #endregion

        #region OMOGUCAVA IZMENE ULAZNIH PARAMETARA
        private void Otkljucano_Click(object sender, RoutedEventArgs e)
        {
            if (Otkljucano.IsChecked == true)
            {
                CIzvestaj.listaparametaraizv.Clear();
                foreach (CUIParam dodajp in Lista)
                {
                    CIzvestaj.listaparametaraizv.Add(dodajp);
                }
                txtNazivulparam.IsEnabled = true;
                rbOpisulp.IsEnabled = true;
                txtobavulparam.IsEnabled = true;
                txtdefulparam.IsEnabled = true;
                dgulparam.IsEnabled = true;
                btnDodajparameter.IsEnabled = true;
                btnUkloniulparam.IsEnabled = true;
            }
        }
        #endregion

        #region PRIKAZI FORMU ULAZNIH PARAMETARA
        private void Prikaziformu_Click(object sender, RoutedEventArgs e)
        {
            Prikaziformuulaznihparametara();
        }

        private void Dodajulazniparametar_Click(object sender, RoutedEventArgs e)
        {
            dgulparam.CanUserAddRows = true;
            button_Click(null, null);

        }

        private void Zatvoriformu_Click(object sender, RoutedEventArgs e)
        {
            Skloniformuulaznihparametara();
        }




        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            this.btnUkloniulparam_Click(null, null);
        }
        #endregion

        #region TEXTCHANGED
        private void txtDuzinaUlaznogparametra_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
            if (txtDuzinaUlaznogparametra.Text != "")
            {
                txtDuzinaUlaznogparametra.BorderBrush = Brushes.Blue;
                txtDuzinaUlaznogparametra.BorderThickness = new Thickness(1);

            }
            else
            {
                txtDuzinaUlaznogparametra.BorderBrush = Brushes.Red;
                txtDuzinaUlaznogparametra.BorderThickness = new Thickness(1);
            }
        }
        private void txtTipUlaznogparametra_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
            if (txtTipUlaznogparametra.Text != "")
            {
                txtTipUlaznogparametra.BorderBrush = Brushes.Blue;
                txtTipUlaznogparametra.BorderThickness = new Thickness(0.5);

            }
            else
            {
                txtTipUlaznogparametra.BorderBrush = Brushes.Red;
                txtTipUlaznogparametra.BorderThickness = new Thickness(0.5);
            }
        }
        #endregion

        #endregion

        #region METODA ZA BRISANJE PARAMETARA
        public void DeleteULP(int p)
        {
            Lista.Remove(dgulparam.SelectedItem as CUIParam);
            ListaSpeculparam.Remove(dgSpeculparam.SelectedItem as CSpecUlparam);
            ListaTabulparam.Remove(dgTabulparam.SelectedItem as CTabUlparam);
            ListaGreske.Remove(dgstatusgreske.SelectedItem as CStatusGreske);
            Listaizlaznihvr.Remove(dgIzlaznevr.SelectedItem as CIzlaznevr);
            Listaspizlaznihvr.Remove(dgspizlazvre.SelectedItem as CSpizlazvr);
            Listadodataka.Remove(dgDodatak.SelectedItem as CDodatak);
            listaTabizlparam.Remove(dgTabizlazniparam.SelectedItem as CTabIzlazniParametri);
            Listasaradnika.Remove(dgSarad.SelectedItem as CSaradnici);
        }
        #endregion

        #region SPECIJALIZOVANI ULAZNI PARAMETRI

        #region UKLONI SPECIJALIZOVANI ULAZNI PARANETAR BUTTON
        private void btnUkloniSpulparam_Click(object sender, RoutedEventArgs e)
        {
            txtNazivspeculparam.Clear();
            rbOpisSpeculparam.Document.Blocks.Clear();
            txtDefspulparam.Clear();
            txtTipspeculparam.Clear();
            int index = dgSpeculparam.SelectedIndex;
            DeleteULP(index);
            dgSpeculparam.ItemsSource = ListaSpeculparam;

            CIzvestaj.listaSpeculparam.Clear();
            foreach (CSpecUlparam spulp in ListaSpeculparam)
            {
                CIzvestaj.listaSpeculparam.Add(spulp);
            }
        }
        #endregion

        #region SPECIJALIZOVANI ULAZNI PARAMETRI SELECTONCHANGED
        private void dgSpeculparam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string naziv = (dgSpeculparam.SelectedItem as CSpecUlparam).specNazivulazniparametri.ToString();
                txtNazivspeculparam.Text = naziv;
                rbOpisSpeculparam.Document.Blocks.Clear();
                rbOpisSpeculparam.AppendText((dgSpeculparam.SelectedItem as CSpecUlparam).specOpisulazniparam);
                txtDefspulparam.Text = (dgSpeculparam.SelectedItem as CSpecUlparam).specDefaultulaznogparam.ToString();
                txtObavezSpeculparam.Text = (dgSpeculparam.SelectedItem as CSpecUlparam).specObaveznostulaznogparam.ToString();
                txtTipspeculparam.Text = (dgSpeculparam.SelectedItem as CSpecUlparam).specTipulparam.ToString();
            }
            catch (Exception)
            {
                return;

            }
        }
        #endregion

        #region OBAVEZNOST SPECIJALIZOVANOG ULAZNOG PARAMETRA
        private void cbspulpda_Click(object sender, RoutedEventArgs e)
        {
            if (cbspulpda.IsChecked == true)
            {
                txtObavezSpeculparam.Text = "Da";
                cbspulpda.BorderBrush = Brushes.Blue;

            }
            else
            {
                cbspulpda.IsChecked = false;
                cbspulpda.BorderBrush = Brushes.Red;
                txtObavezSpeculparam.Text = "Ne";
            }
        }

        private void cbspulpne_Click(object sender, RoutedEventArgs e)
        {
            if (cbspulpne.IsChecked == true)
            {
                txtObavezSpeculparam.Text = "Ne";
                cbspulpne.BorderBrush = Brushes.Blue;
                return;
            }
            else
            {
                cbspulpne.BorderBrush = Brushes.Red;
            }

        }



        private void txtObavezSpeculparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
            if (txtObavezSpeculparam.Text == "Ne")
            {
                cbspulpne.IsChecked = true;
                return;
            }
            if (txtObavezSpeculparam.Text == "Da")
            {
                cbspulpda.IsChecked = true;
                return;
            }

        }

        private void Ne_Click(object sender, RoutedEventArgs e)
        {
            if (Ne.IsChecked == true)
            {
                txtobavulparam.Text = "Ne";
            }
        }
        #endregion

        #region OMOGUCAVA IZMENE SPECIJALIZOVANIH ULAZNIH PARAMETARA

        private void Otkljucanospulp_Click(object sender, RoutedEventArgs e)
        {
            if (Otkljucanospulp.IsChecked == true)
            {
                CIzvestaj.listaSpeculparam.Clear();
                foreach (CSpecUlparam speculp in ListaSpeculparam)
                {
                    CIzvestaj.listaSpeculparam.Add(speculp);
                }
                txtNazivspeculparam.IsEnabled = true;
                txtObavezSpeculparam.IsEnabled = true;
                rbOpisSpeculparam.IsEnabled = true;
                txtDefspulparam.IsEnabled = true;
                dgSpeculparam.IsEnabled = true;
                cbspulpda.IsEnabled = true;
                cbspulpne.IsEnabled = true;
                btnDodajSpulparam.IsEnabled = true;
                btnUkloniSpulparam.IsEnabled = true;
            }
        }
        #endregion

        #region ZAKLJUCAVA SPECIJALIZOVANE ULAZNE PARAMETRE I SALJE IHU IZVESTAJ
        private void Zakljucajspulp_Click(object sender, RoutedEventArgs e)
        {
            if (Zakljucajspulp.IsChecked == true)
            {
                CIzvestaj.listaSpeculparam.Clear();
                foreach (CSpecUlparam speculp in ListaSpeculparam)
                {
                    CIzvestaj.listaSpeculparam.Add(speculp);
                }
                txtNazivspeculparam.IsEnabled = false;
                txtObavezSpeculparam.IsEnabled = false;
                rbOpisSpeculparam.IsEnabled = false;
                txtDefspulparam.IsEnabled = false;
                dgSpeculparam.IsEnabled = false;
                cbspulpda.IsEnabled = false;
                cbspulpne.IsEnabled = false;
                btnDodajSpulparam.IsEnabled = false;
                btnUkloniSpulparam.IsEnabled = false;
            }
        }
        #endregion

        #region TextChangedTipspeculparam
        private void txtTipspeculparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (txtTipspeculparam.Text != "")
            {
                txtTipspeculparam.BorderBrush = Brushes.Blue;
                txtTipspeculparam.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txtTipspeculparam.BorderBrush = Brushes.Red;
                txtTipspeculparam.BorderThickness = new Thickness(1);
            }
        }
        #endregion

        #endregion

        #region TABELARNI ULAZNI PARAMETRI
        private void txtNazivtabulparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (txtNazivtabulparam.Text != "")
            {
                lbltnaziv.Content = txtNazivtabulparam.Text;
                lbltnaziv.Visibility = Visibility.Hidden;
                txtNazivtabulparam.BorderBrush = Brushes.Blue;
                txtNazivtabulparam.BorderThickness = new Thickness(0.5);
            }
            if (txtNazivtabulparam.Text != "")
            {
                txtNazivtabulparam.BorderBrush = Brushes.Blue;
                txtNazivtabulparam.BorderThickness = new Thickness(0.5);
            }


        }

        private void txtNazivtabulparam_LostFocus(object sender, RoutedEventArgs e)
        {
            txtNazivtabulparam.IsEnabled = false;
            txtNazivtabulparam.BorderBrush = Brushes.Blue;
            txtNazivtabulparam.BorderThickness = new Thickness(0.5);
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }

        }


        #region OBRISI TABELARNE ULAZNE PARAMETRE
        private void btnobrisitabulp_Click(object sender, RoutedEventArgs e)
        {
            int index = dgTabulparam.SelectedIndex;
            txtNazivtabulparam.Clear();
            txtPoletabulparam.Clear();
            txttipolparam.Clear();
            txtduztabulparam.Clear();
            txtInfosys.Clear();
            rbOpistabulparam.Document.Blocks.Clear();


            DeleteULP(index);
            dgTabulparam.ItemsSource = ListaTabulparam;
        }

        #endregion

        private void brNovatabela_Click(object sender, RoutedEventArgs e)
        {
            if (brNovatabela.IsChecked == true)
            {
                txtNazivtabulparam.IsEnabled = true;
                txtNazivtabulparam.Clear();
            }
        }

        private void Zakljucajtabulp_Click(object sender, RoutedEventArgs e)
        {
            if (Zakljucajtabulp.IsChecked == true)
            {
                CIzvestaj.listaTabulparamizv.Clear();

                foreach (CTabUlparam item in ListaTabulparam)
                {
                    CIzvestaj.listaTabulparamizv.Add(item);
                }
                txtNazivtabulparam.IsEnabled = false;
                txtPoletabulparam.IsEnabled = false;
                txttipolparam.IsEnabled = false;
                txtduztabulparam.IsEnabled = false;
                txtInfosys.IsEnabled = false;
                rbOpistabulparam.IsEnabled = false;
                dgTabulparam.IsEnabled = false;
                btnDodajtabulparam.IsEnabled = true;
                btnobrisitabulp.IsEnabled = true;
            }
        }
        private void Otkljucanotabulp_Click(object sender, RoutedEventArgs e)
        {
            if (Otkljucanotabulp.IsChecked == true)
            {
                CIzvestaj.listaTabulparamizv.Clear();

                foreach (CTabUlparam item in ListaTabulparam)
                {
                    CIzvestaj.listaTabulparamizv.Add(item);
                }
                txtNazivtabulparam.IsEnabled = true;
                txtPoletabulparam.IsEnabled = true;
                txttipolparam.IsEnabled = true;
                txtduztabulparam.IsEnabled = true;
                txtInfosys.IsEnabled = true;
                rbOpistabulparam.IsEnabled = true;
                dgTabulparam.IsEnabled = true;
            }
        }

        private void dgTabulparam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string naziv = (dgTabulparam.SelectedItem as CTabUlparam).Nazivtabelarniulparam.ToString();
                txtNazivtabulparam.Text = naziv;

                txtPoletabulparam.Text = (dgTabulparam.SelectedItem as CTabUlparam).Poljetabelarniulparam.ToString();
                txttipolparam.Text = (dgTabulparam.SelectedItem as CTabUlparam).Tipulparam.ToString();
                txtduztabulparam.Text = (dgTabulparam.SelectedItem as CTabUlparam).Duzinapoljaulparam.ToString();
                txtInfosys.Text = (dgTabulparam.SelectedItem as CTabUlparam).Infosusulparam.ToString();
                rbOpistabulparam.Document.Blocks.Clear();
                rbOpistabulparam.AppendText((dgTabulparam.SelectedItem as CTabUlparam).Opistabelarniulparam);
            }
            catch (Exception)
            {

                return;
            }
        }

        #endregion

        #region STATUSI GRESKI
        private void btnDodajstgreske_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string pathsg = Path.Combine(path, "Trenutno.txt");
            CStatusGreske statusgr = new CStatusGreske();
            statusgr.Identigikatorgreske = txtIdentifgreske.Text;
            statusgr.Tipgreske = txtTiogreske.Text;
            statusgr.Opisgreske = new TextRange(rbopisgreske.Document.ContentStart, rbopisgreske.Document.ContentEnd).Text;

            ListaGreske.Add(statusgr);

            dgstatusgreske.ItemsSource = ListaGreske;

            List<string> listagr = new List<string>();

            foreach (var item in ListaGreske)
            {
                listagr.Add(item.Identigikatorgreske + ";" + item.Tipgreske + ";" + item.Opisgreske + ";");
            }

            File.WriteAllText(pathsg, String.Empty);
            File.WriteAllLines(pathsg, listagr);

            txtIdentifgreske.Clear();
            txtTiogreske.Clear();
            rbopisgreske.Document.Blocks.Clear();
            txtIdentifgreske.Focus();
            ApiParametri.SinhroniyacijaListi = true;
            if (ApiParametri.SinhroniyacijaListi == true)
            {
                Sinhronizacija_Executed(null, null);
            }
        }

        private void obrisisatgr_Click(object sender, RoutedEventArgs e)
        {
            int index = dgstatusgreske.SelectedIndex;
            DeleteULP(index);
            dgstatusgreske.ItemsSource = ListaGreske;
            txtIdentifgreske.Clear();
            txtTiogreske.Clear();
            rbopisgreske.Document.Blocks.Clear();

        }

        private void dgstatusgreske_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtIdentifgreske.Text = (dgstatusgreske.SelectedItem as CStatusGreske).Identigikatorgreske.ToString();
                txtTiogreske.Text = (dgstatusgreske.SelectedItem as CStatusGreske).Tipgreske.ToString();
                rbopisgreske.Document.Blocks.Clear();
                rbopisgreske.AppendText((dgstatusgreske.SelectedItem as CStatusGreske).Opisgreske);
            }
            catch (Exception)
            {

                return;
            }
        }

        private void rbZakljucajstatus_Click(object sender, RoutedEventArgs e)
        {
            if (rbZakljucajstatus.IsChecked == true)
            {
                CIzvestaj.listaGreskeIzv.Clear();

                foreach (CStatusGreske statgr in ListaGreske)
                {
                    CIzvestaj.listaGreskeIzv.Add(statgr);
                }
                txtIdentifgreske.IsEnabled = false;
                txtTiogreske.IsEnabled = false;
                rbOpistabulparam.IsEnabled = false;
                dgstatusgreske.IsEnabled = false;
                btnDodajstgreske.IsEnabled = false;
                obrisisatgr.IsEnabled = false;
                rbopisgreske.IsEnabled = false;
            }
        }

        private void rbOtkljucajstatus_Click(object sender, RoutedEventArgs e)
        {
            if (rbOtkljucajstatus.IsChecked == true)
            {
                txtIdentifgreske.IsEnabled = true;
                txtTiogreske.IsEnabled = true;
                rbopisgreske.IsEnabled = true;
                dgstatusgreske.IsEnabled = true;
                btnDodajstgreske.IsEnabled = true;
                obrisisatgr.IsEnabled = true;
            }
        }
        #endregion

        #region IZLAZNE VREDNOSTI
        private void btnUkloniizlvr_Click(object sender, RoutedEventArgs e)
        {
            int index = dgIzlaznevr.SelectedIndex;
            txtIzlvrn.Clear();
            rbopisizlvr.Document.Blocks.Clear();
            DeleteULP(index);
            dgIzlaznevr.ItemsSource = Listaizlaznihvr;
        }

        private void rbZakljucajizvr_Click(object sender, RoutedEventArgs e)
        {
            if (rbZakljucajizvr.IsChecked == true)
            {
                CIzvestaj.Listaizlaznevr.Clear();
                foreach (CIzlaznevr izlvr in Listaizlaznihvr)
                {
                    CIzvestaj.Listaizlaznevr.Add(izlvr);
                }
                txtIzlvrn.IsEnabled = false;
                rbopisizlvr.IsEnabled = false;
                dgIzlaznevr.IsEnabled = false;
                btnDodajizlvr.IsEnabled = false;
                btnUkloniizlvr.IsEnabled = false;
            }
        }

        private void rbotkljucanoizlazvr_Click(object sender, RoutedEventArgs e)
        {
            txtIzlvrn.IsEnabled = true;
            rbopisizlvr.IsEnabled = true;
            dgIzlaznevr.IsEnabled = true;
            btnDodajizlvr.IsEnabled = true;
            btnUkloniizlvr.IsEnabled = true;
        }

        private void dgIzlaznevr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtIzlvrn.Text = (dgIzlaznevr.SelectedItem as CIzlaznevr).Nazivizlaznevr.ToString();
                rbopisizlvr.Document.Blocks.Clear();
                rbopisizlvr.AppendText((dgIzlaznevr.SelectedItem as CIzlaznevr).Opisizlaznevr);
            }
            catch (Exception)
            {

                return;
            }
        }

        #endregion

        #region SPECIJALIZOVANI IZLAZNI PARAMETRI
        private void btndodajspulp_Click(object sender, RoutedEventArgs e)
        {
            CSpizlazvr spizlazvr = new CSpizlazvr();
            spizlazvr.Nazivspizlaznevr = txtnazspizlvr.Text;
            spizlazvr.Defizlazvr = txtDefizlazvr.Text;
            spizlazvr.Obaveznost = txtobaveznospizlazvr.Text;
            spizlazvr.Opisspizlaznevr = new TextRange(rbspizlvr.Document.ContentStart, rbspizlvr.Document.ContentEnd).Text;
            spizlazvr.TipSpecijalneIzlaznevrednosti = txtTipspecizlaznevr.Text;

            Listaspizlaznihvr.Add(spizlazvr);
            CIzvestaj.SPltizlazvr.Add(spizlazvr);
            dgspizlazvre.ItemsSource = Listaspizlaznihvr;

            List<string> tr = new List<string>();
            foreach (var item in Listaspizlaznihvr)
            {
                tr.Add(item.Nazivspizlaznevr + ";" + item.TipSpecijalneIzlaznevrednosti + ";" + item.Opisspizlaznevr + ";");
            }

            File.WriteAllText("TrenutnoSpizlvr.txt", String.Empty);
            File.WriteAllLines("TrenutnoSpizlvr.txt", tr);

            txtnazspizlvr.Clear();
            rbspizlvr.Document.Blocks.Clear();
            txtDefizlazvr.Clear();
            txtobaveznospizlazvr.Clear();
            txtTipspecizlaznevr.Clear();
            txtnazspizlvr.Focus();

            ApiParametri.PagingSelectedPage = true;

            ApiParametri.SinhroniyacijaListi = true;
            if (ApiParametri.SinhroniyacijaListi == true)
            {
                Sinhronizacija_Executed(null, null);
            }

        }

        private void btnobrisispulp_Click(object sender, RoutedEventArgs e)
        {
            int index = dgspizlazvre.SelectedIndex;
            DeleteULP(index);
            dgspizlazvre.ItemsSource = Listaspizlaznihvr;
            txtnazspizlvr.Clear();
            rbspizlvr.Document.Blocks.Clear();
            txtDefizlazvr.Clear();
            txtTipspecizlaznevr.Clear();
        }

        private void Zakljucajspizlazvr_Click(object sender, RoutedEventArgs e)
        {
            if (Zakljucajspizlazvr.IsChecked == true)
            {
                CIzvestaj.SPltizlazvr.Clear();
                foreach (CSpizlazvr izlvr in Listaspizlaznihvr)
                {
                    CIzvestaj.SPltizlazvr.Add(izlvr);
                }
                txtnazspizlvr.IsEnabled = false;
                rbspizlvr.IsEnabled = false;
                btndodajspulp.IsEnabled = false;
                btnobrisispulp.IsEnabled = false;
                dgspizlazvre.IsEnabled = false;
                txtDefizlazvr.IsEnabled = false;
                txtobaveznospizlazvr.IsEnabled = false;
                btndodajspulp.IsEnabled = false;
                btnobrisispulp.IsEnabled = false;
            }
        }

        private void Otkljucanjspizlvr_Click(object sender, RoutedEventArgs e)
        {
            txtnazspizlvr.IsEnabled = true;
            rbspizlvr.IsEnabled = true;
            btndodajspulp.IsEnabled = true;
            btnobrisispulp.IsEnabled = true;
            dgspizlazvre.IsEnabled = true;
            txtDefizlazvr.IsEnabled = true;
            txtobaveznospizlazvr.IsEnabled = true;
            btndodajspulp.IsEnabled = true;
            btnobrisispulp.IsEnabled = true;
        }


        private void txtTipspecizlaznevr_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (txtTipspecizlaznevr.Text != "")
            {
                txtTipspecizlaznevr.BorderBrush = Brushes.Blue;
                txtTipspecizlaznevr.BorderThickness = new Thickness(0.5);

            }
            else
            {
                txtTipspecizlaznevr.BorderBrush = Brushes.Red;
                txtTipspecizlaznevr.BorderThickness = new Thickness(1);
            }
        }

        #endregion

        #region DODATAK
        private void btnDodajdodatak_Click(object sender, RoutedEventArgs e)
        {
            CDodatak dodatak = new CDodatak();
            dodatak.Dodataknaziv = txtNazivDodatka.Text;
            dodatak.Dodataktip = txtTipDodatka.Text;
            dodatak.Opisdodatka = new TextRange(rbOpisdodatka.Document.ContentStart, rbOpisdodatka.Document.ContentEnd).Text;
            dodatak.NapomeanaDodatak = new TextRange(rbDodatakNapomena.Document.ContentStart, rbDodatakNapomena.Document.ContentEnd).Text;
            Listadodataka.Add(dodatak);
            CIzvestaj.listaDodatakizv.Add(dodatak);
            dgDodatak.ItemsSource = Listadodataka;

            txtNazivDodatka.Clear();
            txtTipDodatka.Clear();
            rbOpisdodatka.Document.Blocks.Clear();
            rbDodatakNapomena.Document.Blocks.Clear();
            txtNazivDodatka.Focus();



        }

        private void btnobrisidodatak_Click(object sender, RoutedEventArgs e)
        {
            int index = dgDodatak.SelectedIndex;

            DeleteULP(index);
            dgDodatak.ItemsSource = Listadodataka;

            txtNazivDodatka.Clear();
            txtTipDodatka.Clear();
            rbOpisdodatka.Document.Blocks.Clear();
            rbDodatakNapomena.Document.Blocks.Clear();
        }

        private void ZakljucajDodatak_Click(object sender, RoutedEventArgs e)
        {
            if (ZakljucajDodatak.IsChecked == true)
            {
                CIzvestaj.listaDodatakizv.Clear();
                foreach (CDodatak dodatak in Listadodataka)
                {
                    CIzvestaj.listaDodatakizv.Add(dodatak);
                }
                txtNazivDodatka.IsEnabled = false;
                txtTipDodatka.IsEnabled = false;
                rbOpisdodatka.IsEnabled = false;
                btnDodajdodatak.IsEnabled = false;
                btnobrisidodatak.IsEnabled = false;
                dgDodatak.IsEnabled = false;
            }
        }

        private void OtkljucajDodatak_Click(object sender, RoutedEventArgs e)
        {
            txtNazivDodatka.IsEnabled = true;
            txtTipDodatka.IsEnabled = true;
            rbOpisdodatka.IsEnabled = true;
            btnDodajdodatak.IsEnabled = true;
            btnobrisidodatak.IsEnabled = true;
            dgDodatak.IsEnabled = true;

        }

        private void dgDodatak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtNazivDodatka.Text = ((dgDodatak.SelectedItem as CDodatak).Dodataknaziv).ToString();
                txtTipDodatka.Text = ((dgDodatak.SelectedItem as CDodatak).Dodataktip).ToString();
                cbTipdodatak.SelectedItem = ((dgDodatak.SelectedItem as CDodatak).Dodataktip).ToString();
                rbOpisdodatka.Document.Blocks.Clear();
                rbOpisdodatka.AppendText((dgDodatak.SelectedItem as CDodatak).Opisdodatka);
                rbDodatakNapomena.Document.Blocks.Clear();
                rbDodatakNapomena.AppendText((dgDodatak.SelectedItem as CDodatak).NapomeanaDodatak);
            }
            catch (Exception)
            {

                return;
            }
        }
        #endregion

        #region CLOSING WINODW

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CPositionHelper.SaveSize(this);

            if (ApiParametri.VidljivostSaveAs == true)
            {

                this.SaveAs_Executed(null, null);
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                //StanjeSaveAsKomande = false;
                //  MessageBox.Show("ne visdljiva komanda SAVE AS");
                // Process.GetCurrentProcess().Kill();
            }

            if (ApiParametri.VidljivostkomandeSave == true)
            {

                this.Save_Executed(null, null);
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                // ApiParametri.VidljivostkomandeSave = false;
                //  MessageBox.Show("ne visdljiva komanda SAVE ");
                //  Process.GetCurrentProcess().Kill();
            }

            if (ApiParametri.VidljivostSaveAs == false && ApiParametri.VidljivostkomandeSave == false)
            {
                Process.GetCurrentProcess().Kill();
            }




        }

        #endregion

        #region VALIDACIJA

        #region EXPANDER OPIS FUNKCIJE
        private void Opisfunkcije_MouseEnter(object sender, MouseEventArgs e)
        {


            OtkljucajSve_Executed(null, null);

            

            if (string.IsNullOrWhiteSpace(txtnazivfunkcija.Text) && StanjeSaveAsKomande == true)
            {
                txtnazivfunkcija.BorderBrush = Brushes.Red;
                txtnazivfunkcija.BorderThickness = new Thickness(1);

                ApiParametri.Sacuvano = false;
                ApiParametri.VidljivostkomandeSave = false;


            }

            if (ApiParametri.ForsiraniParametri == "True")
            {
                if (string.IsNullOrWhiteSpace(txtModul.Text))
                {
                    txtModul.BorderBrush = Brushes.Red;
                    txtModul.BorderThickness = new Thickness(1);
                    label6.Content = "* Modul:";
                    label6.Foreground = new SolidColorBrush(Colors.Red);
                    lforsiraniparametarmodul = true;
                    ApiParametri.VidljivostkomandeSave = false;
                    StanjeSaveAsKomande = false;
                }
                else
                {
                    label6.Content = "Modul:";
                    label6.Foreground = new SolidColorBrush(Colors.Black);
                    ApiParametri.VidljivostkomandeSave = true;
                    StanjeSaveAsKomande = true;
                    btnsave.IsEnabled = true;
                    lforsiraniparametarmodul = false;
                }

                if (string.IsNullOrWhiteSpace(new TextRange(rbOpisfunkcije.Document.ContentStart, rbOpisfunkcije.Document.ContentEnd).Text))
                {
                    rbOpisfunkcije.BorderBrush = Brushes.Red;
                    rbOpisfunkcije.BorderThickness = new Thickness(1);
                    label1.Content = "* Opis funkcije";
                    label1.Foreground = new SolidColorBrush(Colors.Red);
                    ApiParametri.VidljivostkomandeSave = false;
                    StanjeSaveAsKomande = false;
                }
                else
                {
                    label1.Content = "Opis funkcije";
                    label1.Foreground = new SolidColorBrush(Colors.Black);
                    ApiParametri.VidljivostkomandeSave = true;
                    StanjeSaveAsKomande = true;
                }

                if (string.IsNullOrWhiteSpace(new TextRange(rbopisLogikefunkcije.Document.ContentStart, rbopisLogikefunkcije.Document.ContentEnd).Text))
                {

                    rbopisLogikefunkcije.BorderBrush = Brushes.Red;
                    rbopisLogikefunkcije.BorderThickness = new Thickness(1);

                    label2.Content = "* Opis logike funkcije";
                    label2.Foreground = new SolidColorBrush(Colors.Red);
                    ApiParametri.VidljivostkomandeSave = false;
                    StanjeSaveAsKomande = false;

                }
                else
                {
                    label2.Content = "Opis logike funkcije";
                    label2.Foreground = new SolidColorBrush(Colors.Black);
                    ApiParametri.VidljivostkomandeSave = true;
                    StanjeSaveAsKomande = true;
                }

                if (string.IsNullOrWhiteSpace(new TextRange(rbScenario.Document.ContentStart, rbScenario.Document.ContentEnd).Text))
                {

                    rbScenario.BorderBrush = Brushes.Red;
                    rbScenario.BorderThickness = new Thickness(1);
                    ApiParametri.VidljivostkomandeSave = false;
                    StanjeSaveAsKomande = false;

                }
                else
                {
                    rbScenario.BorderBrush = Brushes.Black;
                    rbScenario.BorderThickness = new Thickness(1);
                    ApiParametri.VidljivostkomandeSave = true;
                    StanjeSaveAsKomande = true;
                }

                if (string.IsNullOrWhiteSpace(new TextRange(rbOgrucestfunkc.Document.ContentStart, rbOgrucestfunkc.Document.ContentEnd).Text))
                {

                    rbOgrucestfunkc.BorderBrush = Brushes.Red;
                    rbOgrucestfunkc.BorderThickness = new Thickness(1);

                }

            }
            else
            {
                label1.Content = "Opis funkcije";
                label1.Foreground = new SolidColorBrush(Colors.Black);
                label2.Content = "Opis logike funkcije";
                label2.Foreground = new SolidColorBrush(Colors.Black);

            }


        }

        private void Textchetest(object sender, RoutedEventArgs e)
        {
            if (txtnazivfunkcija.Text != "")
            {
                ApiParametri.Sacuvano = true;
                txtnazivfunkcija.BorderBrush = Brushes.Blue;
                txtnazivfunkcija.BorderThickness = new Thickness(1);
                Tabitem.Header = txtnazivfunkcija.Text;
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];
                ApiParametri.VidljivostkomandeSave = true;
                if (ApiParametri.VidljivostkomandeSave == true && ApiParametri.Sacuvano == false)
                {
                    btnsave.IsEnabled = true;
                }

            }
            else
            {
                txtnazivfunkcija.BorderBrush = Brushes.Red;
                txtnazivfunkcija.BorderThickness = new Thickness(1);
            }


            if (ucitanfajl == false)
            {
                ApiParametri.VidljivostkomandeSave = false;
                if (txtnazivfunkcija.Text != "")
                {
                    txtnazivfunkcija.BorderBrush = Brushes.Blue;
                    txtnazivfunkcija.BorderThickness = new Thickness(0.5);


                    ApiParametri.VidljivostkomandeSave = true;
                }
                else
                {
                    txtnazivfunkcija.BorderBrush = Brushes.Red;
                    txtnazivfunkcija.BorderThickness = new Thickness(1);
                    ApiParametri.VidljivostkomandeSave = false;
                    ucitanfajl = false;

                }

            }
        }

        public bool lforsiraniparametarmodul = true;
        private void txtModul_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (txtModul.Text != "")
            {
                txtModul.BorderBrush = Brushes.Blue;
                txtModul.BorderThickness = new Thickness(1);
                label6.Content = "Modul:";
                label6.Foreground = new SolidColorBrush(Colors.Black);
                lforsiraniparametarmodul = false;
                ApiParametri.VidljivostkomandeSave = true;
            }
            else
            {
                txtModul.BorderBrush = Brushes.Red;
                txtModul.BorderThickness = new Thickness(1);
                lforsiraniparametarmodul = false;
                label6.Content = "* Modul:";
                label6.Foreground = new SolidColorBrush(Colors.Red);
                lforsiraniparametarmodul = true;

            }
        }

        public bool lForsiraniparametarOpisfunkcije = true;
        private void rbOpisfunkcije_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (string.IsNullOrWhiteSpace(new TextRange(rbOpisfunkcije.Document.ContentStart, rbOpisfunkcije.Document.ContentEnd).Text))
            {

                rbOpisfunkcije.BorderBrush = Brushes.Red;
                rbOpisfunkcije.BorderThickness = new Thickness(1);
                label1.Content = "* Opis funkcije";
                label1.Foreground = new SolidColorBrush(Colors.Red);
                lForsiraniparametarOpisfunkcije = true;
            }
            else
            {
                rbOpisfunkcije.BorderBrush = Brushes.Blue;
                rbOpisfunkcije.BorderThickness = new Thickness(1);
                label1.Content = "Opis funkcije";
                label1.Foreground = new SolidColorBrush(Colors.Black);
                lForsiraniparametarOpisfunkcije = false;
            }

        }
        public bool lForsiraniparametriOpislogike = true;
        private void rbopisLogikefunkcije_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (string.IsNullOrWhiteSpace(new TextRange(rbopisLogikefunkcije.Document.ContentStart, rbopisLogikefunkcije.Document.ContentEnd).Text))
            {

                rbopisLogikefunkcije.BorderBrush = Brushes.Red;
                rbopisLogikefunkcije.BorderThickness = new Thickness(1);
                label2.Content = "* Opis logike funkcije";
                label2.Foreground = new SolidColorBrush(Colors.Red);
                lForsiraniparametriOpislogike = true;

            }
            else
            {
                rbopisLogikefunkcije.BorderBrush = Brushes.Blue;
                rbopisLogikefunkcije.BorderThickness = new Thickness(1);
                label2.Content = "Opis logike funkcije";
                label2.Foreground = new SolidColorBrush(Colors.Black);
                lForsiraniparametriOpislogike = false;
            }
        }

        private void rbOgrucestfunkc_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (string.IsNullOrWhiteSpace(new TextRange(rbOgrucestfunkc.Document.ContentStart, rbOgrucestfunkc.Document.ContentEnd).Text))
            {

                rbOgrucestfunkc.BorderBrush = Brushes.Red;
                rbOgrucestfunkc.BorderThickness = new Thickness(1);


            }
            else
            {
                rbOgrucestfunkc.BorderBrush = Brushes.Blue;
                rbOgrucestfunkc.BorderThickness = new Thickness(0.5);

            }
        }
        #endregion

        #region ULAZNI PARAMETRI

        private void ulazniparametri_MouseEnter(object sender, MouseEventArgs e)
        {
            OtkljucajSve_Executed(null, null);

            if (Da.IsChecked == false)
            {
                Da.BorderBrush = Brushes.Red;
                Da.BorderThickness = new Thickness(1);

            }
            else
            {
                Da.BorderBrush = Brushes.Blue;
                Da.BorderThickness = new Thickness(1);
                this.Da.IsChecked = true;
            }
            if (Ne.IsChecked == false)
            {
                Ne.BorderBrush = Brushes.Red;
                Ne.BorderThickness = new Thickness(1);
            }
            else
            {
                Ne.BorderBrush = Brushes.Blue;
                Ne.BorderThickness = new Thickness(1);
            }


            if (string.IsNullOrWhiteSpace(txtNazivulparam.Text))
            {
                txtNazivulparam.BorderBrush = Brushes.Red;
                txtNazivulparam.BorderThickness = new Thickness(1);
            }
            if (string.IsNullOrWhiteSpace(txtDuzinaUlaznogparametra.Text))
            {
                txtDuzinaUlaznogparametra.BorderBrush = Brushes.Red;
                txtDuzinaUlaznogparametra.BorderThickness = new Thickness(1);
            }

            if (string.IsNullOrWhiteSpace(txtdefulparam.Text))
            {
                txtdefulparam.BorderBrush = Brushes.Red;
                txtdefulparam.BorderThickness = new Thickness(1);
            }

            if (string.IsNullOrWhiteSpace(txtTipspeculparam.Text))
            {
                txtTipspeculparam.BorderBrush = Brushes.Red;
                txtTipspeculparam.BorderThickness = new Thickness(1);
            }

            if (string.IsNullOrWhiteSpace(txtTipspecizlaznevr.Text))
            {
                txtTipspecizlaznevr.BorderBrush = Brushes.Red;
                txtTipspecizlaznevr.BorderThickness = new Thickness(1);
            }

            if (string.IsNullOrWhiteSpace(new TextRange(rbOpisulp.Document.ContentStart, rbOpisulp.Document.ContentEnd).Text))
            {

                rbOpisulp.BorderBrush = Brushes.Red;
                rbOpisulp.BorderThickness = new Thickness(1);

            }

            if (string.IsNullOrWhiteSpace(txtTipUlaznogparametra.Text))
            {
                txtTipUlaznogparametra.BorderBrush = Brushes.Red;
                txtTipUlaznogparametra.BorderThickness = new Thickness(1);
            }


        }


        private void txtNazivulparam_TextChanged(object sender, TextChangedEventArgs e)
        {

            string tipstringaulp = txtNazivulparam.Text;

            List<string> Autoulp = new List<string>();

            foreach (string aulp in ALULP)
            {
                if (!string.IsNullOrEmpty(txtNazivulparam.Text))
                {
                    if (aulp.StartsWith(tipstringaulp))
                    {
                        Autoulp.Add(aulp);
                    }
                }
            }

            if (Autoulp.Count > 0)
            {
                lbAutoulazniparametri.ItemsSource = Autoulp;
                lbAutoulazniparametri.Visibility = Visibility.Visible;

            }
            else if (txtIdentifgreske.Text.Equals(" "))
            {
                lbAutoulazniparametri.Visibility = Visibility.Collapsed;
                lbAutoulazniparametri.ItemsSource = null;

            }
            else
            {
                lbAutoulazniparametri.Visibility = Visibility.Collapsed;
                lbAutoulazniparametri.ItemsSource = null;

            }


            ApiParametri.VidljivostkomandeSave = true;

            if (txtNazivulparam.Text != "")
            {
                txtNazivulparam.BorderBrush = Brushes.Blue;
                txtNazivulparam.BorderThickness = new Thickness(0.5);

            }
            else
            {
                txtNazivulparam.BorderBrush = Brushes.Red;
                txtNazivulparam.BorderThickness = new Thickness(0.5);
            }
        }

        private void txtdefulparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
            if (txtdefulparam.Text != "")
            {
                txtdefulparam.BorderBrush = Brushes.Blue;
                txtdefulparam.BorderThickness = new Thickness(0.5);

            }
            else
            {
                txtdefulparam.BorderThickness = new Thickness(0.5);
                txtdefulparam.BorderBrush = Brushes.Red;
            }
        }

        private void rbOpisulp_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
            if (string.IsNullOrWhiteSpace(new TextRange(rbOpisulp.Document.ContentStart, rbOpisulp.Document.ContentEnd).Text))
            {

                rbOpisulp.BorderBrush = Brushes.Red;
                rbOpisulp.BorderThickness = new Thickness(1);

            }
            else
            {
                rbOpisulp.BorderBrush = Brushes.Blue;
                rbOpisulp.BorderThickness = new Thickness(0.5);
            }
        }


        #endregion


        #region SCENARIO
        private void Scenario_MouseEnter(object sender, MouseEventArgs e)
        {
            OtkljucajSve_Executed(null, null);

            if (string.IsNullOrWhiteSpace(new TextRange(rbScenario.Document.ContentStart, rbScenario.Document.ContentEnd).Text))
            {

                rbScenario.BorderBrush = Brushes.Red;
                rbScenario.BorderThickness = new Thickness(1);


            }
        }
        public bool lscenarioforsiran = true;
        private void rbScenario_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;


            if (string.IsNullOrWhiteSpace(new TextRange(rbScenario.Document.ContentStart, rbScenario.Document.ContentEnd).Text))
            {

                rbScenario.BorderBrush = Brushes.Red;
                rbScenario.BorderThickness = new Thickness(1);
                lscenarioforsiran = true;


            }
            else
            {

                lscenarioforsiran = false;
                rbScenario.BorderBrush = Brushes.Blue;
                rbScenario.BorderThickness = new Thickness(0.5);
            }
        }


        #endregion


        #region SPECIJALIZOVANI ULAZNI PARAMETRI
        private void Spulparam_MouseEnter(object sender, MouseEventArgs e)
        {
            OtkljucajSve_Executed(null, null);

            if (string.IsNullOrWhiteSpace(txtNazivspeculparam.Text))
            {
                txtNazivspeculparam.BorderBrush = Brushes.Red;
                txtNazivspeculparam.BorderThickness = new Thickness(1);
            }


            if (string.IsNullOrWhiteSpace(txtDefspulparam.Text))
            {
                txtDefspulparam.BorderBrush = Brushes.Red;
                txtDefspulparam.BorderThickness = new Thickness(1);
            }

            if (string.IsNullOrWhiteSpace(new TextRange(rbOpisSpeculparam.Document.ContentStart, rbOpisSpeculparam.Document.ContentEnd).Text))
            {

                rbOpisSpeculparam.BorderBrush = Brushes.Red;
                rbOpisSpeculparam.BorderThickness = new Thickness(1);
            }
        }

        private void txtNazivspeculparam_TextChanged(object sender, TextChangedEventArgs e)
        {

            string tipstringaulp = txtNazivspeculparam.Text;
            List<string> lAutospeculp = new List<string>();
            lAutospeculp.Clear();

            foreach (string speculp in SPECULP)
            {
                if (!string.IsNullOrEmpty(txtNazivspeculparam.Text))
                {
                    if (speculp.StartsWith(tipstringaulp))
                    {
                        lAutospeculp.Add(speculp);
                    }
                }
            }
            if (lAutospeculp.Count > 0)
            {
                lbSpeculparamsugestija.ItemsSource = lAutospeculp;
                lbSpeculparamsugestija.Visibility = Visibility.Visible;
            }
            else if (txtNazivspeculparam.Text.Equals(" "))
            {
                lbSpeculparamsugestija.Visibility = Visibility.Collapsed;
                lbSpeculparamsugestija.ItemsSource = null;
            }
            else
            {
                lbSpeculparamsugestija.Visibility = Visibility.Collapsed;
                lbSpeculparamsugestija.ItemsSource = null;
            }

            ApiParametri.VidljivostkomandeSave = true;

            if (txtNazivspeculparam.Text != "")
            {
                txtNazivspeculparam.BorderBrush = Brushes.Blue;
                txtNazivspeculparam.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txtNazivspeculparam.BorderBrush = Brushes.Red;
                txtNazivspeculparam.BorderThickness = new Thickness(1);
            }




        }

        private void txtDefspulparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (txtDefspulparam.Text != "")
            {
                txtDefspulparam.BorderBrush = Brushes.Blue;
                txtDefspulparam.BorderThickness = new Thickness(0.5);

            }
            else
            {
                txtDefspulparam.BorderBrush = Brushes.Red;
                txtDefspulparam.BorderThickness = new Thickness(1);
            }
        }

        private void rbOpisSpeculparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
            if (string.IsNullOrWhiteSpace(new TextRange(rbOpisSpeculparam.Document.ContentStart, rbOpisSpeculparam.Document.ContentEnd).Text))
            {

                rbOpisSpeculparam.BorderBrush = Brushes.Red;
                rbOpisSpeculparam.BorderThickness = new Thickness(1);

            }
            else
            {
                rbOpisSpeculparam.BorderBrush = Brushes.Blue;
                rbOpisSpeculparam.BorderThickness = new Thickness(0.5);
            }
        }
        #endregion



        #region TABELARNI ULAZNI PARAMETRI
        private void Tabulparam_MouseEnter(object sender, MouseEventArgs e)
        {
            OtkljucajSve_Executed(null, null);

            if (string.IsNullOrWhiteSpace(txtNazivtabulparam.Text))
            {
                txtNazivtabulparam.BorderBrush = Brushes.Red;
                txtNazivtabulparam.BorderThickness = new Thickness(1);
            }
            else
            {
                txtNazivtabulparam.BorderBrush = Brushes.Blue;
                txtNazivtabulparam.BorderThickness = new Thickness(1);
            }

            if (string.IsNullOrWhiteSpace(txtPoletabulparam.Text))
            {
                txtPoletabulparam.BorderBrush = Brushes.Red;
                txtPoletabulparam.BorderThickness = new Thickness(1);
            }

            if (string.IsNullOrWhiteSpace(txttipolparam.Text))
            {
                txttipolparam.BorderBrush = Brushes.Red;
                txttipolparam.BorderThickness = new Thickness(1);

            }

            if (string.IsNullOrWhiteSpace(txtduztabulparam.Text))
            {
                txtduztabulparam.BorderBrush = Brushes.Red;
                txtduztabulparam.BorderThickness = new Thickness(1);


            }

            if (string.IsNullOrWhiteSpace(txtInfosys.Text))
            {
                txtInfosys.BorderBrush = Brushes.Red;
                txtInfosys.BorderThickness = new Thickness(1);
            }

            if (string.IsNullOrWhiteSpace(new TextRange(rbOpistabulparam.Document.ContentStart, rbOpistabulparam.Document.ContentEnd).Text))
            {

                rbOpistabulparam.BorderBrush = Brushes.Red;
                rbOpistabulparam.BorderThickness = new Thickness(1);

            }
            else
            {
                rbOpistabulparam.BorderBrush = Brushes.Blue;
                rbOpistabulparam.BorderThickness = new Thickness(1);
            }
        }

        private void txtPoletabulparam_TextChanged(object sender, TextChangedEventArgs e)
        {

            string tipstringaulp = txtPoletabulparam.Text;
            List<string> lAutotabulp = new List<string>();
            lAutotabulp.Clear();

            foreach (string tabulp in TABULPARAM)
            {
                if (!string.IsNullOrEmpty(txtPoletabulparam.Text))
                {
                    if (tabulp.StartsWith(tipstringaulp))
                    {
                        lAutotabulp.Add(tabulp);
                    }
                }
            }
            if (lAutotabulp.Count > 0)
            {
                lbTabelarniulparamSugestija.ItemsSource = lAutotabulp;
                lbTabelarniulparamSugestija.Visibility = Visibility.Visible;
            }
            else if (txtPoletabulparam.Text.Equals(" "))
            {
                lbTabelarniulparamSugestija.Visibility = Visibility.Collapsed;
                lbTabelarniulparamSugestija.ItemsSource = null;
            }
            else
            {
                lbTabelarniulparamSugestija.Visibility = Visibility.Collapsed;
                lbTabelarniulparamSugestija.ItemsSource = null;
            }



            //--------------------------------------------
            ApiParametri.VidljivostkomandeSave = true;

            if (txtPoletabulparam.Text != "")
            {
                txtPoletabulparam.BorderBrush = Brushes.Blue;
                txtPoletabulparam.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txtPoletabulparam.BorderBrush = Brushes.Red;
                txtPoletabulparam.BorderThickness = new Thickness(0.5);
            }
        }

        private void txttipolparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (txttipolparam.Text != "")
            {
                txttipolparam.BorderBrush = Brushes.Blue;
                txttipolparam.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txttipolparam.BorderBrush = Brushes.Red;
                txttipolparam.BorderThickness = new Thickness(1);
            }
        }

        private void txtduztabulparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (txtduztabulparam.Text != "")
            {
                txtduztabulparam.BorderBrush = Brushes.Blue;
                txtduztabulparam.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txtduztabulparam.BorderBrush = Brushes.Red;
                txtduztabulparam.BorderThickness = new Thickness(1);
            }
        }

        private void txtInfosys_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (txtInfosys.Text != "")
            {
                txtInfosys.BorderBrush = Brushes.Blue;
                txtInfosys.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txtInfosys.BorderBrush = Brushes.Red;
                txtInfosys.BorderThickness = new Thickness(1);
            }
        }

        private void rbOpistabulparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (string.IsNullOrWhiteSpace(new TextRange(rbOpistabulparam.Document.ContentStart, rbOpistabulparam.Document.ContentEnd).Text))
            {

                rbOpistabulparam.BorderBrush = Brushes.Blue;
                rbOpistabulparam.BorderThickness = new Thickness(0.5);


            }
            else
            {
                rbOpistabulparam.BorderBrush = Brushes.Red;
                rbOpistabulparam.BorderThickness = new Thickness(1);
            }
        }

        #endregion

        #region STATUSI GRESKI
        private void Statusigreski_MouseEnter(object sender, MouseEventArgs e)
        {
            OtkljucajSve_Executed(null, null);



            if (string.IsNullOrWhiteSpace(txtIdentifgreske.Text))
            {
                txtIdentifgreske.BorderBrush = Brushes.Red;
                txtIdentifgreske.BorderThickness = new Thickness(1);
            }

            if (string.IsNullOrWhiteSpace(txtTiogreske.Text))
            {
                txtTiogreske.BorderBrush = Brushes.Red;
                txtTiogreske.BorderThickness = new Thickness(1);
            }

            if (string.IsNullOrWhiteSpace(new TextRange(rbopisgreske.Document.ContentStart, rbopisgreske.Document.ContentEnd).Text))
            {

                rbopisgreske.BorderBrush = Brushes.Red;
                rbopisgreske.BorderThickness = new Thickness(1);
            }
        }

        private void txtIdentifgreske_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            string tipstringaulp = txtIdentifgreske.Text;

            List<string> Autolsgr = new List<string>();


            foreach (string ausgr in SG)
            {
                if (!string.IsNullOrEmpty(txtIdentifgreske.Text))
                {

                    if (ausgr.StartsWith(tipstringaulp))
                    {
                        Autolsgr.Add(ausgr);
                    }
                }
            }

            if (Autolsgr.Count > 0)
            {
                lbAutosugrstijaGresaka.ItemsSource = Autolsgr;
                lbAutosugrstijaGresaka.Visibility = Visibility.Visible;

            }
            else if (txtIdentifgreske.Text.Equals(" "))
            {
                lbAutosugrstijaGresaka.Visibility = Visibility.Collapsed;
                lbAutosugrstijaGresaka.ItemsSource = null;

            }
            else
            {
                lbAutosugrstijaGresaka.Visibility = Visibility.Collapsed;
                lbAutosugrstijaGresaka.ItemsSource = null;

            }

            //***********************************************************


            if (txtIdentifgreske.Text != "")
            {
                txtIdentifgreske.BorderBrush = Brushes.Blue;
                txtIdentifgreske.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txtIdentifgreske.BorderBrush = Brushes.Red;
                txtIdentifgreske.BorderThickness = new Thickness(1);
            }
        }

        private void txtTiogreske_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;



            if (txtTiogreske.Text != "")
            {

                txtTiogreske.BorderBrush = Brushes.Blue;
                txtTiogreske.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txtTiogreske.BorderBrush = Brushes.Red;
                txtTiogreske.BorderThickness = new Thickness(1);
            }
        }

        private void rbopisgreske_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (string.IsNullOrWhiteSpace(new TextRange(rbopisgreske.Document.ContentStart, rbopisgreske.Document.ContentEnd).Text))
            {

                rbopisgreske.BorderBrush = Brushes.Red;
                rbopisgreske.BorderThickness = new Thickness(0.5);
            }
            else
            {
                rbopisgreske.BorderBrush = Brushes.Blue;
                rbopisgreske.BorderThickness = new Thickness(0.5);
            }
        }
        #endregion

        #region IZLAZNE VREDNOSTI
        private void Izlaznevrednosti_MouseEnter(object sender, MouseEventArgs e)
        {
            OtkljucajSve_Executed(null, null);

            if (string.IsNullOrWhiteSpace(txtIzlvrn.Text))
            {
                txtIzlvrn.BorderBrush = Brushes.Red;
                txtIzlvrn.BorderThickness = new Thickness(1);
            }

            if (string.IsNullOrWhiteSpace(new TextRange(rbopisizlvr.Document.ContentStart, rbopisizlvr.Document.ContentEnd).Text))
            {

                rbopisizlvr.BorderBrush = Brushes.Red;
                rbopisizlvr.BorderThickness = new Thickness(1);
            }

        }

        private void rbopisizlvr_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (string.IsNullOrWhiteSpace(new TextRange(rbopisizlvr.Document.ContentStart, rbopisizlvr.Document.ContentEnd).Text))
            {

                rbopisizlvr.BorderBrush = Brushes.Red;
                rbopisizlvr.BorderThickness = new Thickness(0.5);
            }
            else
            {
                rbopisizlvr.BorderBrush = Brushes.Blue;
                rbopisizlvr.BorderThickness = new Thickness(0.5);
            }
        }

        private void txtIzlvrn_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            string tipstringa = txtIzlvrn.Text;
            List<string> Autoizlvr = new List<string>();
            Autoizlvr.Clear();
            foreach (string asg in IZLAZNEVR)
            {
                if (!string.IsNullOrEmpty(txtIzlvrn.Text))
                {
                    if (asg.StartsWith(tipstringa))
                    {
                        Autoizlvr.Add(asg);
                    }
                }
            }
            if (Autoizlvr.Count > 0)
            {
                lbIzlazneSugestija.ItemsSource = Autoizlvr;
                lbIzlazneSugestija.Visibility = Visibility.Visible;
            }
            else if (txtIzlvrn.Text.Equals(" "))
            {
                lbIzlazneSugestija.Visibility = Visibility.Collapsed;
                lbIzlazneSugestija.ItemsSource = null;
            }
            else
            {
                lbIzlazneSugestija.Visibility = Visibility.Collapsed;
                lbIzlazneSugestija.ItemsSource = null;
            }



            ApiParametri.VidljivostkomandeSave = true;

            if (txtIzlvrn.Text != "")
            {
                txtIzlvrn.BorderBrush = Brushes.Blue;
                txtIzlvrn.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txtIzlvrn.BorderBrush = Brushes.Red;
                txtIzlvrn.BorderThickness = new Thickness(1);
            }
        }
        #endregion

        #region SPECIJALIZOVANE IZLAZNE VREDNOSTI


        private void Specijalizovaneizlaznevr_MouseEnter(object sender, MouseEventArgs e)
        {
            OtkljucajSve_Executed(null, null);

            if (string.IsNullOrWhiteSpace(txtnazspizlvr.Text))
            {
                txtnazspizlvr.BorderBrush = Brushes.Red;
                txtnazspizlvr.BorderThickness = new Thickness(1);
            }

            if (string.IsNullOrWhiteSpace(new TextRange(rbspizlvr.Document.ContentStart, rbspizlvr.Document.ContentEnd).Text))
            {

                rbspizlvr.BorderBrush = Brushes.Red;
                rbspizlvr.BorderThickness = new Thickness(1);
            }
            if (string.IsNullOrWhiteSpace(txtDefizlazvr.Text))
            {
                txtDefizlazvr.BorderBrush = Brushes.Red;
                txtDefizlazvr.BorderThickness = new Thickness(1);
            }
        }




        private void txtnazspizlvr_TextChanged(object sender, TextChangedEventArgs e)
        {
            string tipstringa = txtnazspizlvr.Text;
            List<string> Autospecizlvr = new List<string>();
            Autospecizlvr.Clear();
            foreach (string asg in SPECIZLAZNEVR)
            {
                if (!string.IsNullOrEmpty(txtnazspizlvr.Text))
                {
                    if (asg.StartsWith(tipstringa))
                    {
                        Autospecizlvr.Add(asg);
                    }
                }
            }
            if (Autospecizlvr.Count > 0)
            {
                lbSpecIzlvrSugestija.ItemsSource = Autospecizlvr;
                lbSpecIzlvrSugestija.Visibility = Visibility.Visible;
            }
            else if (txtnazspizlvr.Text.Equals(" "))
            {
                lbSpecIzlvrSugestija.Visibility = Visibility.Collapsed;
                lbSpecIzlvrSugestija.ItemsSource = null;
            }
            else
            {
                lbSpecIzlvrSugestija.Visibility = Visibility.Collapsed;
                lbSpecIzlvrSugestija.ItemsSource = null;
            }

            //*-****************************************************************

            ApiParametri.VidljivostkomandeSave = true;

            if (txtnazspizlvr.Text != "")
            {
                txtnazspizlvr.BorderBrush = Brushes.Blue;
                txtnazspizlvr.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txtnazspizlvr.BorderBrush = Brushes.Red;
                txtnazspizlvr.BorderThickness = new Thickness(1);

            }
        }

        private void rbspizlvr_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (string.IsNullOrWhiteSpace(new TextRange(rbspizlvr.Document.ContentStart, rbspizlvr.Document.ContentEnd).Text))
            {

                rbspizlvr.BorderBrush = Brushes.Red;
                rbspizlvr.BorderThickness = new Thickness(1);
            }
            else
            {
                rbspizlvr.BorderBrush = Brushes.Blue;
                rbspizlvr.BorderThickness = new Thickness(0.5);
            }
        }


        #endregion

        #region DODATAK
        private void dodaci_MouseEnter(object sender, MouseEventArgs e)
        {
            OtkljucajSve_Executed(null, null);

            if (string.IsNullOrWhiteSpace(txtNazivDodatka.Text))
            {
                txtNazivDodatka.BorderBrush = Brushes.Red;
            }

            if (string.IsNullOrWhiteSpace(txtTipDodatka.Text))
            {
                txtTipDodatka.BorderBrush = Brushes.Red;
            }

            if (string.IsNullOrWhiteSpace(new TextRange(rbOpisdodatka.Document.ContentStart, rbOpisdodatka.Document.ContentEnd).Text))
            {

                rbOpisdodatka.BorderBrush = Brushes.Red;
            }
        }

        private void txtNazivDodatka_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;



            if (txtNazivDodatka.Text != "")
            {
                txtNazivDodatka.BorderBrush = Brushes.Blue;
            }
            else
            {
                txtNazivDodatka.BorderBrush = Brushes.Red;
            }
        }

        private void txtTipDodatka_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;



            if (txtTipDodatka.Text != "")
            {



                txtTipDodatka.BorderBrush = Brushes.Blue;
            }
            else
            {
                txtTipDodatka.BorderBrush = Brushes.Red;
            }
        }

        private void rbOpisdodatka_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (string.IsNullOrWhiteSpace(new TextRange(rbOpisdodatka.Document.ContentStart, rbOpisdodatka.Document.ContentEnd).Text))
            {

                rbOpisdodatka.BorderBrush = Brushes.Red;
            }
            else
            {
                rbOpisdodatka.BorderBrush = Brushes.Blue;
            }
        }


        #endregion


        #region TABELARNE IZLAZNE VREDNOTI
        private void Tabelarneizlaznevr_MouseEnter(object sender, MouseEventArgs e)
        {
            OtkljucajSve_Executed(null, null);

            if (string.IsNullOrWhiteSpace(txtNazivtabizlazniparam.Text))
            {
                txtNazivtabizlazniparam.BorderBrush = Brushes.Red;
                txtNazivtabizlazniparam.BorderThickness = new Thickness(1);
            }

            if (string.IsNullOrWhiteSpace(new TextRange(rbOpistabizlazniparam.Document.ContentStart, rbOpistabizlazniparam.Document.ContentEnd).Text))
            {

                rbOpistabizlazniparam.BorderBrush = Brushes.Red;
                rbOpistabizlazniparam.BorderThickness = new Thickness(1);
            }

            if (string.IsNullOrWhiteSpace(txtPoletabizlazniparam.Text))
            {
                txtPoletabizlazniparam.BorderBrush = Brushes.Red;
                txtPoletabizlazniparam.BorderThickness = new Thickness(1);
            }
            if (string.IsNullOrWhiteSpace(txttipizlazniparam.Text))
            {
                txttipizlazniparam.BorderBrush = Brushes.Red;
                txttipizlazniparam.BorderThickness = new Thickness(1);
            }
            if (string.IsNullOrWhiteSpace(txtduztabizlazniparam.Text))
            {
                txtduztabizlazniparam.BorderBrush = Brushes.Red;
                txtduztabizlazniparam.BorderThickness = new Thickness(1);
            }
            if (string.IsNullOrWhiteSpace(txttzlaznuInfosys.Text))
            {
                txttzlaznuInfosys.BorderBrush = Brushes.Red;
                txttzlaznuInfosys.BorderThickness = new Thickness(1);

            }
        }
        private void rbOpistabizlazniparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (string.IsNullOrWhiteSpace(new TextRange(rbOpistabizlazniparam.Document.ContentStart, rbOpistabizlazniparam.Document.ContentEnd).Text))
            {

                rbOpistabizlazniparam.BorderBrush = Brushes.Red;
                rbOpistabizlazniparam.BorderThickness = new Thickness(1);

            }
            else
            {
                rbOpistabizlazniparam.BorderBrush = Brushes.Blue;
                rbOpistabizlazniparam.BorderThickness = new Thickness(0.5);
            }
        }



        private void txtPoletabizlazniparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            string tipstringaulp = txtPoletabizlazniparam.Text;
            List<string> lAutotabizlparam = new List<string>();
            lAutotabizlparam.Clear();

            foreach (string tabizlvr in TABIZLVR)
            {
                if (!string.IsNullOrEmpty(txtPoletabizlazniparam.Text))
                {
                    if (tabizlvr.StartsWith(tipstringaulp))
                    {
                        lAutotabizlparam.Add(tabizlvr);
                    }
                }
            }
            if (lAutotabizlparam.Count > 0)
            {
                lbTabizlaznevrSugestija.ItemsSource = lAutotabizlparam;
                lbTabizlaznevrSugestija.Visibility = Visibility.Visible;
            }
            else if (txtPoletabizlazniparam.Text.Equals(" "))
            {
                lbTabizlaznevrSugestija.Visibility = Visibility.Collapsed;
                lbTabizlaznevrSugestija.ItemsSource = null;
            }
            else
            {
                lbTabizlaznevrSugestija.Visibility = Visibility.Collapsed;
                lbTabizlaznevrSugestija.ItemsSource = null;
            }



            if (txtPoletabizlazniparam.Text != "")
            {
                txtPoletabizlazniparam.BorderBrush = Brushes.Blue;
                txtPoletabizlazniparam.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txtPoletabizlazniparam.BorderBrush = Brushes.Red;
                txtPoletabizlazniparam.BorderThickness = new Thickness(1);
            }
        }



        private void txtNazivtabizlazniparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (txtNazivtabizlazniparam.Text != "")
            {
                lbltnaziv.Content = txtNazivtabulparam.Text;
                lbltnaziv.Visibility = Visibility.Hidden;
            }
            if (txtNazivtabizlazniparam.Text != "")
            {
                txtNazivtabizlazniparam.BorderBrush = Brushes.Blue;
                txtNazivtabizlazniparam.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txtNazivtabizlazniparam.BorderBrush = Brushes.Red;
                txtNazivtabizlazniparam.BorderThickness = new Thickness(1);
            }
        }

        private void txtNazivtabizlazniparam_LostFocus(object sender, RoutedEventArgs e)
        {
            txtNazivtabizlazniparam.IsEnabled = false;

            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }



        private void txttipizlazniparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (txttipizlazniparam.Text != "")
            {
                txttipizlazniparam.BorderBrush = Brushes.Blue;
                txttipizlazniparam.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txttipizlazniparam.BorderBrush = Brushes.Red;
                txttipizlazniparam.BorderThickness = new Thickness(1);
            }
        }
        #endregion

        #endregion

        #region TABELARNE IZLAZNE VREDNOSTI
        private void Otkljucanotabizlaznip_Click(object sender, RoutedEventArgs e)
        {
            if (Otkljucanotabizlaznip.IsChecked == true)
            {
                CIzvestaj.listaTabizlparamizvestaj.Clear();

                foreach (CTabIzlazniParametri item in listaTabizlparam)
                {
                    CIzvestaj.listaTabizlparamizvestaj.Add(item);
                }
                txtNazivtabizlazniparam.IsEnabled = true;
                txtPoletabizlazniparam.IsEnabled = true;
                txttipizlazniparam.IsEnabled = true;
                txtduztabizlazniparam.IsEnabled = true;
                txttzlaznuInfosys.IsEnabled = true;
                rbOpistabizlazniparam.IsEnabled = true;
                dgTabizlazniparam.IsEnabled = true;
                btnDodajtabizlazniparam.IsEnabled = true;
                btnobrisitabizlaznip.IsEnabled = true;
            }
        }

        private void Zakljucajtabizlaznip_Click(object sender, RoutedEventArgs e)
        {
            if (Zakljucajtabizlaznip.IsChecked == true)
            {
                CIzvestaj.listaTabizlparamizvestaj.Clear();

                foreach (CTabIzlazniParametri item in listaTabizlparam)
                {
                    CIzvestaj.listaTabizlparamizvestaj.Add(item);
                }

                txtNazivtabizlazniparam.IsEnabled = false;
                txtPoletabizlazniparam.IsEnabled = false;
                txttipizlazniparam.IsEnabled = false;
                txtduztabizlazniparam.IsEnabled = false;
                txttzlaznuInfosys.IsEnabled = false;
                rbOpistabizlazniparam.IsEnabled = false;
                btnDodajtabizlazniparam.IsEnabled = false;
                btnobrisitabizlaznip.IsEnabled = false;
                dgTabizlazniparam.IsEnabled = false;

            }
        }

        private void brNovatabelaizlazni_Click(object sender, RoutedEventArgs e)
        {
            if (brNovatabelaizlazni.IsChecked == true)
            {
                txtNazivtabizlazniparam.IsEnabled = true;
                txtNazivtabizlazniparam.Clear();
            }
        }

        private void txttzlaznuInfosys_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (txttzlaznuInfosys.Text != "")
            {
                txttzlaznuInfosys.BorderBrush = Brushes.Blue;
                txttzlaznuInfosys.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txttzlaznuInfosys.BorderBrush = Brushes.Red;
                txttzlaznuInfosys.BorderThickness = new Thickness(1);
            }
        }


        private void txtduztabizlazniparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (txtduztabizlazniparam.Text != "")
            {
                txtduztabizlazniparam.BorderBrush = Brushes.Blue;
                txtduztabizlazniparam.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txtduztabizlazniparam.BorderBrush = Brushes.Red;
                txtduztabizlazniparam.BorderThickness = new Thickness(1);


            }
        }


        private void btnobrisitabizlaznip_Click(object sender, RoutedEventArgs e)
        {

            int index = dgTabizlazniparam.SelectedIndex;

            DeleteULP(index);
            txtNazivtabizlazniparam.Clear();
            txtPoletabizlazniparam.Clear();
            txttipizlazniparam.Clear();
            txtduztabizlazniparam.Clear();
            txttzlaznuInfosys.Clear();
            Listaspizlaznihvr.Clear();
            rbOpistabizlazniparam.Document.Blocks.Clear();
            dgTabizlazniparam.ItemsSource = listaTabizlparam;
        }

        private void btnDodajtabizlazniparam_Click(object sender, RoutedEventArgs e)
        {
            CTabIzlazniParametri tabizlazniparam = new CTabIzlazniParametri();
            tabizlazniparam.Nazivtabelarniizlazniparam = txtNazivtabizlazniparam.Text;
            tabizlazniparam.Poljetabelarniizlazniparam = txtPoletabizlazniparam.Text;
            tabizlazniparam.Tipizlazniparam = txttipizlazniparam.Text;


            tabizlazniparam.Duzinapoljaizlazniparam = txtduztabizlazniparam.Text;
            tabizlazniparam.Opistabelarniizlazniparam = new TextRange(rbOpistabizlazniparam.Document.ContentStart, rbOpistabizlazniparam.Document.ContentEnd).Text;
            tabizlazniparam.Infosysizlazniparam = txttzlaznuInfosys.Text;
            listaTabizlparam.Add(tabizlazniparam);

            dgTabizlazniparam.ItemsSource = listaTabizlparam;
            List<string> tr = new List<string>();
            foreach (var item in listaTabizlparam)
            {
                tr.Add(item.Poljetabelarniizlazniparam + ";" + item.Tipizlazniparam + ";" + item.Duzinapoljaizlazniparam + ";" + item.Infosysizlazniparam + ";" + item.Opistabelarniizlazniparam + ";");
            }

            File.WriteAllText("TrenutnoTabelarniIzlazniParametri.txt", String.Empty);
            File.WriteAllLines("TrenutnoTabelarniIzlazniParametri.txt", tr);

            txttipizlazniparam.Clear();
            txtPoletabizlazniparam.Clear();
            txtduztabizlazniparam.Clear();
            rbOpistabizlazniparam.Document.Blocks.Clear();
            txttzlaznuInfosys.Clear();
            txtPoletabizlazniparam.Focus();

            ApiParametri.SinhroniyacijaListi = true;
            if (ApiParametri.SinhroniyacijaListi == true)
            {
                Sinhronizacija_Executed(null, null);
            }
        }

        private void dgTabizlazniparam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string naziv = (dgTabizlazniparam.SelectedItem as CTabIzlazniParametri).Nazivtabelarniizlazniparam.ToString();
                txtNazivtabizlazniparam.Text = naziv;

                txtPoletabizlazniparam.Text = (dgTabizlazniparam.SelectedItem as CTabIzlazniParametri).Poljetabelarniizlazniparam.ToString();
                txttipizlazniparam.Text = (dgTabizlazniparam.SelectedItem as CTabIzlazniParametri).Tipizlazniparam.ToString();
                txtduztabizlazniparam.Text = (dgTabizlazniparam.SelectedItem as CTabIzlazniParametri).Duzinapoljaizlazniparam.ToString();
                txttzlaznuInfosys.Text = (dgTabizlazniparam.SelectedItem as CTabIzlazniParametri).Infosysizlazniparam.ToString();
                rbOpistabizlazniparam.Document.Blocks.Clear();
                rbOpistabizlazniparam.AppendText((dgTabizlazniparam.SelectedItem as CTabIzlazniParametri).Opistabelarniizlazniparam);
            }
            catch (Exception)
            {

                return;
            }
        }

        private void txtDefizlazvr_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (txtDefizlazvr.Text != "")
            {
                txtDefizlazvr.BorderBrush = Brushes.Blue;
                txtDefizlazvr.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txtDefizlazvr.BorderBrush = Brushes.Red;
                txtDefizlazvr.BorderThickness = new Thickness(1);
            }
        }

        private void txtobaveznospizlazvr_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (txtobaveznospizlazvr.Text == "Da")
            {
                txtobaveznospizlazvr.BorderBrush = Brushes.Blue;
                txtobaveznospizlazvr.BorderThickness = new Thickness(0.5);
                cbspizlazDa.IsChecked = true;
                cbspizlazne.IsChecked = false;
            }
            else
            {
                txtobaveznospizlazvr.BorderBrush = Brushes.Red;
                txtobaveznospizlazvr.BorderThickness = new Thickness(1);
                cbspizlazDa.IsChecked = false;
                cbspizlazne.IsChecked = true;
            }
        }

        private void cbspizlazDa_Click(object sender, RoutedEventArgs e)
        {
            if (cbspizlazDa.IsChecked == true)
            {
                txtobaveznospizlazvr.Text = "Da";
                cbspizlazDa.BorderBrush = Brushes.Blue;
            }
        }

        private void cbspizlazne_Click(object sender, RoutedEventArgs e)
        {
            if (cbspizlazne.IsChecked == true)
            {
                txtobaveznospizlazvr.Text = "Ne";
                cbspizlazne.BorderBrush = Brushes.Blue;
            }
        }
        #endregion

        #region PODESAVANJE

        private void Podesvanje_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Podesvanje_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            frmPodesavanja frmpod = new frmPodesavanja();
            CIniFile Ini1 = new CIniFile(ApiParametri.PutanjadoInifajla);
            string putanjatrenutnogfajla = Ini1.Read("PutanjaTrenutnoPokrenutogFajla", "ApiDocument");




            frmpod.Show();
        }
        #endregion

        #region DUGMAD NA TOOLBAR KONTROLAMA
        private void zavrseno_Click(object sender, RoutedEventArgs e)
        {
            ApiParametri.StanjedugmetaObavljeno = false;


        }

        private void izvestaj_Click(object sender, RoutedEventArgs e)
        {
            ApiParametri.StanjedugmetaObavljeno = true;

        }

        #endregion

        #region Chackbixne
        private void Ne_Click_1(object sender, RoutedEventArgs e)
        {
            if (Ne.IsChecked == true)
            {
                txtobavulparam.Text = "Ne";
                Ne.BorderBrush = Brushes.Blue;
                Ne.BorderThickness = new Thickness(0.5);

            }
        }

        #endregion

        #region KOMANDA OBJAVI
        private void Objavljeno_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Objavljeno_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Zaljucajsve_Executed(null, null);
            API.NazivApiFunkcije = txtnazivfunkcija.Text;
            API.Modul = txtModul.Text;
            API.Opisfunkcije = new TextRange(rbOpisfunkcije.Document.ContentStart, rbOpisfunkcije.Document.ContentEnd).Text;
            API.Opislogikefunkcije = new TextRange(rbopisLogikefunkcije.Document.ContentStart, rbopisLogikefunkcije.Document.ContentEnd).Text;
            API.Ogrucestalostifunkcija = new TextRange(rbOgrucestfunkc.Document.ContentStart, rbOgrucestfunkc.Document.ContentEnd).Text;
            API.Scenario = new TextRange(rbScenario.Document.ContentStart, rbScenario.Document.ContentEnd).Text;
            CIzvestaj.l1.Add(API);

            if (ApiParametri.StanjedugmetaObavljeno == true)
            {
                Window1 w1 = new Window1();
                w1.btnPDF.Visibility = Visibility.Hidden;
                w1.VremekreiranjaPDF();


            }
            else
            {
                Window1 w1 = new Window1();
                w1.btnPDF.Visibility = Visibility.Visible;
                w1.VremekreiranjaPDF();


            }

        }
        #endregion

        #region Explorer
        public string PutanjaExplorera { get; set; }
        public string Putanjanajcescekoriscenih { get; set; }


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


            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());

            }

        }


        private void folders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.Source is TreeViewItem && ((TreeViewItem)e.Source).IsSelected)
                {

                }
                this.New_Executed(null, null);
                this.Load_Executed(null, null);
                StatusKesiranja();
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

                txtpath.Text = PutanjaExplorera.ToString();
                ApiParametri.PutanjaTrenutnoPokrenutogFajla = PutanjaExplorera.ToString();
            }
            catch (Exception)
            {

                return;
            }
        }

        private void Favorites_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.Load_Executed(null, null);
            }
            catch (Exception)
            {

                return;
            }
        }

        private void Favorites_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                string pathfavorite = Favorites.SelectedItem.ToString();
                ApiParametri.PutanjaTrenutnoPokrenutogFajla = pathfavorite;
                MessageBox.Show(pathfavorite);
                Process.Start(pathfavorite);
            }
            catch (Exception)
            {

                return;
            }
        }

        private void btnpath_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Favorites.Items.Add(txtpath.Text);
                CIniFile MojIni = new CIniFile(ApiParametri.PutanjadoInifajla.ToString());
                MojIni.KeyExists("PoslednjeSacuvanaPutanjaustabli", "ApiDocument");
                MojIni.DeleteKey("PoslednjeSacuvanaPutanjaustabli", "ApiDocument");
                MojIni.Write("PoslednjeSacuvanaPutanjaustabli", txtpath.Text, "ApiDocument");
            }
            catch (Exception)
            {

                return;
            }


        }








        #endregion

        #region  SKLONI FORME

        #region ULAZNI PARAMETRI

        public void Skloniformuulaznihparametara()
        {
            lbAutoulazniparametri.Visibility = Visibility.Hidden;
            label7.Visibility = Visibility.Hidden;

            label8.Visibility = Visibility.Hidden;

            label9.Visibility = Visibility.Hidden;
            label10.Visibility = Visibility.Hidden;
            btnDodajparameter.Visibility = Visibility.Hidden;
            txtNazivulparam.Visibility = Visibility.Hidden;
            rbOpisulp.Visibility = Visibility.Hidden;
            Da.Visibility = Visibility.Hidden;
            Ne.Visibility = Visibility.Hidden;
            txtdefulparam.Visibility = Visibility.Hidden;
            btnUkloniulparam.Visibility = Visibility.Hidden;

            Zakljucaj.Visibility = Visibility.Hidden;
            Otkljucano.Visibility = Visibility.Hidden;
            txtobavulparam.Visibility = Visibility.Hidden;
            label7_Copy.Visibility = Visibility.Hidden;
            label7_Copy1.Visibility = Visibility.Hidden;
            dgulparam.Margin = new Thickness(5, 15, 0, 10);
            dgulparam.Height = 680;

        }
        #endregion

        #region SPECIJALIZOVANI ULAZNI PARAMETRI

        public void Skloniformuspulparam()
        {
            labe20.Visibility = Visibility.Hidden;

            labe21.Visibility = Visibility.Hidden;

            label22.Visibility = Visibility.Hidden;

            label23.Visibility = Visibility.Hidden;

            btnDodajSpulparam.Visibility = Visibility.Hidden;
            txtObavezSpeculparam.Visibility = Visibility.Hidden;
            rbOpisSpeculparam.Visibility = Visibility.Hidden;
            cbspulpda.Visibility = Visibility.Hidden;
            cbspulpne.Visibility = Visibility.Hidden;
            txtNazivspeculparam.Visibility = Visibility.Hidden;
            btnUkloniSpulparam.Visibility = Visibility.Hidden;

            Zakljucajspulp.Visibility = Visibility.Hidden;
            Otkljucanospulp.Visibility = Visibility.Hidden;
            txtObavezSpeculparam.Visibility = Visibility.Hidden;

            dgSpeculparam.Margin = new Thickness(10, 20, 0, 5);
            dgSpeculparam.Height = 660;

        }
        #endregion

        #region TABELARNI ULAZNI PARAMETRI
        public void SkloniformuTabelarniulazniparametri()
        {



            label11.Visibility = Visibility.Hidden;

            label12.Visibility = Visibility.Hidden;

            label13a.Visibility = Visibility.Hidden;
            label14a.Visibility = Visibility.Hidden;
            label14.Visibility = Visibility.Hidden;
            label13.Visibility = Visibility.Hidden;

            btnDodajtabulparam.Visibility = Visibility.Hidden;
            txtNazivtabulparam.Visibility = Visibility.Hidden;
            txtPoletabulparam.Visibility = Visibility.Hidden;
            txttipolparam.Visibility = Visibility.Hidden;
            txtduztabulparam.Visibility = Visibility.Hidden;
            txtInfosys.Visibility = Visibility.Hidden;
            rbOpistabulparam.Visibility = Visibility.Hidden;

            btnobrisitabulp.Visibility = Visibility.Hidden;

            Zakljucajtabulp.Visibility = Visibility.Hidden;
            Otkljucanotabulp.Visibility = Visibility.Hidden;
            brNovatabela.Visibility = Visibility.Hidden;

            dgTabulparam.Margin = new Thickness(10, 20, 0, 5);
            dgTabulparam.Height = 670;

        }

        #endregion

        #region STATUSI GRESKI
        public void ZatvorifirmuStatusigreski()
        {
            label15.Visibility = Visibility.Hidden;
            label16.Visibility = Visibility.Hidden;
            label17.Visibility = Visibility.Hidden;
            txtIdentifgreske.Visibility = Visibility.Hidden;
            txtTiogreske.Visibility = Visibility.Hidden;
            rbopisgreske.Visibility = Visibility.Hidden;
            btnDodajstgreske.Visibility = Visibility.Hidden;
            obrisisatgr.Visibility = Visibility.Hidden;
            dgstatusgreske.Margin = new Thickness(10, 20, 0, 5);
            dgstatusgreske.Height = 650;

        }

        #endregion

        #region SPECIJALIZOVANE IZLAZNE VREDNOSTI

        public void ZatvoriformuSpecijalizovaneIzlaznevrednosti()
        {
            label20.Visibility = Visibility.Hidden;
            label3.Visibility = Visibility.Hidden;
            label21.Visibility = Visibility.Hidden;
            labeldef.Visibility = Visibility.Hidden;
            txtnazspizlvr.Visibility = Visibility.Hidden;
            txtDefizlazvr.Visibility = Visibility.Hidden;
            rbspizlvr.Visibility = Visibility.Hidden;
            cbspizlazDa.Visibility = Visibility.Hidden;
            cbspizlazne.Visibility = Visibility.Hidden;
            btndodajspulp.Visibility = Visibility.Hidden;
            btnobrisispulp.Visibility = Visibility.Hidden;
            txtobaveznospizlazvr.Visibility = Visibility.Hidden;
            Zakljucajspizlazvr.Visibility = Visibility.Hidden;
            Otkljucanjspizlvr.Visibility = Visibility.Hidden;
            dgspizlazvre.Margin = new Thickness(10, -50, 0, 5);
            dgspizlazvre.Height = 620;
        }


        #endregion

        #region TABELARNI IZLAZNI PARAMETRI

        public void Zatvoriformutabelarniizlazniparametri()
        {
            label111.Visibility = Visibility.Hidden;

            label121.Visibility = Visibility.Hidden;

            label13a1.Visibility = Visibility.Hidden;
            label14a1.Visibility = Visibility.Hidden;
            label141.Visibility = Visibility.Hidden;
            label131.Visibility = Visibility.Hidden;

            btnDodajtabizlazniparam.Visibility = Visibility.Hidden;
            txtNazivtabizlazniparam.Visibility = Visibility.Hidden;
            txtPoletabizlazniparam.Visibility = Visibility.Hidden;
            txttipizlazniparam.Visibility = Visibility.Hidden;
            txtduztabizlazniparam.Visibility = Visibility.Hidden;
            txttzlaznuInfosys.Visibility = Visibility.Hidden;
            rbOpistabizlazniparam.Visibility = Visibility.Hidden;

            btnobrisitabizlaznip.Visibility = Visibility.Hidden;

            Zakljucajtabizlaznip.Visibility = Visibility.Hidden;
            Otkljucanotabizlaznip.Visibility = Visibility.Hidden;
            brNovatabelaizlazni.Visibility = Visibility.Hidden;


            dgTabizlazniparam.Margin = new Thickness(10, 20, 0, 0);
            dgTabizlazniparam.Height = 660;
        }

        #endregion

        #region DODATAK
        public void Zatvoriformudodatak()
        {
            labedod.Visibility = Visibility.Hidden;
            Tipdodatka.Visibility = Visibility.Hidden;
            Opisdodatka.Visibility = Visibility.Hidden;
            txtNazivDodatka.Visibility = Visibility.Hidden;
            txtTipDodatka.Visibility = Visibility.Hidden;
            rbOpisdodatka.Visibility = Visibility.Hidden;
            btnDodajdodatak.Visibility = Visibility.Hidden;
            btnobrisidodatak.Visibility = Visibility.Hidden;
            rbDodatakNapomena.Visibility = Visibility.Hidden;
            cbTipdodatak.Visibility = Visibility.Hidden;
            dgDodatak.Margin = new Thickness(10, 20, 0, 0);
            dgDodatak.Height = 690;
        }

        #endregion

        #region IZLAZNE VREDNOSTI
        public void Zatvoriformuizlaznevrednosti()
        {
            label18.Visibility = Visibility.Hidden;
            label19.Visibility = Visibility.Hidden;
            txtIzlvrn.Visibility = Visibility.Hidden;
            rbopisizlvr.Visibility = Visibility.Hidden;
            btnDodajizlvr.Visibility = Visibility.Hidden;
            btnUkloniizlvr.Visibility = Visibility.Hidden;
            dgIzlaznevr.Margin = new Thickness(10, 20, 0, 5);
            dgIzlaznevr.Height = 670;

        }


        #endregion

        public void Zatvoriformusaradnika()
        {
            labels.Visibility = Visibility.Hidden;

            txtIme.Visibility = Visibility.Hidden;

            btnDodajsaradnika.Visibility = Visibility.Hidden;
            btnobrisiSaradnika.Visibility = Visibility.Hidden;
            dgSarad.Margin = new Thickness(10, 10, 0, 0);
            dgSarad.Height = 420;
        }
        #endregion

        #region PRIKAZI FORME

        #region ULAZNI PARAMETRI
        public void Prikaziformuulaznihparametara()
        {
            label7.Visibility = Visibility.Visible;

            label8.Visibility = Visibility.Visible;

            label9.Visibility = Visibility.Visible;
            label10.Visibility = Visibility.Visible;
            btnDodajparameter.Visibility = Visibility.Visible;
            txtNazivulparam.Visibility = Visibility.Visible;
            rbOpisulp.Visibility = Visibility.Visible;
            Da.Visibility = Visibility.Visible;
            Ne.Visibility = Visibility.Visible;
            txtdefulparam.Visibility = Visibility.Visible;
            btnUkloniulparam.Visibility = Visibility.Visible;

            Zakljucaj.Visibility = Visibility.Visible;
            Otkljucano.Visibility = Visibility.Visible;
            txtobavulparam.Visibility = Visibility.Visible;
            label7_Copy.Visibility = Visibility.Visible;
            label7_Copy1.Visibility = Visibility.Visible;
            dgulparam.Margin = new Thickness(5, 370, 0, 0);
            dgulparam.Height = 350;



        }
        #endregion

        #region SPECIJALIZOVANI ULAZNI PAARAMETRI

        public void Prikazifornuspecijalizovaniulparameri()
        {
            labe20.Visibility = Visibility.Visible;

            labe21.Visibility = Visibility.Visible;

            label22.Visibility = Visibility.Visible;

            label23.Visibility = Visibility.Visible;

            btnDodajSpulparam.Visibility = Visibility.Visible;
            txtObavezSpeculparam.Visibility = Visibility.Visible;
            rbOpisSpeculparam.Visibility = Visibility.Visible;
            cbspulpda.Visibility = Visibility.Visible;
            cbspulpne.Visibility = Visibility.Visible;
            txtNazivspeculparam.Visibility = Visibility.Visible;
            btnUkloniSpulparam.Visibility = Visibility.Visible;

            Zakljucajspulp.Visibility = Visibility.Visible;
            Otkljucanospulp.Visibility = Visibility.Visible;
            txtObavezSpeculparam.Visibility = Visibility.Visible;

            dgSpeculparam.Margin = new Thickness(10, 370, 0, 5);
            dgSpeculparam.Height = 340;
        }

        #endregion

        #region TABELARNI ULAZNI PARAMETRI
        public void PrikaziformuTabelarnihulaznihparametara()
        {
            label11.Visibility = Visibility.Visible;

            label12.Visibility = Visibility.Visible;

            label13a.Visibility = Visibility.Visible;
            label14a.Visibility = Visibility.Visible;
            label14.Visibility = Visibility.Visible;
            label13.Visibility = Visibility.Visible;

            btnDodajtabulparam.Visibility = Visibility.Visible;
            txtNazivtabulparam.Visibility = Visibility.Visible;
            txtPoletabulparam.Visibility = Visibility.Visible;
            txttipolparam.Visibility = Visibility.Visible;
            txtduztabulparam.Visibility = Visibility.Visible;
            txtInfosys.Visibility = Visibility.Visible;
            rbOpistabulparam.Visibility = Visibility.Visible;

            btnobrisitabulp.Visibility = Visibility.Hidden;

            Zakljucajtabulp.Visibility = Visibility.Visible;
            Otkljucanotabulp.Visibility = Visibility.Visible;
            brNovatabela.Visibility = Visibility.Visible;
            btnobrisitabulp.Visibility = Visibility.Visible;

            dgTabulparam.Margin = new Thickness(10, 350, 0, 5);
            dgTabulparam.Height = 350;
        }

        #endregion

        #region PRIKAZI GRESKE
        public void PrikazifirmuStatusigreski()
        {
            label15.Visibility = Visibility.Visible;
            label16.Visibility = Visibility.Visible;
            label17.Visibility = Visibility.Visible;
            txtIdentifgreske.Visibility = Visibility.Visible;
            txtTiogreske.Visibility = Visibility.Visible;
            rbopisgreske.Visibility = Visibility.Visible;
            btnDodajstgreske.Visibility = Visibility.Visible;
            obrisisatgr.Visibility = Visibility.Visible;
            dgstatusgreske.Margin = new Thickness(0, 310, 0, 0);
            dgstatusgreske.Height = 350;

        }
        #endregion

        #region SPECIJALIZOVANI IZLAZNI PARAMETRI
        public void PrikaziformuSpecijalizovaniizlazniparametri()
        {

            label20.Visibility = Visibility.Visible;
            label3.Visibility = Visibility.Visible;
            label21.Visibility = Visibility.Visible;
            labeldef.Visibility = Visibility.Visible;
            txtnazspizlvr.Visibility = Visibility.Visible;
            txtDefizlazvr.Visibility = Visibility.Visible;
            rbspizlvr.Visibility = Visibility.Visible;
            cbspizlazDa.Visibility = Visibility.Visible;
            cbspizlazne.Visibility = Visibility.Visible;
            btndodajspulp.Visibility = Visibility.Visible;
            btnobrisispulp.Visibility = Visibility.Visible;
            txtobaveznospizlazvr.Visibility = Visibility.Visible;
            Zakljucajspizlazvr.Visibility = Visibility.Visible;
            Otkljucanjspizlvr.Visibility = Visibility.Visible;
            dgspizlazvre.Margin = new Thickness(10, 305, 0, 5);
            dgspizlazvre.Height = 350;
        }

        #endregion

        #region TABELARNI IZLAZNI PARAMETRI
        public void PrikaziformuTabelarniizlazniparametara()
        {
            label111.Visibility = Visibility.Visible;

            label121.Visibility = Visibility.Visible;

            label13a1.Visibility = Visibility.Visible;
            label14a1.Visibility = Visibility.Visible;
            label141.Visibility = Visibility.Visible;
            label131.Visibility = Visibility.Visible;

            btnDodajtabizlazniparam.Visibility = Visibility.Visible;
            txtNazivtabizlazniparam.Visibility = Visibility.Visible;
            txtPoletabizlazniparam.Visibility = Visibility.Visible;
            txttipizlazniparam.Visibility = Visibility.Visible;
            txtduztabizlazniparam.Visibility = Visibility.Visible;
            txttzlaznuInfosys.Visibility = Visibility.Visible;
            rbOpistabizlazniparam.Visibility = Visibility.Visible;

            btnobrisitabizlaznip.Visibility = Visibility.Visible;

            Zakljucajtabizlaznip.Visibility = Visibility.Visible;
            Otkljucanotabizlaznip.Visibility = Visibility.Visible;
            brNovatabelaizlazni.Visibility = Visibility.Visible;


            dgTabizlazniparam.Margin = new Thickness(10, 410, 0, 5);
            dgTabizlazniparam.Height = 400;
        }
        #endregion

        #region DODATAK
        public void Prikaziformudodatak()
        {
            labedod.Visibility = Visibility.Visible;
            Tipdodatka.Visibility = Visibility.Visible;
            Opisdodatka.Visibility = Visibility.Visible;
            txtNazivDodatka.Visibility = Visibility.Visible;
            txtTipDodatka.Visibility = Visibility.Visible;
            rbOpisdodatka.Visibility = Visibility.Visible;
            btnDodajdodatak.Visibility = Visibility.Visible;
            btnobrisidodatak.Visibility = Visibility.Visible;
            rbDodatakNapomena.Visibility = Visibility.Visible;
            cbTipdodatak.Visibility = Visibility.Visible;
            dgDodatak.Margin = new Thickness(10, 460, 0, 0);
            dgDodatak.Height = 380;
        }

        #endregion


        #region IZLAZNE VREDNOSTI
        public void Prikaziformuizlaznevrednosti()
        {
            label18.Visibility = Visibility.Visible;
            label19.Visibility = Visibility.Visible;
            txtIzlvrn.Visibility = Visibility.Visible;
            rbopisizlvr.Visibility = Visibility.Visible;
            btnDodajizlvr.Visibility = Visibility.Visible;
            btnUkloniizlvr.Visibility = Visibility.Visible;
            dgIzlaznevr.Margin = new Thickness(10, 280, 0, 0);
            dgIzlaznevr.Height = 420;

        }




        #endregion

        public void Prikaziformusaradnicii()
        {
            labels.Visibility = Visibility.Visible;

            txtIme.Visibility = Visibility.Visible;

            btnDodajsaradnika.Visibility = Visibility.Visible;
            btnobrisiSaradnika.Visibility = Visibility.Visible;
            dgSarad.Margin = new Thickness(10, 120, 0, 0);
            dgSarad.Height = 420;

        }


        #endregion

        #region PRIKAZI GRID
        public void PrikaziGridove()
        {
            Skloniformuulaznihparametara();
            Skloniformuspulparam();
            SkloniformuTabelarniulazniparametri();
            ZatvorifirmuStatusigreski();
            ZatvoriformuSpecijalizovaneIzlaznevrednosti();
            Zatvoriformutabelarniizlazniparametri();
            Zatvoriformudodatak();
            Zatvoriformuizlaznevrednosti();

        }
        #endregion

        #region Prikazi sve Chackbox
        private void cbprikazi_Click(object sender, RoutedEventArgs e)
        {
            if (cbprikazi.IsChecked.Value == true)
            {
                this.Prikaziiexp_Executed(null, null);
                cbprikazi.Content = "Skupi sve";
            }
            else
            {
                this.Skupiexp_Executed(null, null);

                cbprikazi.Content = "Prikazi sve";
            }
        }

        #endregion

        #region CELLEDITEDITING

        private void dgSpeculparam_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
        }

        private void dgulparam_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
        }

        private void dgTabulparam_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
        }

        private void dgstatusgreske_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
        }

        private void dgIzlaznevr_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
        }

        private void dgspizlazvre_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
        }

        private void dgTabizlazniparam_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
        }

        private void dgDodatak_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
        }

        #endregion

        #region KESIRANJE/STRANICENJE

        private void cbkesiranje_Click(object sender, RoutedEventArgs e)
        {

            if (cbkesiranje.IsChecked.Value == true)
            {
                API.Kesiranje = true;
                // kesiranje  specijalni ulazni parametri
                txtNazivspeculparam.Text = "paging_sort_exp";
                txtTipspeculparam.Text = "char";
                rbOpisSpeculparam.AppendText("Klijent može navesti izraz za sortiranje rezultata." +
                  "Primer: GRUPA,A|MESTO,A|NAME,AC." + "Format izraza je PILJE,SMER|POLJEMSMER|POLJE,SMER"
                  + "gde je POLJE- naziv polja, a SMER je smer sortiranja i to: A-rastući redosled, D - opadakići redosled" +
                  "AC - rastući redosled sa zanemarivanjem veličine slova, DC - opadajući redosled sa zanemarivanjem veličine slova.");
                txtDefspulparam.Text = "";
                txtObavezSpeculparam.Text = "";
                Dodajspulp_Click(null, null);

                //************************************************************

                txtNazivspeculparam.Text = "paging_selected_page";
                txtTipspeculparam.Text = "num";
                rbOpisSpeculparam.AppendText("Klijent može da koristi straničenje upita." +
                    "Ukoliko se očekuje da rezultat sadrži tabelu sa velikim brojem slogova koje" +
                    "ne može odjednom da prikaže ili je to nepraktično. On može da traži od API servera" +
                    "da izvrši funkciju ali da vrati samo određenu stranu tabelarnog rezultata." +
                    "Upotrebljava se zajedno sa poljem (paging_page_len) koje naznačava koliko " +
                    "slogova tabele čine jednu stranu");
                txtDefspulparam.Text = "";
                txtObavezSpeculparam.Text = "";
                Dodajspulp_Click(null, null);
                //*******************************************************************************
                txtNazivspeculparam.Text = "paging_page_len";
                txtTipspeculparam.Text = "num";
                rbOpisSpeculparam.AppendText("Upotrebljava se zajedno sa poljem (paging_selected_page) za straničenje rezultata.");
                txtDefspulparam.Text = "";
                txtObavezSpeculparam.Text = "";
                Dodajspulp_Click(null, null);

                //**************************************************

                //kesiranje specijalni izlazni parametri
                txtnazspizlvr.Text = "paging_page_len";
                rbspizlvr.AppendText("Dužina strane u slogovima.");
                txtDefizlazvr.Text = "";
                txtobaveznospizlazvr.Text = "";
                txtTipspecizlaznevr.Text = "num";
                btndodajspulp_Click(null, null);

                txtnazspizlvr.Text = "paging_rec_count";
                rbspizlvr.AppendText("Ukupan broj slogova u tabeli.");
                txtDefizlazvr.Text = "";
                txtobaveznospizlazvr.Text = "";
                txtTipspecizlaznevr.Text = "num";
                btndodajspulp_Click(null, null);

                txtnazspizlvr.Text = "paging_sort_exp";
                rbspizlvr.AppendText("Izraz koji je upotrebljen za sortiranje tabele");
                txtDefizlazvr.Text = "";
                txtobaveznospizlazvr.Text = "";
                txtTipspecizlaznevr.Text = "char";
                btndodajspulp_Click(null, null);

                txtnazspizlvr.Text = "paging_selected_page";
                rbspizlvr.AppendText("Ako je klijent zadao straničenje upita,vraća se redni broj stranice.");
                txtDefizlazvr.Text = "";
                txtobaveznospizlazvr.Text = "";
                txtTipspecizlaznevr.Text = "num";
                btndodajspulp_Click(null, null);


            }
            else
            {
                API.Kesiranje = false;
                txtnazspizlvr.Text = "";
                rbspizlvr.AppendText("");
                txtDefizlazvr.Text = "";
                txtobaveznospizlazvr.Text = "";
                txtTipspecizlaznevr.Text = "";
                //    btndodajspulp_Click(null, null);

                txtnazspizlvr.Text = "";
                rbspizlvr.AppendText("");
                txtDefizlazvr.Text = "";
                txtobaveznospizlazvr.Text = "";
                txtTipspecizlaznevr.Text = "";
                //  btndodajspulp_Click(null, null);

                txtnazspizlvr.Text = "";
                rbspizlvr.AppendText("");
                txtDefizlazvr.Text = "";
                txtobaveznospizlazvr.Text = "";
                txtTipspecizlaznevr.Text = "";
                //btndodajspulp_Click(null, null);

                txtnazspizlvr.Text = "";
                rbspizlvr.AppendText("");
                txtDefizlazvr.Text = "";
                txtobaveznospizlazvr.Text = "";
                txtTipspecizlaznevr.Text = "";
                //btndodajspulp_Click(null, null);
            }

        }











        #endregion

        #region PROVERA DA LI JE KESIRANJE VEC UKLJUCENO

        public void StatusKesiranja()
        {
            bool stanjekesiranja = ListaSpeculparam.Any(x => x.specNazivulazniparametri == "paging_sort_exp");

            if (stanjekesiranja == true)
            {
                cbkesiranje.IsChecked = true;

            }
            else
            {
                cbkesiranje.IsChecked = false;

            }

        }




        #endregion

        #region RAD SA INI FAJLOM
        private void IniFajl_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void IniFajl_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            frmInifile inif = new frmInifile();
            inif.Show();


        }
        #endregion

        #region LOST FOCUS ZA Forsirane parametre

        private void rbOgrucestfunkc_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtNazivulparam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtTipUlaznogparametra_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtDuzinaUlaznogparametra_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void rbOpisulp_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtdefulparam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtNazivspeculparam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtTipspeculparam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void rbOpisSpeculparam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtDefspulparam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtPoletabulparam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txttipolparam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtduztabulparam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtInfosys_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void rbOpistabulparam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtIdentifgreske_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtTiogreske_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void rbopisgreske_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtIzlvrn_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void rbopisizlvr_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtnazspizlvr_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtTipspecizlaznevr_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void rbspizlvr_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtDefizlazvr_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtPoletabizlazniparam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txttipizlazniparam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtduztabizlazniparam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txttzlaznuInfosys_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void rbOpistabizlazniparam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtNazivDodatka_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void rbOpisdodatka_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtPrezime_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }
        }

        private void txtnazivfunkcija_LostFocus(object sender, RoutedEventArgs e)
        {
            CIniFile ini = new CIniFile(ApiParametri.PutanjadoInifajla);
          string statusforsiranogpolja =  ini.Read("Forsiranapolja", "ApiDocument");

            if (statusforsiranogpolja == "True" && StatusForsirano == true)
            {
                ApiParametri.VidljivostSaveAs = false;
            }
            else
            {
                ApiParametri.VidljivostSaveAs = true;
            }

        }

        private void rbScenario_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }

            if (ApiParametri.ForsiraniParametri == "True")
            {
                if (lscenarioforsiran == true)
                {

                    Scenario.Header = "SCENARIO";

                    Scenario.Foreground = new SolidColorBrush(Colors.Red);
                    btnsaveas.IsEnabled = false;
                    btnsave.IsEnabled = false;
                    ApiParametri.Sacuvano = false;
                    ApiParametri.VidljivostSaveAs = false;

                    ApiParametri.VidljivostkomandeSave = false;
                    StanjeSaveAsKomande = false;
                }
                else
                {
                    Scenario.Header = "SCENARIO";
                    Scenario.Foreground = new SolidColorBrush(Colors.Black);
                    btnsaveas.IsEnabled = true;
                    btnsave.IsEnabled = true;
                    ApiParametri.Sacuvano = true;
                    ApiParametri.VidljivostSaveAs = true;
                    ApiParametri.VidljivostkomandeSave = true;
                    StanjeSaveAsKomande = true;
                }
            }
            else
            {
                lscenarioforsiran = false;
            }
        }
        private void txtModul_LostFocus(object sender, RoutedEventArgs e)
        {

            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }

            if (ApiParametri.ForsiraniParametri == "True")
            {
                if (lforsiraniparametarmodul == true)
                {

                    Opisfunkcije.Header = "OPIS FUNKCIJE";
                    Opisfunkcije.Foreground = new SolidColorBrush(Colors.Red);
                    btnsaveas.IsEnabled = false;
                    btnsave.IsEnabled = false;
                    ApiParametri.Sacuvano = false;
                    ApiParametri.VidljivostSaveAs = false;
                    lforsiraniparametarmodul = true;
                    txtModul.BorderBrush = Brushes.Blue;
                    txtModul.BorderThickness = new Thickness(1);
                }
                else
                {
                    Opisfunkcije.Header = "OPIS FUNKCIJE";
                    Opisfunkcije.Foreground = new SolidColorBrush(Colors.Black);
                    btnsaveas.IsEnabled = true;
                    btnsave.IsEnabled = true;
                    ApiParametri.Sacuvano = true;
                    ApiParametri.VidljivostSaveAs = true;
                    lforsiraniparametarmodul = false;
                    txtModul.BorderBrush = Brushes.Red;
                    txtModul.BorderThickness = new Thickness(1);
                }
            }
            else
            {
                lforsiraniparametarmodul = false;
                txtModul.BorderBrush = Brushes.Blue;
                txtModul.BorderThickness = new Thickness(0.5);
                label6.Content = "Modul:";
                label6.Foreground = new SolidColorBrush(Colors.Black);
                lforsiraniparametarmodul = false;
            }
            if (string.IsNullOrWhiteSpace(txtModul.Text))
            {
                txtModul.BorderBrush = Brushes.Red;
                txtModul.BorderThickness = new Thickness(0.5);
            }
            else
            {
                txtModul.BorderBrush = Brushes.Blue;
                txtModul.BorderThickness = new Thickness(0.5);
            }
        }

        private void rbOpisfunkcije_LostFocus(object sender, RoutedEventArgs e)
        {

            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }

            if (ApiParametri.ForsiraniParametri == "True")
            {
                if (lForsiraniparametarOpisfunkcije == true)
                {

                    Opisfunkcije.Header = "OPIS FUNKCIJE";
                    Opisfunkcije.Foreground = new SolidColorBrush(Colors.Red);
                    btnsaveas.IsEnabled = false;
                    btnsave.IsEnabled = false;
                    ApiParametri.Sacuvano = false;
                    ApiParametri.VidljivostSaveAs = false;


                }
                else
                {
                    Opisfunkcije.Header = "OPIS FUNKCIJE";
                    Opisfunkcije.Foreground = new SolidColorBrush(Colors.Black);
                    btnsaveas.IsEnabled = true;
                    btnsave.IsEnabled = true;
                    ApiParametri.Sacuvano = true;
                    ApiParametri.VidljivostSaveAs = true;
                }
            }
            else
            {
                lForsiraniparametarOpisfunkcije = false;
            }
        }
        private void rbopisLogikefunkcije_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }


            if (ApiParametri.ForsiraniParametri == "True")
            {
                if (lForsiraniparametriOpislogike == true)
                {

                    Opisfunkcije.Header = "OPIS FUNKCIJE";
                    Opisfunkcije.Foreground = new SolidColorBrush(Colors.Red);
                    btnsaveas.IsEnabled = false;
                    btnsave.IsEnabled = false;
                    ApiParametri.Sacuvano = false;
                    ApiParametri.VidljivostSaveAs = false;


                }
                else
                {
                    Opisfunkcije.Header = "OPIS FUNKCIJE";
                    Opisfunkcije.Foreground = new SolidColorBrush(Colors.Black);
                    btnsaveas.IsEnabled = true;
                    btnsave.IsEnabled = true;
                    ApiParametri.Sacuvano = true;
                    ApiParametri.VidljivostSaveAs = true;
                }
            }
            else
            {
                lForsiraniparametriOpislogike = false;
            }
        }

        #endregion


        #region AUTOLISTE SELCEDCHANGED


        //********************************************************************************************************************
        private void lbAutoulazniparametri_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lbAutoulazniparametri.ItemsSource != null)
            {

                lbAutoulazniparametri.Visibility = Visibility.Collapsed;
                txtNazivulparam.TextChanged -= txtNazivulparam_TextChanged;
                if (lbAutoulazniparametri.SelectedIndex != -1)
                {
                    string text = lbAutoulazniparametri.SelectedItem.ToString();
                    string[] words = text.Split(';');

                    txtNazivulparam.Text = words[0].ToString();
                    txtTipUlaznogparametra.Text = words[1].ToString();
                    txtDuzinaUlaznogparametra.Text = words[2].ToString();
                    rbOpisulp.Document.Blocks.Clear();
                    rbOpisulp.AppendText(words[3].ToString());


                    lbAutoulazniparametri.Visibility = Visibility.Collapsed;


                }

                txtNazivulparam.TextChanged += txtNazivulparam_TextChanged;

            }

        }

        private void lbSpeculparamsugestija_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbSpeculparamsugestija.ItemsSource != null)
            {
                lbSpeculparamsugestija.Visibility = Visibility.Collapsed;
                txtNazivspeculparam.TextChanged -= txtNazivspeculparam_TextChanged;
                if (lbSpeculparamsugestija.SelectedIndex != -1)
                {
                    string text = lbSpeculparamsugestija.SelectedItem.ToString();
                    string[] words = text.Split(';');

                    txtNazivspeculparam.Text = words[0].ToString();
                    txtTipspeculparam.Text = words[1].ToString();

                    rbOpisSpeculparam.Document.Blocks.Clear();
                    rbOpisSpeculparam.AppendText(words[2].ToString());


                    lbSpeculparamsugestija.Visibility = Visibility.Collapsed;
                }
                txtNazivspeculparam.TextChanged += txtNazivspeculparam_TextChanged;
            }
        }

        //tabelarni ulazni parametri
        private void lbTabelarniulparamSugestija_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbTabelarniulparamSugestija.ItemsSource != null)
            {
                lbTabelarniulparamSugestija.Visibility = Visibility.Collapsed;
                txtPoletabulparam.TextChanged -= txtPoletabulparam_TextChanged;
                if (lbTabelarniulparamSugestija.SelectedIndex != -1)
                {
                    string text = lbTabelarniulparamSugestija.SelectedItem.ToString();
                    string[] words = text.Split(';');
                    txtPoletabulparam.Text = words[0].ToString();
                    txttipolparam.Text = words[1].ToString();
                    txtduztabulparam.Text = words[2].ToString();
                    txtInfosys.Text = words[3].ToString();
                    rbOpistabulparam.Document.Blocks.Clear();
                    rbOpistabulparam.AppendText(words[4].ToString());
                    lbTabelarniulparamSugestija.Visibility = Visibility.Collapsed;
                }
                txtPoletabulparam.TextChanged += txtPoletabulparam_TextChanged;
            }
        }


        // izlazne vrednosti
        private void lbIzlazneSugestija_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbIzlazneSugestija.ItemsSource != null)
            {
                lbIzlazneSugestija.Visibility = Visibility.Collapsed;
                txtIzlvrn.TextChanged -= txtIzlvrn_TextChanged;
                if (lbIzlazneSugestija.SelectedIndex != -1)
                {
                    string text = lbIzlazneSugestija.SelectedItem.ToString();
                    string[] words = text.Split(';');
                    txtIzlvrn.Text = words[0].ToString();
                    rbopisizlvr.Document.Blocks.Clear();
                    rbopisizlvr.AppendText(words[1].ToString());

                    lbTabelarniulparamSugestija.Visibility = Visibility.Collapsed;
                }
                txtIzlvrn.TextChanged += txtIzlvrn_TextChanged;
            }
        }


        // specijalizovane izlazne vrednosti

        private void lbSpecIzlvrSugestija_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbSpecIzlvrSugestija.ItemsSource != null)
            {
                lbSpecIzlvrSugestija.Visibility = Visibility.Collapsed;
                txtnazspizlvr.TextChanged -= txtnazspizlvr_TextChanged;
                if (lbSpecIzlvrSugestija.SelectedIndex != -1)
                {
                    string text = lbSpecIzlvrSugestija.SelectedItem.ToString();
                    string[] words = text.Split(';');
                    txtnazspizlvr.Text = words[0].ToString();
                    txtTipspecizlaznevr.Text = words[1].ToString();
                    rbspizlvr.Document.Blocks.Clear();
                    rbspizlvr.AppendText(words[2].ToString());

                    lbSpecIzlvrSugestija.Visibility = Visibility.Collapsed;
                }
                txtnazspizlvr.TextChanged += txtnazspizlvr_TextChanged;
            }
        }



        private void lbTabizlaznevrSugestija_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbTabizlaznevrSugestija.ItemsSource != null)
            {
                lbTabizlaznevrSugestija.Visibility = Visibility.Collapsed;
                txtPoletabizlazniparam.TextChanged -= txtPoletabizlazniparam_TextChanged;
                if (lbTabizlaznevrSugestija.SelectedIndex != -1)
                {
                    string text = lbTabizlaznevrSugestija.SelectedItem.ToString();
                    string[] words = text.Split(';');
                    txtPoletabizlazniparam.Text = words[0].ToString();
                    txttipizlazniparam.Text = words[1].ToString();
                    txtduztabizlazniparam.Text = words[2].ToString();
                    txttzlaznuInfosys.Text = words[3].ToString();
                    rbOpistabizlazniparam.Document.Blocks.Clear();
                    rbOpistabizlazniparam.AppendText(words[4].ToString());

                    lbTabizlaznevrSugestija.Visibility = Visibility.Collapsed;
                }
                txtPoletabizlazniparam.TextChanged += txtPoletabizlazniparam_TextChanged;
            }
        }

        private void lbAutosugrstijaGresaka_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbAutosugrstijaGresaka.ItemsSource != null)
            {

                lbAutosugrstijaGresaka.Visibility = Visibility.Collapsed;
                txtIdentifgreske.TextChanged -= txtIdentifgreske_TextChanged;
                if (lbAutosugrstijaGresaka.SelectedIndex != -1)
                {
                    string text = lbAutosugrstijaGresaka.SelectedItem.ToString();
                    string[] words = text.Split(';');

                    txtIdentifgreske.Text = words[0].ToString();
                    txtTiogreske.Text = words[1].ToString();

                    rbopisgreske.Document.Blocks.Clear();
                    rbopisgreske.AppendText(words[2].ToString());


                    lbAutosugrstijaGresaka.Visibility = Visibility.Collapsed;


                }

                txtIdentifgreske.TextChanged += txtIdentifgreske_TextChanged;

            }
        }

        #endregion

        #region SARADNICI


        private void btnobrisiSaradnika_Click(object sender, RoutedEventArgs e)
        {

            int index = dgSarad.SelectedIndex;

            DeleteULP(index);
            dgSarad.ItemsSource = Listasaradnika;

            txtIme.Clear();

        }

        private void btnDodajsaradnika_Click(object sender, RoutedEventArgs e)
        {
            CSaradnici saradnici = new CSaradnici();
            saradnici.Sifrasaradnika = txtIme.Text;

            Listasaradnika.Add(saradnici);
            dgSarad.ItemsSource = Listasaradnika;
            txtIme.Clear();
            txtIme.Focus();
        }


        private void saradnici_MouseEnter(object sender, MouseEventArgs e)
        {

        }





        private void dgsaradnici_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtIme.Text = (dgSarad.SelectedItem as CSaradnici).Sifrasaradnika.ToString();

            }
            catch (Exception)
            {

                return;
            }
        }

        private void dgsaradnici_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;
        }



        public bool lForsiraniparametriimesaradnika = true;


        private void txtIme_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApiParametri.VidljivostkomandeSave = true;

            if (txtIme.Text != "")
            {
                txtIme.BorderBrush = Brushes.Blue;
                txtIme.BorderThickness = new Thickness(1);
                lForsiraniparametriimesaradnika = true;
                labels.Content = "Šifra:";
                labels.Foreground = new SolidColorBrush(Colors.Black);
            }

            else
            {
                txtIme.BorderBrush = Brushes.Red;
                txtIme.BorderThickness = new Thickness(1);
                lForsiraniparametriimesaradnika = false;
                labels.Content = "* Šifra:";
                labels.Foreground = new SolidColorBrush(Colors.Red);

            }
        }





        private void txtIme_LostFocus(object sender, RoutedEventArgs e)
        {

            if (ApiParametri.VidljivostkomandeSave == true)
            {
                TaskbarItemInfo.Overlay = (ImageSource)Resources["Sacuvajdokument"];

            }

            if (ApiParametri.ForsiraniParametri == "True")
            {
                if (lForsiraniparametriimesaradnika == true)
                {

                    saradnici.Header = "SARADNICI";
                    saradnici.Foreground = new SolidColorBrush(Colors.Red);
                    btnsaveas.IsEnabled = false;
                    btnsave.IsEnabled = false;
                    ApiParametri.Sacuvano = false;
                    ApiParametri.VidljivostSaveAs = false;


                }
                else
                {
                    saradnici.Header = "SARADNICI";
                    saradnici.Foreground = new SolidColorBrush(Colors.Black);
                    btnsaveas.IsEnabled = true;
                    btnsave.IsEnabled = true;
                    ApiParametri.Sacuvano = true;
                    ApiParametri.VidljivostSaveAs = true;
                }
            }
            else
            {
                lForsiraniparametriimesaradnika = false;
            }
        }






        private void cbTipdodatak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtTipDodatka.Text = cbTipdodatak.SelectedItem.ToString();
        }

        #endregion

        #region AUTO PDF
        private void cbautopdf2_Click(object sender, RoutedEventArgs e)
        {
            CIniFile Ini = new CIniFile(ApiParametri.PutanjadoInifajla);
            if (cbautopdf2.IsChecked == true)
            {
                Ini.Write("Autopdf", "True", "ApiDocument");
            }
            else
            {
                Ini.Write("Autopdf", "False", "ApiDocument");
            }

        }
        #endregion

        #region Arhiviranje
        private void Arhiviranje_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Arhiviranje_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {

                int brojac = ApiParametri.ArhivaCount++;
                CIniFile Ini = new CIniFile(ApiParametri.PutanjadoInifajla.ToString());

                string projectPath1 = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                string Arhiva = Ini.Read("Arhiva", "ApiDocument");
                string dirgdejeobjavljeno = Ini.Read("Direktorijumzaobjavljenefunkcije", "ApiDocument");
                string Drivezaarhivu = Ini.Read("DriveGdeArhivirimo", "ApiDocument");

                string projectPath = Drivezaarhivu.ToString();
                string folderName = System.IO.Path.Combine(projectPath, Arhiva.ToString());
                string Startpath = Path.Combine(projectPath1, dirgdejeobjavljeno);
                string zipPath = Path.Combine(folderName, dirgdejeobjavljeno + "_" + brojac + ".zip");
                string extenzija = Path.GetExtension(Path.Combine(folderName, dirgdejeobjavljeno + "_" + brojac + ".zip"));
                DirectoryInfo dorektorijum = new DirectoryInfo(folderName);


                brojac++;
                ZipFile.CreateFromDirectory(Startpath, zipPath, CompressionLevel.Fastest, true);
                MessageBox.Show("Vasi podaci su arhivirani" + " " + zipPath.ToString(), "oBAVESTENJE", MessageBoxButton.OK, MessageBoxImage.Asterisk);

            }
            catch (Exception)
            {

                return;
            }
        }
        #endregion

        #region DEARHIVIRANJE
        private void Dearhiviranje_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Dearhiviranje_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openArchivefile = new OpenFileDialog();
            openArchivefile.Filter = "ZipArchive (.zip)|*.zip| RarArchive (*.rar)|*.rar|All Files (*.*)|*.*";


            if (openArchivefile.ShowDialog() == true)
            {
                string putanjadoarhive = openArchivefile.FileName.ToString();
                MessageBoxResult upozorenjedearhiviranje = MessageBox.Show("Raspakivanjem arhive svi fajlovi koji se nalaze u direktorijumu bice zamenjeni.Da li ste zaista sigurni da zelite da raspakujete arhivu.", "UPOZORENJE", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (upozorenjedearhiviranje == MessageBoxResult.Yes)
                {
                    int brojac = ApiParametri.ArhivaCount++;
                    CIniFile Ini = new CIniFile(ApiParametri.PutanjadoInifajla.ToString());

                    string projectPath1 = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                    string Arhiva = Ini.Read("Arhiva", "ApiDocument");
                    string dirgdejeobjavljeno = Ini.Read("Direktorijumzaobjavljenefunkcije", "ApiDocument");
                    string Drivezaarhivu = Ini.Read("DriveGdeArhivirimo", "ApiDocument");

                    string projectPath = Drivezaarhivu.ToString();
                    string folderName = System.IO.Path.Combine(projectPath, Arhiva.ToString());
                    string Startpath = Path.Combine(projectPath1, dirgdejeobjavljeno);
                    System.IO.DirectoryInfo di = new DirectoryInfo(Startpath);

                    foreach (FileInfo file in di.GetFiles("*.*"))
                    {

                        if (file != null)
                        {
                            file.Delete();
                        }

                    }

                    ZipFile.ExtractToDirectory(putanjadoarhive, projectPath1);

                    MessageBox.Show("Vasi podaci su kopirani u direktorijum za objavljene funkcije", "OBAVESTENJE", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }
        #endregion

        #region PROVERA PARAMETARA
        public void ProveraIniParametarra()
        {
            CIniFile Ini = new CIniFile(ApiParametri.PutanjadoInifajla);
            if (Ini.KeyExists("Arhiva", "ApiDocument") == false)
            {
                Ini.Write("Arhiva", "ApiDocumentArhiva", "ApiDocument");
            }
            string Arhiva = Ini.Read("Arhiva", "ApiDocument");
            if (Ini.KeyExists("Direktorijumzaobjavljenefunkcije", "ApiDocument") == false)
            {
                Ini.Write("Direktorijumzaobjavljenefunkcije", "Objavljeno", "ApiDocument");
            }
            //***********************************************************************************
            string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string foldername = System.IO.Path.Combine(ProjectPath, "Konfiguracija");
            if (Ini.KeyExists("DirektorijumIniFajla", "ApiDocument") == false)
            {
                Ini.Write("DirektorijumIniFajla", foldername, "ApiDocument");
            }
            //****************************************************************************************

            if (!Ini.KeyExists("Autopdf", "ApiDocument"))
            {
                Ini.Write("Autopdf", "False", "ApiDocument");
            }

            //*******************************************************************************

            string Drivezaarhivu = Ini.Read("DriveGdeArhivirimo", "ApiDocument");
            if (Ini.KeyExists("DriveGdeArhivirimo", "ApiDocument") == false)
            {
                Ini.Write("DriveGdeArhivirimo", @"D:\", "ApiDocument");
            }
            //*********************************************************************************

            string forsiranapolja = Ini.Read("Forsiranapolja", "ApiDocument");
            if (Ini.KeyExists("Forsiranapolja", "ApiDocument") == false)
            {
                Ini.Write("Forsiranapolja", "False", "ApiDocument");
            }
            //*********************************************************************************
            if (Ini.KeyExists("VelicinaFonta", "ApiDocument") == false)
            {
                Ini.Write("VelicinaFonta", "10", "ApiDocument");
            }
            //*********************************************************************************

            if (!Ini.KeyExists("PutanjaDoIniFajla", "ApiDocument"))
            {
                string putanjadofajla = System.IO.Path.Combine(ProjectPath, @"Konfiguracija\MojNoviIniFajl.ini");
                Ini.Write("PutanjaDoIniFajla", putanjadofajla.ToString(), "ApiDocument");
            }
            //**********************************************************************************
            if (!Ini.KeyExists("sacuvanfajl", "ApiDocument"))
            {
                Ini.Write("sacuvanfajl", "False", "ApiDocument");
            }
            //***********************************************************************************
            if (!Ini.KeyExists("Prikazisamogrid", "ApiDocument"))
            {
                Ini.Write("Prikazisamogrid", "False", "ApiDocument");
            }
            //****************************************************************************************
            if (!Ini.KeyExists("VremePoslednjeIzmenefajla", "ApiDocument"))
            {
                Ini.Write("VremePoslednjeIzmenefajla", DateTime.Now.ToShortTimeString());
            }

            if (!Ini.KeyExists("Vremekreiranjapdf", "ApiDocument"))
            {
                Ini.Write("Vremekreiranjapdf", DateTime.Now.ToShortTimeString(), "ApiDocument");
            }
            //*****************************************************************************************
            if (!Ini.KeyExists("Tip0", "ListaTipovaDodataka"))
            {
                Ini.Write("Tip0", ".jpg", "ListaTipovaDodataka");
            }

            if (!Ini.KeyExists("Tip1", "ListaTipovaDodataka"))
            {
                Ini.Write("Tip1", ".png", "ListaTipovaDodataka");
            }

            if (!Ini.KeyExists("Tip2", "ListaTipovaDodataka"))
            {
                Ini.Write("Tip2", ".gif", "ListaTipovaDodataka");
            }

            if (!Ini.KeyExists("Tip3", "ListaTipovaDodataka"))
            {
                Ini.Write("Tip3", ".txt", "ListaTipovaDodataka");
            }
            if (!Ini.KeyExists("Tip4", "ListaTipovaDodataka"))
            {
                Ini.Write("Tip4", ".zip", "ListaTipovaDodataka");
            }
            if (!Ini.KeyExists("Tip5", "ListaTipovaDodataka"))
            {
                Ini.Write("Tip5", " ", "ListaTipovaDodataka");
            }
            if (!Ini.KeyExists("Tip6", "ListaTipovaDodataka"))
            {
                Ini.Write("Tip6", " ", "ListaTipovaDodataka");
            }
            if (!Ini.KeyExists("Tip7", "ListaTipovaDodataka"))
            {
                Ini.Write("Tip7", " ", "ListaTipovaDodataka");
            }
            if (!Ini.KeyExists("Tip8", "ListaTipovaDodataka"))
            {
                Ini.Write("Tip8", " ", "ListaTipovaDodataka");
            }
            if (!Ini.KeyExists("Tip9", "ListaTipovaDodataka"))
            {
                Ini.Write("Tip9", " ", "ListaTipovaDodataka");
            }
            #endregion



        }

        #endregion

            #region DATAGRID SELECTED CHANGED

        private void dgulparam_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            lbAutoulazniparametri.Visibility = Visibility.Hidden;
        }

        private void dgSpeculparam_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            lbSpeculparamsugestija.Visibility = Visibility.Hidden;
        }

        private void dgTabulparam_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            lbTabelarniulparamSugestija.Visibility = Visibility.Hidden;
        }

        private void dgstatusgreske_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            lbAutosugrstijaGresaka.Visibility = Visibility.Hidden;
        }

        private void dgIzlaznevr_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            lbIzlazneSugestija.Visibility = Visibility.Hidden;
        }

        private void dgTabizlazniparam_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            lbTabizlaznevrSugestija.Visibility = Visibility.Hidden;
        }

        private void dgspizlazvre_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            lbSpecIzlvrSugestija.Visibility = Visibility.Hidden;
        }

        #endregion

            #region ARHIVIRANJE
        public void Zaarhivu()
        {
            CIniFile Ini = new CIniFile(ApiParametri.PutanjadoInifajla);
            string Arhiva = Ini.Read("Arhiva", "ApiDocument");
            string Drivezaarhivu = Ini.Read("DriveGdeArhivirimo", "ApiDocument");
            string projectPath = Drivezaarhivu.ToString();
            string folderName = System.IO.Path.Combine(projectPath, Arhiva.ToString());
            DirectoryInfo d = new DirectoryInfo(folderName);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            DirectoryInfo dorektorijum = new DirectoryInfo(folderName);

            foreach (var file in dorektorijum.GetFiles("*.zip"))
            {
                ApiParametri.ArhivaCount++;


            }
        }
        #endregion

            #region VALIDACIJA NA EXCPANDERU
        public void ValidacijanaExp()
        {
            #region PROVERA IMA LI PODATAKA U LISTAMA PARAMETARA

            if (dgulparam.Items.Count > 0)
            {
                ulazniparametri.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                ulazniparametri.Foreground = new SolidColorBrush(Colors.Red);
            }


            if (dgSpeculparam.Items.Count > 1)
            {
                Spulparam.Foreground = new SolidColorBrush(Colors.Black);

            }
            else
            {
                Spulparam.Foreground = new SolidColorBrush(Colors.Red);
            }

            if (dgTabulparam.Items.Count > 1)
            {
                Tabulparam.Foreground = new SolidColorBrush(Colors.Black);

            }
            else
            {
                Tabulparam.Foreground = new SolidColorBrush(Colors.Red);
            }

            if (dgstatusgreske.Items.Count > 1)
            {
                Statusigreski.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                Statusigreski.Foreground = new SolidColorBrush(Colors.Red);
            }

            if (dgIzlaznevr.Items.Count > 1)
            {
                Izlaznevrednosti.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                Izlaznevrednosti.Foreground = new SolidColorBrush(Colors.Red);
            }

            if (dgTabizlazniparam.Items.Count > 1)
            {
                Tabelarneizlaznevr.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                Tabelarneizlaznevr.Foreground = new SolidColorBrush(Colors.Red);
            }

            if (dgDodatak.Items.Count > 1)
            {
                dodaci.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                dodaci.Foreground = new SolidColorBrush(Colors.Red);
            }

            if (dgSarad.Items.Count > 1)
            {
                saradnici.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                saradnici.Foreground = new SolidColorBrush(Colors.Red);
            }

            if (dgspizlazvre.Items.Count > 1)
            {
                Specijalizovaneizlaznevr.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                Specijalizovaneizlaznevr.Foreground = new SolidColorBrush(Colors.Red);
            }

            #endregion
        }

        public void Validacijaexpopis()
        {
            if (string.IsNullOrWhiteSpace(txtnazivfunkcija.Text) && string.IsNullOrWhiteSpace(txtModul.Text))
            {
                Opisfunkcije.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtnazivfunkcija.Text) || string.IsNullOrWhiteSpace(txtModul.Text)
                  || string.IsNullOrWhiteSpace(new TextRange(rbOpisfunkcije.Document.ContentStart, rbOpisfunkcije.Document.ContentEnd).Text)
                  || string.IsNullOrWhiteSpace(new TextRange(rbopisLogikefunkcije.Document.ContentStart, rbopisLogikefunkcije.Document.ContentEnd).Text)
                  || string.IsNullOrWhiteSpace(new TextRange(rbOgrucestfunkc.Document.ContentStart, rbOgrucestfunkc.Document.ContentEnd).Text))

                {
                    Opisfunkcije.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    Opisfunkcije.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
        }
        public void ValidacijaScenarioexp()
        {
            if (string.IsNullOrWhiteSpace(txtnazivfunkcija.Text) && string.IsNullOrWhiteSpace(txtModul.Text))
            {
                Opisfunkcije.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {


                if (string.IsNullOrWhiteSpace(new TextRange(rbScenario.Document.ContentStart, rbScenario.Document.ContentEnd).Text))
                {

                    rbScenario.BorderBrush = Brushes.Red;
                    rbScenario.BorderThickness = new Thickness(1);
                    Scenario.Foreground = new SolidColorBrush(Colors.Red);

                }
                else
                {
                    Scenario.Foreground = new SolidColorBrush(Colors.Black);
                }
            }


        }
        #endregion


    }
}















