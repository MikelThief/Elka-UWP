using System;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Converters.Json
{
    public class JsonTOrNBoolConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString().ToUpper().Trim();

            switch (value)
            {
                case "T":
                case "TRUE":
                    return true;
                case "N":
                case "FALSE":
                    return false;
                default:
                    throw new JsonSerializationException(message: "Converter" + nameof(JsonTOrNBoolConverter) + "could not handle the value: " + value);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
