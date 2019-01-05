using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Windows.Security.Credentials;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Abstractions.Interfaces;
using ElkaUWP.Infrastructure.Exceptions;
using ElkaUWP.Infrastructure.Extensions;
using ElkaUWP.Infrastructure.Services;
using NLog;
using OAuthClient;

namespace ElkaUWP.Modularity.LoginModule.Service
{
    public class UsosOAuthService : IUsosOAuthService
    {
        private readonly IReadOnlyCollection<string> _usosScopes = new List<string>()
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
        private ILogger Logger { get; }

        private readonly SecretService _secretService;

        public UsosOAuthService(SecretService secretService, ILogger logger)
        {
            Logger = logger;
            _secretService = secretService;
        }

        public async Task StartAuthorizationAsync()
        {
            var tokenRequest = new OAuthRequest()
            {
                Realm = Constants.USOSAPI_BASE_URL,
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
                response = await webClient.DownloadStringTaskAsync(address: requestUri);
            }
            catch (WebException exc)
            {
                Logger.Fatal(exception: exc, "Unable to start OAuth handshake");
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
                Logger.Warn(exception: exc, message: "USOS API returned ambiguous oauth_callback_confirmed value. Returned value is {value}",
                    args: (responseParametersCollection.Get(name: "oauth_callback_confirmed")));
                throw;
            }

            if (!callbackIsAccepted)
            {
                Logger.Warn(message: "USOS API does not support callback");
                throw new InvalidOperationException(message: "USOS API does not support callback");
            }

            await Windows.System.Launcher.LaunchUriAsync(uri: new Uri(uriString: Constants.USOSAPI_AUTHORIZE_URL+ "?oauth_token=" + _requestToken));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorizedRequestToken"></param>
        /// <param name="oauthVerifier"></param>
        /// <returns></returns>
        /// <exception cref="FailedOAuthWorkflowException">Thrown when OAuth session was started multiple times
        /// and application state doesn't match service provider callback</exception>
        public async Task<PasswordCredential> GetAccessAsync(string authorizedRequestToken, string oauthVerifier)
        {
            if (authorizedRequestToken != _requestToken)
            {
                Logger.Fatal("Received token does not match expected one. The value of expected token is {expectedToken}, and the actual value is {actualValue}", argument1: _requestToken, argument2: authorizedRequestToken);
                throw new FailedOAuthWorkflowException(expectedToken: _requestToken, actualToken: authorizedRequestToken);
            }
            else if (_requestToken == string.Empty)
            {
                Logger.Warn(
                    "Application's token is empty, so application's state is lost. It might took too long for a user to authorize token. It could expire anyway. Application will continue.");
            }

            var tokenRequest = new OAuthRequest()
            {
                Realm = Constants.USOSAPI_BASE_URL,
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
                response = await webClient.DownloadStringTaskAsync(address: requestUri);
            }
            catch (WebException exc)
            {
                Logger.Fatal(exception: exc, "USOS API refused to perform a token exchange");
                throw;
            }

            var responseParametersCollection = HttpUtility.ParseQueryString(query: response);

            var oauthAccessToken = responseParametersCollection.Get("oauth_token");
            var oauthTokenSecret = responseParametersCollection.Get("oauth_token_secret");

            return new PasswordCredential(resource: Constants.USOS_CREDENTIAL_CONTAINER_NAME, userName: oauthAccessToken, password: oauthTokenSecret);
        }
    }
}
