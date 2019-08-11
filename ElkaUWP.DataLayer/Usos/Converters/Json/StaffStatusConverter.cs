using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Entities;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Converters.Json
{
    public class StaffStatusConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<sbyte>(reader: reader);
            switch (value)
            {
                case 0:
                    return StaffStatus.InactiveOrNotAStaff;
                case 1:
                    return StaffStatus.ActiveStaff;
                case 2:
                    return StaffStatus.ActiveTeacher;
                default:
                    throw new JsonSerializationException(
                        message: "Converter" + nameof(StaffStatusConverter) + "could not handle the value: " + value);
            }

        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(StaffStatus) || objectType == typeof(StaffStatus?);
    }
}
