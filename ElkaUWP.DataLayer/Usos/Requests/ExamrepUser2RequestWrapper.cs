using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public class ExamrepUser2RequestWrapper : OAuthProtectedResourceRequestWrapperBase
    {
        private const string _destination = "examrep/user2";

        private readonly IReadOnlyCollection<string> _fields = new List<string>()
        {
            "id|type_id|type_description|sessions[number|issuer_grades[value_symbol|counts_into_average|passes]]|grades_distribution"
        };

        /// <inheritdoc />
        public ExamrepUser2RequestWrapper(SecretService secretServiceInstance, ILogger logger) : base(secretServiceInstance, logger)
        {
            var oAuthSecret = SecretService.GetSecret(container: Constants.USOS_CREDENTIAL_CONTAINER_NAME,
                key: Windows.Storage.ApplicationData.Current.LocalSettings.Values[Constants.USOSAPI_ACCESS_TOKEN_KEY].ToString());
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

        /// <inheritdoc />
        public override string GetRequestString()
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
