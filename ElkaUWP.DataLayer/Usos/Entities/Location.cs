using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class Location
    {
        [JsonProperty(propertyName: "lat")]
        public double Lat { get; private set; }

        [JsonProperty(propertyName: "long")]
        public double Long { get; private set; }
    }
}
