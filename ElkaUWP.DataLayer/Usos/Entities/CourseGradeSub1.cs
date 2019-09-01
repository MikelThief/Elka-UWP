using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Converters.Json;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class CourseGradeSub1
    {
        [JsonProperty(propertyName: "value_symbol")]
        public string ValueSymbol { get; set; }

        [JsonProperty(propertyName: "counts_into_average"), JsonConverter(converterType: typeof(JsonTOrNBoolConverter))]
        public bool CountsIntoAverage { get; set; }

        [JsonProperty(propertyName: "passes")]
        public bool Passes { get; set; }

        [JsonProperty(propertyName: "exam_session_number")]
        public short ExamSessionNumber { get; set; }
    }
}
