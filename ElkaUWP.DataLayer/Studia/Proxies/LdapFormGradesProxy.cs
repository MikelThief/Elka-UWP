using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Anotar.NLog;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Helpers;
using ElkaUWP.Infrastructure.Services;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Studia.Proxies
{
    public class LdapFormGradesProxy : IGradesProxy
    {
        private readonly SecretService _secretService;
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

        public LdapFormGradesProxy(IFlurlClientFactory flurlClientFactory, SecretService secretService)
        {
            _secretService = secretService;
            _restClient = flurlClientFactory.Get(url: Constants.STUDIA_BASE_URL).EnableCookies();
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetAsync(string semesterLiteral, string subjectId)
        {
            if (!IsAuthenticated())
            {
                _restClient.Cookies.Clear();
                await Authenticate().ConfigureAwait(continueOnCapturedContext: false);
            }

            var request = _restClient.Request().AppendPathSegment(segment: PlPathSegment)
                .AppendPathSegments($"{semesterLiteral.Substring(startIndex: 3)}/", subjectId, "api", "info");

            try
            {
                return await request.GetAsync().ConfigureAwait(continueOnCapturedContext: true);
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
        }

        /// <inheritdoc />
        public bool IsAuthenticated() =>
            _restClient.Cookies.ContainsKey(key: StudiaAuthCookieName)
            && _restClient.Cookies[key: StudiaAuthCookieName].Expired;

        /// <inheritdoc />
        public async Task Authenticate()
        {
            var secret = _secretService.GetSecret(container: Constants.STUDIA_CREDENTIAL_CONTAINER_NAME);
            secret.RetrievePassword();

            _username = secret.UserName;
            _password = secret.Password;

            var unauthenticatedCookiesRequest = _restClient.Request()
                .AppendPathSegment(segment: PlPathSegment).AppendPathSegment(segment: LoginPagePathSegment);

            var unauthenticatedCookiesResponse =
                await unauthenticatedCookiesRequest.PostUrlEncodedAsync(data: new
                { cookie_ok = CookieAllowedFieldValue }).ConfigureAwait(continueOnCapturedContext: false);

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
                new { studia_login = _username, studia_passwd = _password }).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}