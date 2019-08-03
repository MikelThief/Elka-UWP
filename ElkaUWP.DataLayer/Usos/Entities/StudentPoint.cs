using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class StudentPoint
    {
        [JsonProperty(propertyName: "points", NullValueHandling = NullValueHandling.Ignore)]
        public float? Points { get; set; }

        [JsonProperty(propertyName: "last_changed", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(converterType: typeof(IsoDateTimeConverter))]
        public DateTime? LastChanged { get; set; }

        [JsonProperty(propertyName: "comment", NullValueHandling = NullValueHandling.Ignore)]
        public string Comment { get; set; }

        [JsonProperty(propertyName: "grader", NullValueHandling = NullValueHandling.Ignore)]
        public Grader Grader { get; set; }
    }
}
