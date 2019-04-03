using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Converters.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class PostalAddress
    {
        [JsonProperty(propertyName: "type")]
        [JsonConverter(converterType: typeof(JsonPostalAddressesConverter))]
        public PostalAddressType Type { get; set; }

        [JsonProperty(propertyName: "address")]
        public string Address { get; set; }

        [JsonProperty(propertyName: "type_name")]
        public LangDict TypeName { get; set; }
    }
}
