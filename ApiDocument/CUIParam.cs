using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDocument
{
   public class CUIParam
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }



        private string _nazivulazniparametri;
        [JsonProperty]
        public string Nazivulazniparametri { get { return _nazivulazniparametri; } set { _nazivulazniparametri = value; } }

        private string _opisulazniparam;
        [JsonProperty]
        public string Opisulazniparam { get { return _opisulazniparam; } set { _opisulazniparam = value; } }

        private string _obaveznostulanogparam;
        [JsonProperty]
        public string Obaveznostulaznogparam { get { return _obaveznostulanogparam; } set { _obaveznostulanogparam = value; } }

        private string _defaultulaznogparam;
        [JsonProperty]
        public string Defaultulaznogparam { get { return _defaultulaznogparam; } set { _defaultulaznogparam = value; } }

        private string _tipulparametra;
        [JsonProperty]
        public string Tipulaznogparametra{ get { return _tipulparametra; }set { _tipulparametra = value; } }

        private string _duzinaulparametra;
        [JsonProperty]
        public string Duzinaulaznogparametra { get { return _duzinaulparametra; } set { _duzinaulparametra = value; } }




    }
}
