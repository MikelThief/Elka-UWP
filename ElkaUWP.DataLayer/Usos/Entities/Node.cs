using System.Collections.Generic;
using ElkaUWP.DataLayer.Usos.Converters.Json;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class Node
    {
        [JsonProperty("type"), JsonConverter(converterType: typeof(JsonNodeTypeConverter))]
        public NodeType Type { get; set; }

        [JsonProperty("description")]
        public LangDict Description { get; set; }

        [JsonProperty("root_id")]
        public int RootId { get; set; }

        [JsonProperty("parent_id")]
        public int? ParentId { get; set; }

        /*
        Returns list of groups if the list exists or... false if not
        TODO: Deserialization requires custom (de)serializer
        Co za kretyn to wymyślił...
        [JsonProperty("limit_to_groups")]
        public List<Group> GroupsList { get; set; }
        */

        [JsonProperty("order")]
        public byte Order { get; set; }

        [JsonProperty("visible_for_students")]
        public bool VisibleForStudents { get; set; }

        [JsonProperty("course_edition")]
        public CourseEdition CourseEdition { get; set; }

        [JsonProperty("name")]
        public LangDict Name { get; set; }

        [JsonProperty("public")]
        public bool IsPublic { get; set; }

        [JsonProperty("node_id")]
        public int NodeId { get; set; }

        [JsonProperty("subnodes")]
        public List<Node> SubNodes { get; set; }
    }
}