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
        [JsonProperty("value_symbol")]
        public string ValueSymbol { get; set; }

        [JsonProperty("counts_into_average"), JsonConverter(converterType: typeof(JsonTOrNBoolConverter))]
        public bool CountsIntoAverage { get; set; }

        [JsonProperty("passes")]
        public bool Passes { get; set; }

        [JsonProperty("exam_session_number")]
        public short ExamSessionNumber { get; set; }
    }
}
