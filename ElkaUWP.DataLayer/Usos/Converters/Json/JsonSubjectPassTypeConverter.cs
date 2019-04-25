using System;
using ElkaUWP.DataLayer.Usos.Entities;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Converters.Json
{
    public class JsonSubjectPassTypeConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is null)
            {
                serializer.Serialize(jsonWriter: writer, value: null);
                return;
            }

            var toSerialize = (SubjectPassType)value;
            switch (toSerialize)
            {
                case SubjectPassType.Exam:
                    serializer.Serialize(jsonWriter: writer, "E");
                    return;
                case SubjectPassType.Semester:
                    serializer.Serialize(jsonWriter: writer, "S");
                    return;
                case SubjectPassType.PassOrFail:
                    serializer.Serialize(jsonWriter: writer, "Z");
                    return;
                default:
                    throw new JsonSerializationException(message: "Converter" + nameof(JsonSubjectPassTypeConverter) + "could not handle the value: " + value);
            }
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader: reader);
            switch (value)
            {
                case "E":
                    return SubjectPassType.Exam;
                case "S":
                    return SubjectPassType.Semester;
                case "Z":
                    return SubjectPassType.PassOrFail;
                default:
                    throw new JsonSerializationException(message: "Converter" + nameof(JsonSubjectPassTypeConverter) + "could not handle the value: " + value);
            }

        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType) => objectType == typeof(SubjectPassType) || objectType == typeof(SubjectPassType?);
    }
}
