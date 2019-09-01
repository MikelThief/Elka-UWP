using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    [DebuggerDisplay("Name = {User.FirstName} {User.LastName}, Id = {User.Id}")]
    public class MatchedUserItem
    {
        [JsonProperty(propertyName: "user")]
        public SearchedUser User { get; set; }

        [JsonProperty(propertyName: "match")]
        public string Match { get; set; }
    }
}
