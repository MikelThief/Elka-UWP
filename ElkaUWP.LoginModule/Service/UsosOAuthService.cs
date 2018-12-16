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
        private OAuthRequest request;

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
            request= new OAuthRequest()
            {
                Method = "GET",
                Version = "1.0",
                ConsumerKey = Constants.USOS_CONSUMER_KEY,
                ConsumerSecret = Constants.USOS_CONSUMER_SECRET,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                RequestUrl = Constants.USOSAPI_REQUEST_URL,
                CallbackUrl = Constants.PROTOCOL_URI,
                Type = OAuthRequestType.RequestToken,
            };
        }

        public async Task AuthorizeAsync()
        {
            var scopes = String.Join(separator: "%7C", values: usosScopes);
            var authString = request.GetAuthorizationQuery(new NameValueCollection()
            {
                {"scopes", scopes}
            });
            var requestUri = new Uri(uriString: request.RequestUrl + "?" + authString + "&scopes=" + scopes);
            await Windows.System.Launcher.LaunchUriAsync(uri: requestUri);

        }
    }
}
