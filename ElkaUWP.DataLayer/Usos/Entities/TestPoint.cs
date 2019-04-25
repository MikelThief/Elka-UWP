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
        [JsonProperty("node_id")]
        public int NodeId { get; set; }

        [JsonProperty("last_changed")]
        public DateTime LastChanged { get; set; }

        [JsonProperty("grader_id")]
        public int GraderId { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("points")]
        public float Points { get; set; }

        [JsonProperty("student_id")]
        public int StudentId { get; set; }
    }
}
