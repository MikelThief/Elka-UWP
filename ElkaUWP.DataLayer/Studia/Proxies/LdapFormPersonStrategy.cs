using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Entities;
using ElkaUWP.Infrastructure;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace ElkaUWP.DataLayer.Studia.Proxies
{
    public class LdapFormPersonStrategy : IPersonStrategy
    {
        private readonly IFlurlClient _restClient;

        private const string PlPathSegment = "pl/";

        public LdapFormPersonStrategy(IFlurlClientFactory flurlClientFactory)
        {
            _restClient = flurlClientFactory.Get(url: Constants.STUDIA_BASE_URL).EnableCookies();
        }

        public Task<HttpResponseMessage> GetAsync()
        {
            var request = _restClient.Request().AppendPathSegment(segment: PlPathSegment)
                .AppendPathSegments("19L/", "-", "api", "person", "/");
            return request.GetAsync();
        }
    }
}
