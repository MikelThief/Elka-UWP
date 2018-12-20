using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Windows.Security.Credentials;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Extensions;
using ElkaUWP.Infrastructure.Interfaces;
using OAuth;

namespace ElkaUWP.LoginModule.Service
{
    public class UsosOOAuthService : IUsosOAuthService
    {
        private ICollection<string> usosScopes = new List<string>()
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

        private string requestTokenSecret;

        public UsosOOAuthService()
        {

        }

        public async Task AuthorizeAsync()
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

            var scopes = String.Join(separator: "%7C", values: usosScopes);
            var authString = tokenRequest.GetAuthorizationQuery(parameters: new NameValueCollection()
            {
                {"scopes", scopes}
            });
            var requestUri = new Uri(uriString: tokenRequest.RequestUrl + "?" + authString + "&scopes=" + scopes);

            var webClient = new WebClient();

            string response;

            try
            {
                response = await webClient.DownloadStringTaskAsync(address: requestUri);
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Status + " " + e.Response + " " + e.Response);
                throw;
            }

            var responseParametersCollection = HttpUtility.ParseQueryString(query: response);

            var requestToken = responseParametersCollection.Get(name: "oauth_token");
            requestTokenSecret = responseParametersCollection.Get(name: "oauth_token_secret");
            var callbackIsAccepted = Convert.ToBoolean(value: responseParametersCollection.Get(name: "oauth_callback_confirmed"));

            if(!callbackIsAccepted)
                throw new InvalidOperationException(message: "USOS API does not support callback.");
            await Windows.System.Launcher.LaunchUriAsync(uri: new Uri(uriString: Constants.USOSAPI_AUTHORIZE_URL+ "?oauth_token=" + requestToken));
        }

        public async Task GetAccessAsync(string oauthToken, string oauthVerifier)
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
                RequestUrl = Constants.USOSAPI_ACCESS_TOKEN_URL,
                Type = OAuthRequestType.AccessToken,
                Verifier = oauthVerifier,
                Token = oauthToken,
                TokenSecret = requestTokenSecret,
            };

            var accessString = tokenRequest.GetAuthorizationQuery();

            var requestUri = new Uri(uriString: tokenRequest.RequestUrl + "?" + accessString);

            var webClient = new WebClient();

            string response;

            try
            {
                response = await webClient.DownloadStringTaskAsync(address: requestUri);
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Status + " " + e.Response + " " + e.Response);
                throw;
            }

            var responseParametersCollection = HttpUtility.ParseQueryString(query: response);

            var oauthAccessToken = responseParametersCollection.Get(name: "oauth_token");
            var oauthTokenSecret = responseParametersCollection.Get(name: "oauth_token_secret");

            var credential = new PasswordCredential(resource: Constants.USOS_RESOURCE_TOKEN, userName: oauthAccessToken, password: oauthTokenSecret);

            new PasswordVault().AddUniversitySystemCredential(credential);
        }
    }
}
