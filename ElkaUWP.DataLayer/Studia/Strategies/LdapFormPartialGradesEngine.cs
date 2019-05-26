using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Anotar.NLog;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Entities;
using ElkaUWP.DataLayer.Studia.Flurl;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.Infrastructure;
using Flurl.Http;
using Flurl.Http.Configuration;
using ElkaUWP.Infrastructure.Extensions;
using ElkaUWP.Infrastructure.Helpers;
using Newtonsoft.Json;
using HttpCompletionOption = System.Net.Http.HttpCompletionOption;
using HttpMethod = System.Net.Http.HttpMethod;

namespace ElkaUWP.DataLayer.Studia.Strategies
{
    public class LdapFormPartialGradesEngine : IPartialGradesEngine
    {
        private readonly IFlurlClient _restClient;

        private const string PlPathSegment = "pl/";
        private const string LoginPagePathSegment = "19L/-/login/";
        private const string LdapPagePathSegment = "19L/-/login-ldap";
        private const string PersonEndpointPathSegment = "-/api/person/";
        private const string ClassesEndPointPathSegment = "-/api/classes/";

        private const string CookieAllowedFieldValue = "Zgadzam się";
        private const string StudiaAuthCookieName = "STUDIA_SID";
        private readonly string _username;
        private readonly string _password;

        public LdapFormPartialGradesEngine(IFlurlClientFactory flurlClientFactory)
        {
            _restClient = flurlClientFactory.Get(url: Constants.STUDIA_BASE_URL).EnableCookies();
        }

        /// <inheritdoc />
        public async Task<Subject> GetPartialGradesAsync(string subjectId)
        {
            if (!IsAuthenticated())
            {
                _restClient.Cookies.Clear();
                await Authenticate().ConfigureAwait(continueOnCapturedContext: false);
            }

            var request = _restClient.Request().AppendPathSegment(segment: PlPathSegment)
                .AppendPathSegments("19L/", subjectId, "api/info");

            Subject subject;
            try
            {
                var response = await request.GetAsync().ConfigureAwait(continueOnCapturedContext: true);
                subject = JsonConvert.DeserializeObject<Subject>(value: response.Content.ToString());
            }
            catch (FlurlHttpException fexc)
            {
                LogTo.ErrorException(message: $"Failed to retrieve partial grades for {subjectId}", exception: fexc);
                throw;
            }
            catch (JsonException jexc)
            {
                LogTo.ErrorException(message: $"Failed to deserialize partial grades json for {subjectId}", exception:
                    jexc);
                throw;
            }
            catch (Exception exc)
            {
                LogTo.ErrorException(message: $"Unexpected exception occured while getting partial grades for {subjectId}",
                    exception: exc);
                throw;
            }

            return subject;
        }

        /// <inheritdoc />
        public bool IsAuthenticated() =>
            _restClient.Cookies.ContainsKey(key: StudiaAuthCookieName) && _restClient.Cookies[key: StudiaAuthCookieName].Expired;

        /// <inheritdoc />
        public async Task Authenticate()
        {
            var unauthenticatedCookiesRequest = _restClient.Request()
                .AppendPathSegment(segment: PlPathSegment).AppendPathSegment(segment: LoginPagePathSegment);

            var unauthenticatedCookiesResponse =
                await unauthenticatedCookiesRequest.PostUrlEncodedAsync(data: new
                    {cookie_ok = CookieAllowedFieldValue}).ConfigureAwait(continueOnCapturedContext: false);

            // manually extracting cookies. In-built behaviour is somewhat broken on UWP
            unauthenticatedCookiesResponse.Headers.TryGetValues(name: "Set-Cookie",
                values: out var cookiesFromResponse);

            var cookiesCollection = CookieHelper.GetAllCookiesFromHeader(
                strHeader: cookiesFromResponse.ToList().Single(),
                strHost: Constants.STUDIA_BASE_URL.Substring(
                    startIndex: 1 + Constants.STUDIA_BASE_URL.LastIndexOf(value: '/')));

            var authenticateCookieRequest = _restClient.Request()
                .AppendPathSegment(segment: PlPathSegment)
                .AppendPathSegment(segment: LdapPagePathSegment)
                .WithCookie(cookie: cookiesCollection[index: 0]);

            var authenticateCookieResponse = await authenticateCookieRequest.PostUrlEncodedAsync(data:
                new {studia_login = _username, studia_passwd = _password}).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}