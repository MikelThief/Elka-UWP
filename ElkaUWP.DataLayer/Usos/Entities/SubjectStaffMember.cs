using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class SubjectStaffMember
    {
        [JsonProperty(propertyName: "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(propertyName: "user_id")]
        public long UserId { get; set; }

        [JsonProperty(propertyName: "last_name")]
        public string LastName { get; set; }

        [JsonProperty(propertyName: "id")]
        public int Id { get; set; }
    }
}