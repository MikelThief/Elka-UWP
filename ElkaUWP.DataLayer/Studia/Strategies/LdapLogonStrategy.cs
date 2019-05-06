using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.Infrastructure;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace ElkaUWP.DataLayer.Studia.Strategies
{
    public class LdapLogonStrategy : ILogonStrategy
    {
        private readonly IFlurlClient _restClient;

        public LdapLogonStrategy(IFlurlClientFactory flurlClientFactory)
        {
            _restClient = flurlClientFactory.Get(url: Constants.STUDIA_BASE_URL).EnableCookies();
        }

        /// <inheritdoc />
        public LogonStrategies Name => LogonStrategies.LdapAsForm;

        /// <inheritdoc />
        public HttpCookie GetSessionCookie(string username, string password)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<HttpCookie> GetSessionCookieAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
