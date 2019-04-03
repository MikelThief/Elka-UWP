using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class UserInfoContainer
    {
        [JsonProperty(propertyName: "photo_urls")]
        public PhotoUrls PhotoUrls { get; set; }

        [JsonProperty(propertyName: "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(propertyName: "sex")]
        public char Sex { get; set; }

        [JsonProperty(propertyName: "postal_addresses")]
        public List<PostalAddress> PostalAddresses { get; set; }

        [JsonProperty(propertyName: "id")]
        public int Id { get; set; }

        [JsonProperty(propertyName: "email")]
        public string Email { get; set; }

        [JsonProperty(propertyName: "middle_names")]
        public string MiddleNames { get; set; }

        [JsonProperty(propertyName: "student_number")]
        public int IndexNumber { get; set; }

        [JsonProperty(propertyName: "pesel")]
        public string Pesel { get; set; }

        [JsonProperty(propertyName: "last_name")]
        public string LastName { get; set; }

        [JsonProperty(propertyName: "student_status")]
        public short? StudentStatus { get; set; }
    }
}
