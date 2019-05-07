using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Anotar.NLog;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.Infrastructure;
using Flurl.Http;
using Flurl.Http.Configuration;
using HttpMethod = System.Net.Http.HttpMethod;

namespace ElkaUWP.DataLayer.Studia.Strategies
{
    public class LdapLogonStrategy : ILogonStrategy
    {
        private readonly IFlurlClient _restClient;

        private const string LdapPathSegment = "19L/-/login-ldap";

        private const string CookiesAllowedCookieName = "STUDIA_COOKIES";
        private const string CookiesAllowedCookieValue = "YES&";
        private const string StudiaIdCookieName = "STUDIA_SID";

        public LdapLogonStrategy(IFlurlClientFactory flurlClientFactory)
        {
            _restClient = flurlClientFactory.Get(url: Constants.STUDIA_BASE_URL)
                .WithCookie(name: CookiesAllowedCookieName, value: CookiesAllowedCookieValue);
        }

        /// <inheritdoc />
        public LogonStrategies Name => LogonStrategies.LdapAsForm;

        /// <inheritdoc />
        public Cookie GetSessionCookie(string username, string password)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<Cookie> GetSessionCookieAsync(string username, string password)
        {
            var unauthenticatedCookiesRequest = _restClient.Request();

            var unauthenticatedCookiesResponse = await unauthenticatedCookiesRequest.SendAsync(verb: HttpMethod.Get);

            if (!_restClient.Cookies.ContainsKey(key: StudiaIdCookieName))
                throw new Exception(message: "Failed to get unauthenticated cookie from Studia");

            var authenticateCookieRequest = _restClient.Request().AppendPathSegment(segment: LdapPathSegment);

            var authenticateCookieResponse = authenticateCookieRequest.PostUrlEncodedAsync(data:
                new { studia_login = username, studia_passwd = password });




            return null;

        }
    }
}
