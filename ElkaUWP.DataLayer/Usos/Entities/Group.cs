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
        [JsonProperty(propertyName: "group_number")]
        public int GroupNumber { get; set; }

        [JsonProperty(propertyName: "course_unit_id")]
        public int CourseUnitId { get; set; }

    }
}
