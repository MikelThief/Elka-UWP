using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class MarkerStyle
    {
        [JsonProperty("color")]
        public string Color { get; private set; }
    }
}
