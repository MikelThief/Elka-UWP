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
    public class CourseEdition
    {
        [JsonProperty(propertyName: "term_id")]
        public string TermId { get; set; }

        [JsonProperty(propertyName: "passing_status")]
        [JsonConverter(converterType: typeof(JsonPassTypeBoolConverter))]
        public bool PassingStatus { get; set; }

        [JsonProperty(propertyName: "course_name")]
        public LangDict CourseName { get; set; }

        [JsonProperty(propertyName: "course_id")]
        public string CourseId { get; set; }

        [JsonProperty(propertyName: "lecturers")]
        public List<SubjectStaffMember> Lecturers { get; set; }

        [JsonProperty(propertyName: "profile_url")]
        public Uri ProfileUrl { get; set; }

        [JsonProperty(propertyName: "coordinators")]
        public List<SubjectStaffMember> Coordinators { get; set; }
    }
}
