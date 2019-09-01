using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class StudentProgramme
    {
        [JsonProperty(propertyName: "programme")]
        public Programme Programme { get; set; }

        [JsonProperty(propertyName: "id")]
        public int Id { get; set; }
    }

}
