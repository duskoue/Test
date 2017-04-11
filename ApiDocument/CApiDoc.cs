using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ApiDocument
{
    public class CApiDoc 
    {
        #region PROPERTIES OPIS API FUNKCIJA

        private string _nazivapifunkcije;

        [JsonProperty]
        public string NazivApiFunkcije { get { return _nazivapifunkcije; } set { _nazivapifunkcije = value; } }



        private string _modul;
        [JsonProperty]
        public string Modul { get { return _modul; } set { _modul = value; } }



        private string _opisfunkcije;
        [JsonProperty]

        public string Opisfunkcije { get { return _opisfunkcije; } set { _opisfunkcije = value; } }


        private string _opislogikefunkcije;
        [JsonProperty]
        public string Opislogikefunkcije { get { return _opislogikefunkcije; } set { _opislogikefunkcije = value; } }



        private bool _kesiranje;
        [JsonProperty]
        public bool Kesiranje { get { return _kesiranje; } set { _kesiranje = value; } }



        private string _ogrucestalostifunkcija;
        [JsonProperty]
        public string Ogrucestalostifunkcija { get { return _ogrucestalostifunkcija; } set { _ogrucestalostifunkcija = value; } }

        public string Scenario { get { return _scenario; } set { _scenario = value; } }

        #endregion

        #region LISTE PARAMETARA

        private string _listaulparam;
        private string _listaspeculparam;
        private string _scenario;

       
        
        public string ListaUlaznihParametara { get { return _listaulparam; } set { _listaulparam = value; } }
        public string ListaSpecijalizovanihUlaznihParametara { get { return _listaspeculparam; } set { _listaspeculparam = value; } }

        private string _listatabulparam;

        public string ListaTabelarnihUlaznihParametara{get { return _listatabulparam; }set { _listatabulparam = value; }}

        private string _listagreske;

        public string ListaParametaraStatusiGresaka{get { return _listagreske; }set { _listagreske = value; }}

        private string _listaizlvr;

        public string ListaIzlaznihParametara{get { return _listaizlvr; }set { _listaizlvr = value; }}
        private string _listaspizlazvr;

        public string ListaSpecijalizovanihIzlaznihParametara {get { return _listaspizlazvr; }set { _listaspizlazvr = value; }}

        private string _listadodatak;

        public string ListaPazametarazaDodatke{get { return _listadodatak; } set { _listadodatak = value; }}

        private string _ltabizlaznevr;

        public string ListaTabelarnihIzlaznihParametara{ get { return _ltabizlaznevr; }set { _ltabizlaznevr = value; }}

        private string _lisrasarad;

        public string ListaSaradnika{ get { return _lisrasarad; }set { _lisrasarad = value; }}

        #endregion



        #region SAVE I OPEN METODE

        public void Save(string savePath)
        {
            try
            {
                using (StreamWriter file = File.CreateText(savePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, this);


                }
            }
            catch (Exception)
            {

                return;
            }
           
        }

     
        public static CApiDoc LoadFromFile(string filename)
        {

            try
            {
                using (StreamReader file = File.OpenText(filename))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var API = (CApiDoc)serializer.Deserialize(file, typeof(CApiDoc));
                    return API;

                }
            }
            catch (Exception)
            {

                return null;
            }
          
        }
        #endregion
    }
}


