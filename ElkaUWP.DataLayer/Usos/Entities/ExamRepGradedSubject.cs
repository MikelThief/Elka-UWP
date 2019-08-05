using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Converters;
using ElkaUWP.DataLayer.Usos.Converters.Json;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class ExamRepGradedSubject
    {
        [JsonProperty(propertyName: "type_id")]
        [JsonConverter(converterType: typeof(JsonSubjectPassTypeConverter))]
        public SubjectPassType TypeId { get; set; }

        [JsonProperty(propertyName: "id")]
        public int Id { get; set; }

        [JsonProperty(propertyName: "type_description")]
        public LangDict TypeDescription { get; set; }

        [JsonProperty(propertyName: "sessions")]
        public List<Session> Sessions { get; set; }
    }

}
