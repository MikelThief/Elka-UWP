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
using ElkaUWP.DataLayer.Studia.ResolverParameters;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.Infrastructure;
using Flurl.Http;
using Flurl.Http.Configuration;
using ElkaUWP.Infrastructure.Extensions;
using ElkaUWP.Infrastructure.Helpers;
using HttpMethod = System.Net.Http.HttpMethod;

namespace ElkaUWP.DataLayer.Studia.Strategies
{
    public class StudiaFormLogonStrategy : ILogonStrategy
    {
        private readonly IFlurlClient _restClient;

        private const string LoginPagePathSegment = "pl/19L/-/login/";
        private const string LdapPathSegment = "19L/-/login-ldap";

        private const string CookieAllowedFieldValue = "Zgadzam się";

        public LogonStrategyParametersContainer ParametersContainer { get; set; }

        public StudiaFormLogonStrategy(IFlurlClientFactory flurlClientFactory)
        {
            _restClient = flurlClientFactory.Get(url: Constants.STUDIA_BASE_URL).EnableCookies();
        }

        /// <inheritdoc />
        public LogonStrategies Name => LogonStrategies.StudiaForm;

        /// <inheritdoc />
        public Task InitializeAsync(LogonStrategyParametersContainer parametersContainer)
        {
            ParametersContainer = parametersContainer;
            //TODO: Check if login data are correct
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task AuthenticateAsync(string username, string password)
        {
            var unauthenticatedCookiesRequest = _restClient.Request().AppendPathSegment(segment: LoginPagePathSegment);

            var unauthenticatedCookiesResponse =
                await unauthenticatedCookiesRequest.PostUrlEncodedAsync(data: new {cookie_ok = CookieAllowedFieldValue});

            // manually extracting cookies. In-built behaviour is somewhat broken on UWP
            unauthenticatedCookiesResponse.Headers.TryGetValues(name: "Set-Cookie", values: out var cookiesFromResponse);

            var cookiesCollection = CookieHelper.GetAllCookiesFromHeader(
                strHeader: cookiesFromResponse.ToList().FirstOrDefault(),
                strHost: Constants.STUDIA_BASE_URL.Substring(startIndex: 1 + Constants.STUDIA_BASE_URL.LastIndexOf('/')));

            var authenticateCookieRequest = _restClient.Request().AppendPathSegment(segment: "pl/")
                .AppendPathSegment(segment: LdapPathSegment).WithCookie(cookie: cookiesCollection[0]);

            var authenticateCookieResponse = await authenticateCookieRequest.PostUrlEncodedAsync(data:
                new { studia_login = username, studia_passwd = password });
        }
    }
}