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
        [JsonProperty(propertyName: "number")]
        public long Number { get; set; }

        [JsonProperty(propertyName: "issuer_grades", Required = Required.AllowNull)]
        public IssuerGrades IssuerGrades { get; set; }
    }
}
