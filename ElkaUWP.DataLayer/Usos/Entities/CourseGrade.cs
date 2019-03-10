using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class CourseGrade
    {
        [JsonProperty("1")]
        public CourseGradeSub1 Sub1 { get; set; }
    }
}
