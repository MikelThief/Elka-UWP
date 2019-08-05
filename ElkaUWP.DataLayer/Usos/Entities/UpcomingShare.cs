using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class UpcomingShare
    {
        [JsonProperty(propertyName: "webcal_url")]
        public string WebCalUrl { get; set; }
    }
}
