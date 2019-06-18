using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace ElkaUWP.DataLayer.Studia.Flurl
{
    public class NoRedirectHttpClientFactory : DefaultHttpClientFactory
    {
        private readonly HttpMessageHandler _httpMessageHandler;

        public NoRedirectHttpClientFactory(HttpMessageHandler httpMessageHandler)
        {
            _httpMessageHandler = httpMessageHandler;
        }

        public override HttpMessageHandler CreateMessageHandler()
        {
            var handler = _httpMessageHandler;
            return handler;
        }
    }
}

