using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class Titles
    {
        [JsonProperty(propertyName: "before")]
        public string Before { get; set; }

        [JsonProperty(propertyName: "after")]
        public object After { get; set; }
    }
}
