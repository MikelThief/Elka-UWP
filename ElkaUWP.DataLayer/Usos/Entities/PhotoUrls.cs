using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class PhotoUrls
    {
        [JsonProperty(propertyName: "200x250")]
        public Uri PhotoUri { get; set; }
    }
}
