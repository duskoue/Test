using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDocument
{
  public  class CDodatak
    {
        private string _dodataknaziv;
        [JsonProperty]
        public string Dodataknaziv { get { return _dodataknaziv; } set { _dodataknaziv = value; } }

        private string _dodataktip;
        [JsonProperty]
        public string Dodataktip { get { return _dodataktip; } set { _dodataktip = value; } }

        private string _opisdodatka;
        [JsonProperty]
        public string Opisdodatka { get { return _opisdodatka; } set { _opisdodatka = value; } }

        [JsonProperty]
        private string _napomenadodatak;

        public string NapomeanaDodatak{get { return _napomenadodatak; }set { _napomenadodatak = value; } }

    }
}
