using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDocument
{
    public class CSaradnici
    {
        private string _sifrasaradnika;
        [JsonProperty]
        public string Sifrasaradnika { get { return _sifrasaradnika; } set { _sifrasaradnika = value; } }

    }
}

