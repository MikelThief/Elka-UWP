using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Anotar.NLog;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.DataLayer.Studia.Flurl;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.Infrastructure;
using Flurl.Http;
using Flurl.Http.Configuration;
using ElkaUWP.Infrastructure.Extensions;
using ElkaUWP.Infrastructure.Helpers;
using HttpMethod = System.Net.Http.HttpMethod;

namespace ElkaUWP.DataLayer.Studia.Strategies
{
    public class LdapLogonStrategy : ILogonStrategy
    {
        private readonly IFlurlClient _restClient;

        private const string LdapPathSegment = "19L/-/login-ldap";

        private const string CookiesAllowedCookieName = "STUDIA_COOKIES";
        private const string CookiesAllowedCookieValue = "YES";
        private const string StudiaIdCookieName = "STUDIA_SID";

        public LdapLogonStrategy(IFlurlClientFactory flurlClientFactory)
        {
            _restClient = flurlClientFactory.Get(url: Constants.STUDIA_BASE_URL).EnableCookies();
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
            var unauthenticatedCookiesRequest = _restClient.Request().AppendPathSegment("pl/19L/-/login/");

            var unauthenticatedCookiesResponse =
                await unauthenticatedCookiesRequest.PostUrlEncodedAsync(new {cookie_ok = "Zgadzam się"});

            unauthenticatedCookiesResponse.Headers.TryGetValues("Set-Cookie", out var cookiesy);

            var cookiesCollection = CookieHelper.GetAllCookiesFromHeader(cookiesy.ToList().FirstOrDefault(), "studia3.pw.edu.pl");

            var authenticateCookieRequest = _restClient.Request().AppendPathSegment("pl/")
                .AppendPathSegment(segment: LdapPathSegment).WithCookie(cookiesCollection[0]);

            var authenticateCookieResponse = await authenticateCookieRequest.PostUrlEncodedAsync(data:
                new { studia_login = username, studia_passwd = password });


            var thefinalrequest = _restClient.Request().AppendPathSegment("pl/19L/103B-CTxxx-ISA-ECONE/api/info/")
                .WithCookie(cookiesCollection[0]);

            var respo = await thefinalrequest.GetStringAsync();
            return null;

        }
    }
}