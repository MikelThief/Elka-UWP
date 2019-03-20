using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Entities;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Converters.Json
{
    public class JsonNodeTypeConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is null)
            {
                serializer.Serialize(jsonWriter: writer, value: null);
                return;
            }

            var toSerialize = (NodeType)value;
            switch (toSerialize)
            {
                case NodeType.Root:
                    serializer.Serialize(jsonWriter: writer, "root");
                    return;
                case NodeType.Grade:
                    serializer.Serialize(jsonWriter: writer, "oc");
                    return;
                case NodeType.Task:
                    serializer.Serialize(jsonWriter: writer, "pkt");
                    return;
                case NodeType.Folder:
                    serializer.Serialize(jsonWriter: writer, "fld");
                    return;
                default:
                    throw new JsonSerializationException(message: "Converter" + nameof(JsonNodeTypeConverter) + "could not handle the value: " + value);
            }
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader: reader);
            switch (value)
            {
                // dokuemntacja jest chuja warta:
                // realne mapowanie tak jak w switchu
                case "root":
                    return NodeType.Root;
                case "fld":
                    return NodeType.Grade;
                case "oc":
                    return NodeType.Grade;
                case "pkt":
                    return NodeType.Task;
                default:
                    throw new JsonSerializationException(message: "Converter" + nameof(JsonNodeTypeConverter) + "could not handle the value: " + value);
            }
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType) => objectType == typeof(NodeType) || objectType == typeof(NodeType?);
    }
}
