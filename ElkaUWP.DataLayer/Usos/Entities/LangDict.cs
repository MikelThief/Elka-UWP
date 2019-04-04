using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class LangDict
    {
        [JsonProperty(propertyName: "pl", NullValueHandling = NullValueHandling.Ignore)]
        public string Pl { get; private set; }

        [JsonProperty(propertyName: "en", NullValueHandling = NullValueHandling.Ignore)]
        public string En { get; private set; }}

}
