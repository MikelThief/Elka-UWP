using ElkaUWP.DataLayer.Usos.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.DataLayer.Usos.Converters.Json
{
    class JsonPostalAddressesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(PostalAdresses) || objectType == typeof(PostalAdresses?);
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var value = serializer.Deserialize<string>(reader: reader);
            switch(value)
            {
                case "type":
                    return PostalAdresses.type;
                case "address":
                    return PostalAdresses.addresses;
                case "type_name":

                    return PostalAdresses.type_name;
                default:
                    return new JsonSerializationException(message: "Converter" + nameof(JsonPostalAddressesConverter) + "could not handle the value" + value);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {


            throw new NotImplementedException();
        }
    }
}
