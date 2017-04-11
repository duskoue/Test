using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDocument
{
       public class CTabUlparam
    {
        private string _nazivtabelarniulparam;
        [JsonProperty]
        public string Nazivtabelarniulparam { get { return _nazivtabelarniulparam; } set { _nazivtabelarniulparam = value; } }

        private string _poljetabelarniulparam;
        [JsonProperty]
        public string Poljetabelarniulparam { get { return _poljetabelarniulparam; } set { _poljetabelarniulparam = value; } }

        private string _opistabelarniulparam;
        [JsonProperty]
        public string Opistabelarniulparam { get { return _opistabelarniulparam; } set { _opistabelarniulparam = value; } }

       


        private string _tipulparam;
        [JsonProperty]
        public string Tipulparam {get { return _tipulparam; }set { _tipulparam = value; }}

        private string _duzinapojlaulparam;
        [JsonProperty]
        public string Duzinapoljaulparam{get { return _duzinapojlaulparam; }set { _duzinapojlaulparam = value; }}
        private string _infosysulparam;

        public string Infosusulparam {get { return _infosysulparam; }set { _infosysulparam = value; }}


    }
}
