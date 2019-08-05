using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.Infrastructure.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class Semester
    {
        [JsonProperty(propertyName: "name")]
        public LangDict Name { get; set; }

        [JsonProperty(propertyName: "id")]
        public string Id { get; set; }

        [JsonProperty(propertyName: "order_key")]
        public int OrderKey { get; set; }

        [JsonProperty(propertyName: "start_date"), JsonConverter(converterType: typeof(IsoDateTimeConverter))]
        public DateTime StartDate { get; set; }

        [JsonProperty(propertyName: "end_date"), JsonConverter(converterType: typeof(IsoDateTimeConverter))]
        public DateTime EndDate { get; set; }

        [JsonProperty(propertyName: "finish_date"), JsonConverter(converterType: typeof(IsoDateTimeConverter))]
        public DateTime FinishDate { get; set; }
    }
}
