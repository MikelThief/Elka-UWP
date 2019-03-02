using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class UserCoursesPerSemester
    {
        [JsonProperty("course_editions")]
        public Dictionary<string, List<CourseEdition>> CourseEditions { get; set; }
    }
}
