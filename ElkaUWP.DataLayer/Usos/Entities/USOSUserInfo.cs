using ElkaUWP.DataLayer.Usos.Converters.Json;
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

        [JsonProperty("student_status")]
        public int StudentStatus { get; set; }

        [JsonProperty("pesel")]
        public string Pesel { get; set; }

        [JsonProperty("titles")]
        public string Titles { get; set; }

        [JsonProperty("has_email")]
        public bool HasEmail { get; set; }

        [JsonProperty("phone_numbers")]
        public List<String> PhoneNumbers { get; set; }

        [JsonProperty("office_hours")]
        public LangDict OfficeHours { get; set; }

        [JsonProperty("has_photo")]
        public bool HasPhoto { get; set; }

        [JsonProperty("birth_date")]
        public string BirthDate { get; set; }

        [JsonProperty("postal_addresses"), JsonConverter(converterType:typeof(JsonPostalAddressesConverter)) ]
        public PostalAddressesContainer PostalAddressesList {get;set;}
        
    }
    public partial class PhotoUrls
    {
        [JsonProperty("200x250")]
        public Uri PhotoUri { get; set; }
    }

    
}

   



