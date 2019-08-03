using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Studia.Entities
{
    public class PersonContainer
    {
        [JsonProperty(propertyName: "status")]
        public string Status { get; set; }

        [JsonProperty(propertyName: "person")]
        public Person Person { get; set; }
    }

    public class Person
    {
        [JsonProperty(propertyName: "_")]
        public PersonEntry CommonEntry { get; set; }

        // Currently not deserialized as API delivers same information multiple times
        //[JsonProperty("LDAP_mbator")]
        //public PersonEntry PersonEntry { get; set; }
    }

    public class PersonEntry
    {
        [JsonProperty(propertyName: "tytul")]
        public string Title { get; set; }

        [JsonProperty(propertyName: "xids")]
        public List<string> Identities { get; set; }

        [JsonProperty(propertyName: "imiona")]
        public string Names { get; set; }

        [JsonProperty(propertyName: "mail")]
        public string EMail { get; set; }

        [JsonProperty(propertyName: "nazwisko")]
        public string Surname { get; set; }

        [JsonProperty(propertyName: "kategoria")]
        public string Category { get; set; }

        // meaning is currently unknown
        [JsonProperty(propertyName: "tytul_po")]
        public string TitleAfter { get; set; }

        [JsonProperty(propertyName: "login")]
        public string Login { get; set; }

        // similar property is already present
        //[JsonProperty("XIDS")]
        //public List<string> Xids { get; set; }
    }
}
