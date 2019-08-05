using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class MarkerStyle
    {
        [JsonProperty(propertyName: "color")]
        public string Color { get; private set; }
    }
}
