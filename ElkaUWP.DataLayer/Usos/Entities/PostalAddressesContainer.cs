using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class PostalAddressesContainer
    {
        [JsonProperty ("postal_addresses")]
        public List<PostalAddresses> PostalAddresses { get; set; }

    }

    public class PostalAddresses
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("type_name")]
        public LangDict TypeName { get; set; }
    }
}
