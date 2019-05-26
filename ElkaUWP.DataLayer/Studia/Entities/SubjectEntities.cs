using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Studia.Entities
{
    public class Subject
    {
        [JsonProperty(propertyName: "status")]
        public string Status { get; set; }

        [JsonProperty(propertyName: "info")]
        public List<PartialGrade> PartialGrades { get; set; }

        [JsonProperty(propertyName: "chn")]
        public Attributes Attributes { get; set; }
    }

    public class Attributes
    {
        [JsonProperty(propertyName: "title_en")]
        public string TitleEn { get; set; }

        [JsonProperty(propertyName: "rola")]
        public string Rola { get; set; }

        [JsonProperty(propertyName: "kod")]
        public string Kod { get; set; }

        [JsonProperty(propertyName: "id")]
        public string Id { get; set; }

        [JsonProperty(propertyName: "lang")]
        public string Lang { get; set; }

        [JsonProperty(propertyName: "title_pl")]
        public string TitlePl { get; set; }

        [JsonProperty(propertyName: "skr")]
        public string Skr { get; set; }

        [JsonProperty(propertyName: "typ")]
        public string Typ { get; set; }
    }

    public class PartialGrade
    {
        [JsonProperty(propertyName: "title")]
        public string Title { get; set; }

        [JsonProperty(propertyName: "value")]
        public string Value { get; set; }
    }
}
