using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class SearchedUser
    {
        [JsonProperty(propertyName: "last_name")]
        public string LastName { get; set; }

        [JsonProperty(propertyName: "id")]
        public int Id { get; set; }

        [JsonProperty(propertyName: "titles")]
        public Titles Titles { get; set; }

        [JsonProperty(propertyName: "employment_positions")]
        public List<EmploymentPosition> EmploymentPositions { get; set; }

        [JsonProperty(propertyName: "first_name")]
        public string FirstName { get; set; }
    }
}
