using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;

namespace ElkaUWP.DataLayer.Studia.Flurl
{
    public static class FlurlClientExtensions
    {
        public static IFlurlClient DisableRedirects(this IFlurlClient fc)
        {
            var httpClientHandler = new HttpClientHandler {AllowAutoRedirect = false};
            fc.Settings.HttpClientFactory = new NoRedirectHttpClientFactory(httpClientHandler);
            return fc;
        }
    }
}
