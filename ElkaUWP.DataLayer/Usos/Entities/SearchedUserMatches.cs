using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class SearchedUserMatches
    {
        [JsonProperty(propertyName: "items")]
        public List<MatchedUserItem> Items { get; set; }
    }
}
