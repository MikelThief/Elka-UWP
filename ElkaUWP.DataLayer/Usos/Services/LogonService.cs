using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Windows.Security.Credentials;
using Anotar.NLog;
using CSharpFunctionalExtensions;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Services;
using OAuthClient;

namespace ElkaUWP.DataLayer.Usos.Services
{
    public class LogonService
    {
        private static readonly IReadOnlyCollection<string> _usosScopes = new List<string>
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

        public async Task<Result> BeginOAuthHandshakeAsync()
        {
            var tokenRequest = new OAuthRequest()
            {
                Method = "GET",
                Version = "1.0",
                ConsumerKey = Secrets.USOS_CONSUMER_KEY,
                ConsumerSecret = Secrets.USOS_CONSUMER_SECRET,
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
                response = await webClient.DownloadStringTaskAsync(address: requestUri).ConfigureAwait(continueOnCapturedContext: true);
            }
            catch (WebException wexc)
            {
                LogTo.ErrorException(message: "Unable to start OAuth handshake.", exception: wexc);
                return Result.Fail(error: ErrorCodes.USOS_HANDSHAKE_FAILED);
            }

            var responseParametersCollection = HttpUtility.ParseQueryString(query: response);

            _requestToken = responseParametersCollection.Get(name: "oauth_token");
            _requestTokenSecret = responseParametersCollection.Get(name: "oauth_token_secret");
            bool callbackIsAccepted;
            try
            {
                callbackIsAccepted = Convert.ToBoolean(value: responseParametersCollection.Get(name: "oauth_callback_confirmed"));
            }
            catch (FormatException exc)
            {
                LogTo.ErrorException(message: "USOS API returned ambiguous oauth_callback_confirmed value. Returned value is "
                                                             + (responseParametersCollection.Get(name: "oauth_callback_confirmed")), exception: exc);
                return Result.Fail(error: ErrorCodes.USOS_HANDSHAKE_FAILED);
            }

            if (!callbackIsAccepted)
            {
                LogTo.Error(message: "USOS API does not support callback.");
                return Result.Fail(error: ErrorCodes.USOS_HANDSHAKE_FAILED);
            }

            await Windows.System.Launcher.LaunchUriAsync(uri: new Uri(uriString: Constants.USOSAPI_AUTHORIZE_URL+ "?oauth_token=" + _requestToken));
            return Result.Ok();
        }

        public async Task<Result> FinishOAuthHandshakeAsync(string responseQueryString)
        {
            var responseParameters = HttpUtility.ParseQueryString(query: responseQueryString);

            var (_, isFailure, value, error) = await FinishOAuthHandshakeInternalAsync(
                authorizedRequestToken: responseParameters.Get(name: "oauth_token"),
                oauthVerifier: responseParameters.Get(name: "oauth_verifier")).ConfigureAwait(continueOnCapturedContext: true);

            if(isFailure)
                return Result.Fail(error: error);

            _secretService.CreateOrUpdateSecret(providedCredential: value);
            return Result.Ok();
        }

        /// <summary>
        /// Finishes OAuth handshake with USOS.
        /// </summary>
        /// <param name="authorizedRequestToken">Authorized token from USOS.</param>
        /// <param name="oauthVerifier">Verifier from USOS.</param>
        /// <returns><see cref="PasswordCredential"/> if handshake was successful.</returns>
        private async Task<Result<PasswordCredential>> FinishOAuthHandshakeInternalAsync(string authorizedRequestToken, string oauthVerifier)
        {
            if (authorizedRequestToken != _requestToken)
            {
                LogTo.Error( "Request token mismatch. Expected token is {expectedRequestToken}, actual is {actualRequestToken}", _requestToken, authorizedRequestToken);
                return Result.Fail<PasswordCredential>(error: ErrorCodes.USOS_HANDSHAKE_FAILED);
            }

            var tokenRequest = new OAuthRequest()
            {
                Method = "GET",
                Version = "1.0",
                ConsumerKey = Secrets.USOS_CONSUMER_KEY,
                ConsumerSecret = Secrets.USOS_CONSUMER_SECRET,
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

            string response;
            try
            {
                response = await webClient.DownloadStringTaskAsync(address: requestUri).ConfigureAwait(true);
            }
            catch (WebException exc)
            {
                LogTo.WarnException(exception: exc, message: "Failed to perform a token exchange.");
                return Result.Fail<PasswordCredential>(error: ErrorCodes.USOS_BAD_DATA_RECEIVED);
            }

            var responseParametersCollection = HttpUtility.ParseQueryString(query: response);

            var oauthAccessToken = responseParametersCollection.Get(name: "oauth_token");
            var oauthTokenSecret = responseParametersCollection.Get(name: "oauth_token_secret");

            var credential = new PasswordCredential(resource: Constants.USOS_CREDENTIAL_CONTAINER_NAME, userName: oauthAccessToken, password: oauthTokenSecret);

            return Result.Ok(value: credential);
        }
    }
}
