using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDocument
{
  public  class CStatusGreske
    {
        private string _identifikatorgreske;
        [JsonProperty]
        public string Identigikatorgreske { get { return _identifikatorgreske; } set { _identifikatorgreske = value; } }

        private string _tipgreske;
        [JsonProperty]
        public string Tipgreske { get { return _tipgreske; } set { _tipgreske = value; } }

        private string _opisgreske;
        [JsonProperty]
        public string Opisgreske { get { return _opisgreske; } set { _opisgreske = value; } }
    }
}
