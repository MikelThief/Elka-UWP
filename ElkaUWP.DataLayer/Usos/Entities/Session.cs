using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class Session
    {
        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("issuer_grades")]
        public IssuerGrades IssuerGrades { get; set; }
    }
}
