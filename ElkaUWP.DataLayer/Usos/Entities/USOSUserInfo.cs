using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class USOSUserInfo
    {

        [JsonProperty("photo_urls")]
        public PhotoUrls PhotoUrls { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("sex")]
        public string Sex { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("staff_status")]
        public long StaffStatus { get; set; }

        [JsonProperty("student_number")]
        public string StudentNumber { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("middle_names")]
        public string MiddleName { get; set; }

 
    }
    public partial class PhotoUrls
    {
        [JsonProperty("100x100")]
        public Uri The100X100 { get; set; }
    }

    
}

   



