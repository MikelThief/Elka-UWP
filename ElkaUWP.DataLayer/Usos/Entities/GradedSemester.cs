using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Converters;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class GradedSemester
    {
        [JsonProperty("type_id")]
        [JsonConverter(converterType: typeof(JsonSubjectPassTypeConverter))]
        public SubjectPassType TypeId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type_description")]
        public LangDict TypeDescription { get; set; }

        [JsonProperty("sessions")]
        public List<Session> Sessions { get; set; }
    }

}
