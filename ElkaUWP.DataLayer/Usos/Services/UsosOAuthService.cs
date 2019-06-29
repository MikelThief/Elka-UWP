using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Windows.Security.Credentials;
using Anotar.NLog;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Exceptions;
using ElkaUWP.Infrastructure.Services;
using OAuthClient;

namespace ElkaUWP.DataLayer.Usos.Services
{
    public class LogonService
    {
        private IReadOnlyCollection<string> _usosScopes = new List<string>()
        {
            "cards",
            "change_all_preferences",
            "crstests",
            "edit_user_attrs",
            "email",
            "other_emails",
            "grades",
            "mailclient",
            "mobile_numbers",
            "offline_access",
            "personal",
            "photo",
            "placement_tests",
            "slips",
            "slips_admin",
            "staff_perspective",
            "student_exams",
            "student_exams_write",
            "studies",
            "surveys_filling",
            "surveys_reports",
        };

        private string _requestToken = string.Empty;
        private string _requestTokenSecret = string.Empty;

        private readonly SecretService _secretService;

        public LogonService(SecretService secretService)
        {
            _secretService = secretService;
        }

        public async Task StartOAuthHandshakeAsync()
        {
            var tokenRequest = new OAuthRequest()
            {
                Method = "GET",
                Version = "1.0",
                ConsumerKey = Constants.USOS_CONSUMER_KEY,
                ConsumerSecret = Constants.USOS_CONSUMER_SECRET,
                SignatureTreatment = OAuthSignatureTreatment.Escaped,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                RequestUrl = Constants.USOSAPI_REQUEST_URL,
                CallbackUrl = Constants.PROTOCOL_URI,
                Type = OAuthRequestType.RequestToken,
            };

            var scopes = string.Join("%7C", values: _usosScopes);
            var authString = tokenRequest.GetAuthorizationQuery(parameters: new NameValueCollection()
            {
                {"scopes", scopes}
            });
            var requestUri = new Uri(uriString: $"{tokenRequest.RequestUrl}?{authString}");

            var webClient = new WebClient();

            string response;

            try
            {
                response = await webClient.DownloadStringTaskAsync(address: requestUri).ConfigureAwait(true);
            }
            catch (WebException wexc)
            {
                LogTo.FatalException(exception: wexc, message: "Unable to start OAuth handshake");
                throw;
            }

            var responseParametersCollection = HttpUtility.ParseQueryString(query: response);

            _requestToken = responseParametersCollection.Get(name: "oauth_token");
            _requestTokenSecret = responseParametersCollection.Get(name: "oauth_token_secret");
            var callbackIsAccepted = default(bool);
            try
            {
                callbackIsAccepted = Convert.ToBoolean(value: responseParametersCollection.Get(name: "oauth_callback_confirmed"));
            }
            catch (FormatException exc)
            {
                LogTo.WarnException(exception: exc, message: "USOS API returned ambiguous oauth_callback_confirmed value. Returned value is "
                                                             + (responseParametersCollection.Get(name: "oauth_callback_confirmed")));
                throw;
            }

            if (!callbackIsAccepted)
            {
                LogTo.Warn(message: "USOS API does not support callback");
                throw new InvalidOperationException(message: "USOS API does not support callback");
            }

            await Windows.System.Launcher.LaunchUriAsync(uri: new Uri(uriString: Constants.USOSAPI_AUTHORIZE_URL+ "?oauth_token=" + _requestToken));

        }

        public async Task<bool> TryFinishOAuthHandshakeAsync(string responseQueryString)
        {
            var responseParameters = HttpUtility.ParseQueryString(query: responseQueryString);

            var credential = await FinishOAuthHandshakeInternalAsync(
                authorizedRequestToken: responseParameters.Get(name: "oauth_token"),
                oauthVerifier: responseParameters.Get(name: "oauth_verifier")).ConfigureAwait(continueOnCapturedContext: true);

            if (credential == null)
            {
                return false;
            }

            _secretService.CreateOrUpdateSecret(providedCredential: credential);
            return true;

        }

        /// <summary>
        /// Finished OAuth handshake with USOS.
        /// </summary>
        /// <param name="authorizedRequestToken">Authorized token from USOS.</param>
        /// <param name="oauthVerifier">Verifier from USOS.</param>
        /// <returns></returns>
        /// <exception cref="FailedOAuthWorkflowException">Thrown when OAuth session was started multiple times
        /// and application state doesn't match service provider callback</exception>
        private async Task<PasswordCredential> FinishOAuthHandshakeInternalAsync(string authorizedRequestToken, string oauthVerifier)
        {
            if (authorizedRequestToken != _requestToken)
            {
                LogTo.Fatal( "Request token mismatch. Expected token is {expectedRequestToken}, actual is {actualRequestToken}", _requestToken, authorizedRequestToken);
                throw new FailedOAuthWorkflowException(expectedToken: _requestToken, actualToken: authorizedRequestToken);
            }

            var tokenRequest = new OAuthRequest()
            {
                Method = "GET",
                Version = "1.0",
                ConsumerKey = Constants.USOS_CONSUMER_KEY,
                ConsumerSecret = Constants.USOS_CONSUMER_SECRET,
                SignatureTreatment = OAuthSignatureTreatment.Escaped,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                RequestUrl = Constants.USOSAPI_ACCESS_TOKEN_URL,
                Type = OAuthRequestType.AccessToken,
                Verifier = oauthVerifier,
                Token = authorizedRequestToken,
                TokenSecret = _requestTokenSecret,
            };

            var accessString = tokenRequest.GetAuthorizationQuery();

            var requestUri = new Uri(uriString: $"{tokenRequest.RequestUrl}?{accessString}");

            var webClient = new WebClient();

            string response = default;

            try
            {
                response = await webClient.DownloadStringTaskAsync(address: requestUri).ConfigureAwait(true);
            }
            catch (WebException exc)
            {
                LogTo.WarnException(exception: exc, message: "Failed to perform a token exchange");
                return null;
            }

            var responseParametersCollection = HttpUtility.ParseQueryString(query: response);

            var oauthAccessToken = responseParametersCollection.Get("oauth_token");
            var oauthTokenSecret = responseParametersCollection.Get("oauth_token_secret");

            return new PasswordCredential(resource: Constants.USOS_CREDENTIAL_CONTAINER_NAME, userName: oauthAccessToken, password: oauthTokenSecret);
        }
    }
}
