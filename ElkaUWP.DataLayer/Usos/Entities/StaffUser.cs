using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class StaffUser
    {
        [JsonProperty(propertyName: "titles")] public Titles Titles { get; set; }

        [JsonProperty(propertyName: "employment_positions")]
        public List<EmploymentPosition> EmploymentPositions { get; set; }

        [JsonProperty(propertyName: "course_editions_conducted")]
        public List<ConductedCourseEdition> CourseEditionsConducted { get; set; }

        [JsonProperty(propertyName: "staff_status")]
        public short StaffStatus { get; set; }

        [JsonProperty(propertyName: "last_name")]
        public string LastName { get; set; }

        [JsonProperty(propertyName: "office_hours")]
        public LangDict OfficeHours { get; set; }

        [JsonProperty(propertyName: "profile_url")]
        public Uri ProfileUrl { get; set; }

        [JsonProperty(propertyName: "id")] public int Id { get; set; }

        [JsonProperty(propertyName: "first_name")]
        public string FirstName { get; set; }
    }
}
