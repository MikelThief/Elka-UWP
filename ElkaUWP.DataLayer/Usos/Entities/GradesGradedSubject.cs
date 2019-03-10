using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class GradesGradedSubject
    {
        [JsonProperty("course_grades")]
        public List<CourseGrade> CourseGrades { get; set; }
    }
}
