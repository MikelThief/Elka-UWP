using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.Infrastructure.Converters;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class ClassGroup2
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("group_number")]
        public int GroupNumber { get; set; }

        [JsonProperty("building_name")]
        public LangDict BuildingName { get; set; }

        [JsonProperty("course_name")]
        public LangDict CourseName { get; set; }

        [JsonProperty("unit_id")]
        public int UnitId { get; set; }

        [JsonProperty("building_id")]
        public string BuildingId { get; set; }

        [JsonProperty("classtype_id")]
        public string ClasstypeId { get; set; }

        [JsonProperty("end_time"), JsonConverter(typeof(UsosDateTimeConverter))]
        public DateTime EndTime { get; set; }

        [JsonProperty("end_time"), JsonConverter(typeof(UsosDateTimeConverter))]
        public DateTime StartTime { get; set; }

        [JsonProperty("classtype_name")]
        public LangDict ClasstypeName { get; set; }

        [JsonProperty("course_id")]
        public string CourseId { get; set; }

        [JsonProperty("room_number")]
        public string RoomNumber { get; set; }

        [JsonProperty("cgwm_id")]
        public int CgwmId { get; set; }

        [JsonProperty("room_id")]
        public int RoomId { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        /* classgroup_profile_url - same as URL
        [JsonProperty("classgroup_profile_url")]
        public Uri Url { get; set; }
        */
    }
}
