using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Services;
using OAuthClient;

namespace ElkaUWP.DataLayer.Usos.Requests
{
    public class UserStaffRequestWrapper : OAuthProtectedResourceRequestWrapperBase
    {
        private const string _destination = "users/user";

        // Fields index to be received in response
        // API may return more fields
        private readonly IReadOnlyCollection<string> _fields = new List<string>()
        {
            "first_name",
            "id",
            "last_name",
            "titles",
            "staff_status",
            "profile_url",
            "office_hours",
            "employment_positions",
            "course_editions_conducted"
        };

        /// <inheritdoc />
        public UserStaffRequestWrapper(SecretService secretServiceInstance) : base(secretServiceInstance)
        {
            var oAuthSecret = SecretService.GetSecret(container: Constants.USOS_CREDENTIAL_CONTAINER_NAME);
            oAuthSecret.RetrievePassword();

            UnderlyingOAuthRequest = new OAuthRequest
            {
                ConsumerKey = Constants.USOS_CONSUMER_KEY,
                ConsumerSecret = Constants.USOS_CONSUMER_SECRET,
                Token = oAuthSecret.UserName,
                TokenSecret = oAuthSecret.Password,
                RequestUrl = Constants.USOSAPI_SECURE_BASE_URL + _destination,
                Method = "GET",
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                SignatureTreatment = OAuthSignatureTreatment.Escaped,
                Type = OAuthRequestType.ProtectedResource
            };
        }

        public string GetRequestString(int userId)
        {
            var fieldsString = string.Join(separator: "%7C", values: _fields);

            var additionalParameters = new NameValueCollection()
            {
                {"fields", fieldsString},
                {"user_id", userId.ToString()},
            };

            return $"{UnderlyingOAuthRequest.RequestUrl}?" +
                   UnderlyingOAuthRequest.GetAuthorizationQuery(parameters: additionalParameters);
        }
    }
}
