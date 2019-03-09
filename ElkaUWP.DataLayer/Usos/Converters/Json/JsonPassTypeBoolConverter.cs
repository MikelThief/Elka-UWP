using System;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Converters.Json
{
    public class JsonPassTypeBoolConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(bool);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString().ToUpper().Trim();

            switch (value)
            {
                // EJ zespół od USOSA, Wy kurwa jesteście normalni?
                case "Passed":
                case "PASSED":
                case "passed":
                    return true;
                case "not_yet_passed":
                case "NOT_YET_PASSED":
                case "Not_Yet_Passed":
                case "NotYetPassed":
                case "NOTYETPASSED":
                case "notyetpassed":
                    return false;
                default:
                    throw new JsonSerializationException(message: "Converter" + nameof(JsonPassTypeBoolConverter) + "could not handle the value: " + value);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is null)
            {
                serializer.Serialize(jsonWriter: writer, value: null);
                return;
            }

            var toSerialize = (bool) value;
            switch (toSerialize)
            {
                case true:
                    serializer.Serialize(jsonWriter: writer, "Passed");
                    return;
                case false:
                    serializer.Serialize(jsonWriter: writer, "NotYetPassed");
                    return;
                default:
                    throw new JsonSerializationException(message: "Converter" + nameof(JsonPassTypeBoolConverter) + "could not handle the value: " + value);
            }
        }
    }
}
