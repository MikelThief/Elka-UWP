using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public partial class EmploymentPosition
    {
        [JsonProperty(propertyName: "faculty")]
        public Faculty Faculty { get; set; }

        [JsonProperty(propertyName: "position")]
        public Faculty Position { get; set; }
    }
}
