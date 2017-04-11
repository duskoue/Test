using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDocument
{ 
  public  class CSpecUlparam
    {
        private string _specnazivulazniparametri;
        
        [JsonProperty]
        public string specNazivulazniparametri { get { return _specnazivulazniparametri; } set { _specnazivulazniparametri = value; } }

        private string _specopisulazniparam;
        [JsonProperty]
        public string specOpisulazniparam { get { return _specopisulazniparam; } set { _specopisulazniparam = value; } }

        private string _specobaveznostulanogparam;
        [JsonProperty]
        public string specObaveznostulaznogparam { get { return _specobaveznostulanogparam; } set { _specobaveznostulanogparam = value; } }

        private string _specdefaultulaznogparam;
        [JsonProperty]
        public string specDefaultulaznogparam { get { return _specdefaultulaznogparam; } set { _specdefaultulaznogparam = value; } }

        private string _specTipulparam;
        [JsonProperty]
        public string specTipulparam { get { return _specTipulparam; }set { _specTipulparam = value; } }

    }
}
