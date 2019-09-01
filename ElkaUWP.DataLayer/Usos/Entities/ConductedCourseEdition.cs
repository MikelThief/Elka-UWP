using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class ConductedCourseEdition
    {
        [JsonProperty(propertyName: "term")]
        public ConductedTerm Term { get; set; }

        [JsonProperty(propertyName: "id")]
        public string Id { get; set; }

        [JsonProperty(propertyName: "course")]
        public ConductedCourse Course { get; set; }
    }
}
