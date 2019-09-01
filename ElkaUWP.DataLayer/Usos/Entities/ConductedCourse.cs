using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class ConductedCourse
    {
        public partial class Course
        {
            /// <summary>
            /// In form course_string_id|semesterLiteral ex. 103C-INIIT-ISP-TKOM|2014L
            /// </summary>
            [JsonProperty(propertyName: "id")]
            public string Id { get; set; }

            [JsonProperty(propertyName: "name")]
            public LangDict Name { get; set; }
        }
    }
}
