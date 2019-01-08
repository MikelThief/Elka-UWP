using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class Location
    {
        [JsonProperty("lat")]
        public double Lat { get; private set; }

        [JsonProperty("long")]
        public double Long { get; private set; }
    }
}
