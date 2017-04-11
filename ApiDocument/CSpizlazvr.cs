using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDocument
{
  public  class CSpizlazvr
    {
        private string _spnazivizlazvr;
        [JsonProperty]
        public string Nazivspizlaznevr { get { return _spnazivizlazvr; } set { _spnazivizlazvr = value; } }

        private string _spopisizlaznevr;
        [JsonProperty]
        public string Opisspizlaznevr { get { return _spopisizlaznevr; } set { _spopisizlaznevr = value; } }
        
        [JsonProperty]
        private string _defizlazvr;

        public string Defizlazvr { get { return _defizlazvr; }set { _defizlazvr = value; }}

        private string _obaveznost;
        [JsonProperty]
        public string Obaveznost{ get { return _obaveznost; }set { _obaveznost = value; }}

        private string _tipspecizlazvre;
        [JsonProperty]
        public string TipSpecijalneIzlaznevrednosti{ get { return _tipspecizlazvre; } set { _tipspecizlazvre = value; }}


    }
}
