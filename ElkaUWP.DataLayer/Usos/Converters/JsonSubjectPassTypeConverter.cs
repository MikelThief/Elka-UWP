using System;
using ElkaUWP.DataLayer.Usos.Entities;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Converters
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
                    serializer.Serialize(writer, "E");
                    return;
                case SubjectPassType.Semester:
                    serializer.Serialize(writer, "S");
                    return;
                case SubjectPassType.PassOrFail:
                    serializer.Serialize(writer, "Z");
                    return;
                default:
                    throw new JsonSerializationException(message: "Cannot serialize: SubjectPassType does not contain value " + value);
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
                    throw new JsonSerializationException(message: "Cannot deserialize: SubjectPassType does not contain value " + value);
            }

        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}
