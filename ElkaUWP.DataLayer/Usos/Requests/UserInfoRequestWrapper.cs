using System.Collections.Generic;
using System.Collections.Specialized;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Services;
using NLog;
using OAuthClient;

namespace ElkaUWP.DataLayer.Usos.Requests
{
    public class UserInfoRequestWrapper : OAuthProtectedResourceRequestWrapperBase
    {

        private const string _destination = "users/user";

        private readonly IReadOnlyCollection<string> _fields = new List<string>()
        {
            "id",
            "first_name",
            "middle_names",
            "last_name",
            "sex",
            "email",
            "photo_urls[200x250]",
            "student_number",
            "pesel",
            "student_status",
            "postal_addresses"
        };

        public UserInfoRequestWrapper(SecretService secretServiceInstance) : base(secretServiceInstance: secretServiceInstance)
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

        public string GetRequestString()
        {
            var fieldsString = string.Join(separator: "%7C", values: _fields);
            var additionalParameters = new NameValueCollection()
            {
                { "fields", fieldsString }
            };

            return $"{UnderlyingOAuthRequest.RequestUrl}?" + UnderlyingOAuthRequest.GetAuthorizationQuery(parameters: additionalParameters);
        }
    }

}
