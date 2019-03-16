using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class TestNode
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("description")]
        public LangDict Description { get; set; }

        [JsonProperty("root_id")]
        public int RootId { get; set; }

        [JsonProperty("parent_id")]
        public int? ParentId { get; set; }

        [JsonProperty("limit_to_groups")]
        public bool IsLimitedToGroups { get; set; }

        [JsonProperty("order")]
        public byte Order { get; set; }

        [JsonProperty("visible_for_students")]
        public bool VisibleForStudents { get; set; }

        [JsonProperty("course_edition")]
        public CourseEdition CourseEdition { get; set; }

        [JsonProperty("name")]
        public LangDict Name { get; set; }

        [JsonProperty("my_permissions")]
        public IList<object> MyPermissions { get; set; }

        [JsonProperty("public")]
        public bool IsPublic { get; set; }

        [JsonProperty("node_id")]
        public int NodeId { get; set; }
    }
}