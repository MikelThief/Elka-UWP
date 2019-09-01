using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class TestPoint
    {
        [JsonProperty(propertyName: "node_id")]
        public int NodeId { get; set; }

        [JsonProperty(propertyName: "last_changed")]
        public DateTime LastChanged { get; set; }

        [JsonProperty(propertyName: "grader_id")]
        public int GraderId { get; set; }

        [JsonProperty(propertyName: "comment")]
        public string Comment { get; set; }

        [JsonProperty(propertyName: "points")]
        public float Points { get; set; }

        [JsonProperty(propertyName: "student_id")]
        public int StudentId { get; set; }
    }
}
