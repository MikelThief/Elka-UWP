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
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Interfaces;
using OAuth;

namespace ElkaUWP.LoginModule.Service
{
    public class UsosOAuthService : IUsosAuthService
    {
        private OAuthRequest tokenRequest;
        private OAuthRequest authorizationRequest;

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

        public UsosOAuthService()
        {

        }

        public async Task AuthorizeAsync()
        {
            tokenRequest = new OAuthRequest()
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

            var scopes = String.Join(separator: "%7C", values: usosScopes);
            var authString = tokenRequest.GetAuthorizationQuery(new NameValueCollection()
            {
                {"scopes", scopes}
            });
            var requestUri = new Uri(uriString: tokenRequest.RequestUrl + "?" + authString + "&scopes=" + scopes);

            var webClient = new WebClient();

            var response = await webClient.DownloadStringTaskAsync(address: requestUri);

            // redirects flow to browser
            //await Windows.System.Launcher.LaunchUriAsync(uri: requestUri);

            var responseParametersCollection = HttpUtility.ParseQueryString(query: response);

            var requestToken = responseParametersCollection.Get(name: "oauth_token");
            var requestTokenSecret = responseParametersCollection.Get(name: "oauth_token_secret");
            var callbackIsAccepted = Convert.ToBoolean(value: responseParametersCollection.Get("oauth_callback_confirmed"));

            if(!callbackIsAccepted)
                throw new InvalidOperationException(message: "USOS API does not support callback.");

            authorizationRequest = OAuthRequest.ForAccessToken(Constants.USOS_CONSUMER_KEY, Constants.USOS_CONSUMER_SECRET, requestToken, requestTokenSecret);
            authorizationRequest.RequestUrl = Constants.USOSAPI_AUTHORIZE_URL;

            var authorizeUrl = authorizationRequest.GetAuthorizationQuery();

            await Windows.System.Launcher.LaunchUriAsync(uri: new Uri(uriString: Constants.USOSAPI_AUTHORIZE_URL+ "?oauth_token=" + requestToken));

        }
    }
}
