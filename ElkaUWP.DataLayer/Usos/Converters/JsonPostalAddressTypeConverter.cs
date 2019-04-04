using ElkaUWP.DataLayer.Usos.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.DataLayer.Usos.Converters.Json
{
    class JsonPostalAddressTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(PostalAddressType) || objectType == typeof(PostalAddressType?);
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var value = serializer.Deserialize<string>(reader: reader);
            switch(value)
            {
                case "primary":
                    return PostalAddressType.Primary;
                case "correspondence":
                    return PostalAddressType.Correspondence;
                case "residence":
                    return PostalAddressType.Residence;
                default:
                    return PostalAddressType.Other;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
