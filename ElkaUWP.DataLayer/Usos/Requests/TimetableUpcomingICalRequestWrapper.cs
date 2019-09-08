using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Services;
using NLog;
using OAuthClient;

namespace ElkaUWP.DataLayer.Usos.Requests
{
    /// <summary>
    /// Wraps https://apps.usos.pw.edu.pl/developers/api/services/tt/#upcoming_ical
    /// </summary>
    public class TimetableUpcomingICalRequestWrapper : OAuthProtectedResourceRequestWrapperBase
    {
        private const string _destination = "tt/upcoming_ical";

        public  string GetRequestString()
        {
            var language = CultureInfo.CurrentCulture.ToString() == "pl-PL" ? "pl" : "en";

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            var userId = localSettings.Values[key: Constants.USOSAPI_USER_ID_SETTING_NAME].ToString();

            var additionalParameters = new NameValueCollection()
            {
                { "lang", language },
                { "user_id", userId }
            };

            return $"{UnderlyingOAuthRequest.RequestUrl}?" + UnderlyingOAuthRequest.GetAuthorizationQuery(parameters: additionalParameters);
        }

        public TimetableUpcomingICalRequestWrapper(SecretService secretServiceInstance) : base(secretServiceInstance: secretServiceInstance)
        {
            var oAuthSecret = SecretService.GetSecret(container: Constants.USOS_CREDENTIAL_CONTAINER_NAME);
            oAuthSecret.RetrievePassword();

            UnderlyingOAuthRequest = new OAuthRequest
            {
                ConsumerKey = Secrets.USOS_CONSUMER_KEY,
                ConsumerSecret = Secrets.USOS_CONSUMER_SECRET,
                Token = oAuthSecret.UserName,
                TokenSecret = oAuthSecret.Password,
                RequestUrl = Constants.USOSAPI_SECURE_BASE_URL + _destination,
                Method = "GET",
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                SignatureTreatment = OAuthSignatureTreatment.Escaped,
                Type = OAuthRequestType.ProtectedResource
            };
        }
    }
}
