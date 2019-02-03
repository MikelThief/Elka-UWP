using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace ElkaUWP.Infrastructure.Converters
{
    public class UsosDateTimeConverter : IsoDateTimeConverter
    {
        public UsosDateTimeConverter() => DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    }
}
