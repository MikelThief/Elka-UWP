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
        [JsonProperty(propertyName: "group_number")]
        public int GroupNumber { get; set; }

        [JsonProperty(propertyName: "building_name")]
        public LangDict BuildingName { get; set; }

        [JsonProperty(propertyName: "course_name")]
        public LangDict CourseName { get; set; }

        [JsonProperty(propertyName: "unit_id")]
        public int UnitId { get; set; }

        [JsonProperty(propertyName: "building_id")]
        public string BuildingId { get; set; }

        [JsonProperty(propertyName: "classtype_id")]
        public string ClasstypeId { get; set; }

        [JsonProperty(propertyName: "end_time"), JsonConverter(converterType: typeof(UsosDateTimeConverter))]
        public DateTime EndTime { get; set; }

        [JsonProperty(propertyName: "start_time"), JsonConverter(converterType: typeof(UsosDateTimeConverter))]
        public DateTime StartTime { get; set; }

        [JsonProperty(propertyName: "classtype_name")]
        public LangDict ClasstypeName { get; set; }

        [JsonProperty(propertyName: "course_id")]
        public string CourseId { get; set; }

        [JsonProperty(propertyName: "room_number")]
        public string RoomNumber { get; set; }

        [JsonProperty(propertyName: "cgwm_id")]
        public int CgwmId { get; set; }

        [JsonProperty(propertyName: "room_id")]
        public int RoomId { get; set; }

        [JsonProperty(propertyName: "url")]
        public Uri Url { get; set; }

        /* classgroup_profile_url - same as URL
        [JsonProperty("classgroup_profile_url")]
        public Uri Url { get; set; }
        */
    }
}
