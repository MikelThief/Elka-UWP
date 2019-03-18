using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class Group
    {
        [JsonProperty("group_number")]
        public int GroupNumber { get; set; }

        [JsonProperty("course_unit_id")]
        public int CourseUnitId { get; set; }

    }
}
