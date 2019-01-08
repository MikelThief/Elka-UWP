using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class Building
    {
        [JsonProperty("phone_numbers")]
        public List<string> PhoneNumbers { get; private set; }

        [JsonProperty("marker_style")]
        public MarkerStyle MarkerStyle { get; private set; }

        /*
        USOS API returns error 400: not available
        [JsonProperty("postal_adress")]
        public string PostalAdress { get; private set; }
        */

        /*
        Not used - needs extra treatment to implement.
        Rather useless because of having geo data and MapControl
        [JsonProperty("static_map_urls")]
        public List<string> PostalAdress { get; private set; }
        */

        /*
        related_faculties - not allowed by USOS API. State unknown.
        */

        [JsonProperty("location")]
        public Location Location { get; private set; }

        [JsonProperty("campus_name")]
        public LangDict CampusName { get; private set; }

        [JsonProperty("name")]
        public LangDict Name { get; private set; }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("profile_url")]
        public Uri ProfileUrl { get; private set; }
    }
}
