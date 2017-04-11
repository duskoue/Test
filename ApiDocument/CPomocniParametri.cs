using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDocument
{
    public class CPomocniParametri : INotifyPropertyChanged
    {
        private bool _vidljivostkomandesave;
        private bool _ucitanfajl;

        public bool Ucitanfajl
        {
            get { return _ucitanfajl; }
            set { _ucitanfajl = value; }
        }

        private bool _stanjedugmetaObavljeno;

        public bool StanjedugmetaObavljeno
        {
            get { return _stanjedugmetaObavljeno; }
            set { _stanjedugmetaObavljeno = value; }
        }
        private bool _dalijeobjavljenfajl;

        public bool Dalijeobjavljenfajl
        {
            get { return _dalijeobjavljenfajl; }
            set { _dalijeobjavljenfajl = value; }
        }



        private bool _pagingsortexp;

        public bool PagingSortExp
        {
            get { return _pagingsortexp; }
            set { _pagingsortexp = value; }
        }

        private bool _pagingselectedpage;

        public bool PagingSelectedPage
        {
            get { return _pagingselectedpage; }
            set { _pagingselectedpage = value; }
        }


        private bool _pagingpagelen;

        public bool PagingPagelen
        {
            get { return _pagingpagelen; }
            set { _pagingpagelen = value; }
        }


        public bool VidljivostkomandeSave
        {
            get { return _vidljivostkomandesave; }
            set
            {
                if (value != _vidljivostkomandesave)
                {
                    _vidljivostkomandesave = value;
                    OnPropertyChangeg("Validacija");

                }
            }
        }



        private string _vremekreiranjapdf;

        public string Vremekreiranjapdf
        {
            get { return _vremekreiranjapdf; }
            set { _vremekreiranjapdf = value; }
        }

        private string _vremeposlednjeizmenefajla;



        public string Vremeposlednjeizmenefajla
        {
            get { return _vremeposlednjeizmenefajla; }
            set { _vremeposlednjeizmenefajla = value; }
        }

        private bool _sacuvano;

        public bool Sacuvano
        {
            get { return _sacuvano; }
            set { _sacuvano = value; }
        }

        private string _putanjasacuvanogdukumenta;

        public string PutanjaSacuvanogDokumenta
        {
            get { return _putanjasacuvanogdukumenta; }
            set { _putanjasacuvanogdukumenta = value; }
        }

        private bool _samogrid;

        public bool PrikaziSamoGrid
        {
            get { return _samogrid; }
            set { _samogrid = value; }
        }

        private string  _putanjadoinifajla;

        public string  PutanjadoInifajla
        {
            get { return _putanjadoinifajla; }
            set { _putanjadoinifajla = value; }
        }

        private string _velicinafonta;

        public string VelicinaFonta
        {
            get { return _velicinafonta; }
            set { _velicinafonta = value; }
        }


        private string _zapamcenaPutanjaExplorer;

        public string ZapamcenaPutanjaExplorer
        {
            get { return _zapamcenaPutanjaExplorer; }
            set { _zapamcenaPutanjaExplorer = value; }
        }


        private string _fontfamily;

        public string FontFamily
        {
            get { return _fontfamily; }
            set { _fontfamily = value; }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChangeg(string pNaziv)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pNaziv));
            }
        }

        private string _putanjatrenutnopokrenutogfajla;

        public string PutanjaTrenutnoPokrenutogFajla
        {
            get { return _putanjatrenutnopokrenutogfajla; }
            set { _putanjatrenutnopokrenutogfajla = value; }
        }
        private string _extensija;

        public string Extensija
        {
            get { return _extensija; }
            set { _extensija = value; }
        }

        private string _strfilename;

        public string StrFilename
        {
            get { return _strfilename; }
            set { _strfilename = value; }
        }
        private string _forsiraniparametri;

        public string ForsiraniParametri
        {
            get { return _forsiraniparametri; }
            set { _forsiraniparametri = value; }
        }
        private bool _vidljivostsaveas;

        public bool VidljivostSaveAs
        {
            get { return _vidljivostsaveas; }
            set { _vidljivostsaveas = value; }
        }
        private string _slikaJpg;

        public string SlikaJPG{get { return _slikaJpg; }set { _slikaJpg = value; }}

        private string _slikaPng;

        public string SlikaPNG { get { return _slikaPng; } set { _slikaPng = value; } }

        private string _slikaGif;

        public string SlikaGif { get { return _slikaGif; } set { _slikaGif = value; } }

        private string _text;

        public string Text { get { return _text; } set { _text = value; } }

        private string _zip;

        public string ZIP { get { return _zip; } set { _zip = value; } }

        private string _ext1;

        public string Ext1 { get { return _ext1; } set { _ext1 = value; } }

        private string _ext2;

        public string Ext2 { get { return _ext2; } set { _ext2 = value; } }
        private string _ext3;

        public string Ext3 { get { return _ext3; } set { _ext3 = value; } }
        private string _ext4;

        public string Ext4 { get { return _ext4; } set { _ext4 = value; } }
        private string _ext5;

        public string Ext5 { get { return _ext5; } set { _ext5 = value; } }
        private string _ext6;

        public string Ext6 { get { return _ext6; } set { _ext6 = value; } }

        private int _arhivacount;

        public int ArhivaCount
        {
            get { return _arhivacount; }
            set { _arhivacount = value; }
        }
        private bool _sinhroniyacija;

        public bool SinhroniyacijaListi
        {
            get { return _sinhroniyacija; }
            set { _sinhroniyacija = value; }
        }

    }
}
