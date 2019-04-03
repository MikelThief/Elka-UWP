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
        public List<PostalAddress> PostalAddresses { get; set; }

    }

   
}
