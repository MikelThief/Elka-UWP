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
                return await Task.Run<T>(() => JsonConvert.DeserializeObject<T>(value: value));
            }

            public static async Task<string> ToJsonAsync(object value)
            {
                return await Task.Run<string>(() => JsonConvert.SerializeObject(value: value));
            }
        }
    }
}
