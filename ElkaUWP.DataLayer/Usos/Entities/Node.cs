using System.Collections.Generic;
using System.Diagnostics;
using ElkaUWP.DataLayer.Usos.Converters.Json;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    [DebuggerDisplay("NodeId = {NodeId}, Name = {Name}, Type = {Type}, VisibleForStudents = {VisibleForStudents}")]
    public class Node
    {
        [JsonProperty(propertyName: "type"), JsonConverter(converterType: typeof(JsonNodeTypeConverter))]
        public NodeType Type { get; set; }

        [JsonProperty(propertyName: "description")]
        public LangDict Description { get; set; }

        [JsonProperty(propertyName: "root_id")]
        public int RootId { get; set; }

        [JsonProperty(propertyName: "parent_id")]
        public int? ParentId { get; set; }

        [JsonProperty(propertyName: "order")]
        public byte Order { get; set; }

        [JsonProperty(propertyName: "visible_for_students")]
        public bool VisibleForStudents { get; set; }

        [JsonProperty(propertyName: "course_edition")]
        public CourseEdition CourseEdition { get; set; }

        [JsonProperty(propertyName: "name")]
        public LangDict Name { get; set; }

        [JsonProperty(propertyName: "public")]
        public bool IsPublic { get; set; }

        [JsonProperty(propertyName: "node_id")]
        public int NodeId { get; set; }

        [JsonProperty(propertyName: "subnodes")]
        public List<Node> SubNodes { get; set; }

        [JsonProperty(propertyName: "points_max", NullValueHandling = NullValueHandling.Ignore)]
        public long? PointsMax { get; set; }

        [JsonProperty(propertyName: "points_min", NullValueHandling = NullValueHandling.Ignore)]
        public long? PointsMin { get; set; }

    }
}