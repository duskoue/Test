using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDocument
{
   public class CTabIzlazniParametri
    {
        private string _nazivtabelarniizlazniparam;
        [JsonProperty]
        public string Nazivtabelarniizlazniparam { get { return _nazivtabelarniizlazniparam; } set { _nazivtabelarniizlazniparam = value; } }

        private string _poljetabelarniizlazniparam;
        [JsonProperty]
        public string Poljetabelarniizlazniparam { get { return _poljetabelarniizlazniparam; } set { _poljetabelarniizlazniparam = value; } }

        private string _opistabelarniizlazniparam;
        [JsonProperty]
        public string Opistabelarniizlazniparam { get { return _opistabelarniizlazniparam; } set { _opistabelarniizlazniparam = value; } }




        private string _tipizlazniparam;
        [JsonProperty]
        public string Tipizlazniparam { get { return _tipizlazniparam; } set { _tipizlazniparam = value; } }

        private string _duzinapojlaizlazniparam;
        [JsonProperty]
        public string Duzinapoljaizlazniparam { get { return _duzinapojlaizlazniparam; } set { _duzinapojlaizlazniparam = value; } }
        private string _infosysizlazniparam;

        public string Infosysizlazniparam { get { return _infosysizlazniparam; } set { _infosysizlazniparam = value; } }
    }
}
