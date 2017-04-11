using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDocument
{
   public class CIzlaznevr
    {
        private string _nazivizlazvr;
        [JsonProperty]
        public string Nazivizlaznevr{get { return _nazivizlazvr; }set { _nazivizlazvr = value; }}

        private string _opisizlaznevr;
        [JsonProperty]
        public string Opisizlaznevr {get { return _opisizlaznevr; }set { _opisizlaznevr = value; } }


    }
}
