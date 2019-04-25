using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class SubjectStaffMember
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}