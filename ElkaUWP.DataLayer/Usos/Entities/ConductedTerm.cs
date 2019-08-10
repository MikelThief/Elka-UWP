using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class ConductedTerm
    {
        [JsonProperty(propertyName: "id")]
        public string Id { get; set; }

        [JsonProperty(propertyName: "name")]
        public LangDict Name { get; set; }
    }
}
