using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ElkaUWP.Infrastructure.Helpers
{
    public static class JsonHelper
    {
        public static class Json
        {
            public static async Task<T> FromJsonAsync<T>(string value)
            {
                return await Task.Run<T>(function: () => JsonConvert.DeserializeObject<T>(value: value));
            }

            public static async Task<string> ToJsonAsync(object value)
            {
                return await Task.Run<string>(function: () => JsonConvert.SerializeObject(value: value));
            }
        }
    }
}
