using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDocument
{
    class DynamicContractResolver : DefaultContractResolver
    {
       
            private readonly string _StartingWithChar;

            public DynamicContractResolver(string StartingWithChar)
            {
                _StartingWithChar = StartingWithChar;
            }

            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerializayion)
            {
                IList<JsonProperty> properties = base.CreateProperties(type, memberSerializayion);

                //Serializacija po karaktery properties

                properties = properties.Where(p => p.PropertyName.Contains(_StartingWithChar.ToString())).ToList();
                return properties;
            }

        }
    }
