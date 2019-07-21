using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Helpers;
using ElkaUWP.Infrastructure.Services;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace ElkaUWP.DataLayer.Studia.Proxies
{
    public class LdapFormLogonStrategy : ILogonStrategy
    {
        private readonly IFlurlClient _restClient;

        private const string PlPathSegment = "pl/";
        private const string LoginPagePathSegment = "19L/-/login/";
        private const string LdapPagePathSegment = "19L/-/login-ldap";
        private const string PersonEndpointPathSegment = "-/api/person/";
        private const string ClassesEndPointPathSegment = "-/api/classes/";

        private const string CookieAllowedFieldValue = "Zgadzam się";
        private const string StudiaAuthCookieName = "STUDIA_SID";
        private string _username;
        private string _password;

        public LdapFormLogonStrategy(IFlurlClientFactory flurlClientFactory)
        {
            _restClient = flurlClientFactory.Get(url: Constants.STUDIA_BASE_URL).EnableCookies();
        }

        /// <inheritdoc />
        public bool IsAuthenticated() =>
            _restClient.Cookies.ContainsKey(key: StudiaAuthCookieName)
            && _restClient.Cookies[key: StudiaAuthCookieName].Expired;

        /// <inheritdoc />
        public async Task AuthenticateAsync(PasswordCredential credential)
        {

            _username = credential.UserName;
            _password = credential.Password;

            var unauthenticatedCookiesRequest = _restClient.Request()
                .AppendPathSegment(segment: PlPathSegment).AppendPathSegment(segment: LoginPagePathSegment);

            var unauthenticatedCookiesResponse =
                await unauthenticatedCookiesRequest.PostUrlEncodedAsync(data: new
                    {cookie_ok = CookieAllowedFieldValue}).ConfigureAwait(continueOnCapturedContext: false);

            // manually extracting cookies. In-built behaviour is somewhat broken on UWP
            unauthenticatedCookiesResponse.Headers.TryGetValues(name: "Set-Cookie",
                values: out var cookiesFromResponse);

            unauthenticatedCookiesResponse.Dispose();

            var cookiesCollection = CookieHelper.GetAllCookiesFromHeader(
                strHeader: cookiesFromResponse.ToList().Single(),
                strHost: Constants.STUDIA_BASE_URL.Substring(
                    startIndex: 1 + Constants.STUDIA_BASE_URL.LastIndexOf(value: '/')));

            var authenticateCookieRequest = _restClient.Request()
                .AppendPathSegment(segment: PlPathSegment)
                .AppendPathSegment(segment: LdapPagePathSegment)
                .WithCookie(cookie: cookiesCollection[index: 0]);

            var authenticateCookieResponse = await authenticateCookieRequest.PostUrlEncodedAsync(data:
                    new {studia_login = _username, studia_passwd = _password})
                .ConfigureAwait(continueOnCapturedContext: false);

            authenticateCookieResponse.Dispose();
        }
    }
}
