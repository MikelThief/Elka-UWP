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
        [JsonProperty("name")]
        public LangDict Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("order_key")]
        public int OrderKey { get; set; }

        [JsonProperty("start_date"), JsonConverter(converterType: typeof(IsoDateTimeConverter))]
        public DateTime StartDate { get; set; }

        [JsonProperty("end_date"), JsonConverter(converterType: typeof(IsoDateTimeConverter))]
        public DateTime EndDate { get; set; }

        [JsonProperty("finish_date"), JsonConverter(converterType: typeof(IsoDateTimeConverter))]
        public DateTime FinishDate { get; set; }
    }
}
