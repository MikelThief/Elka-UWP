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
    public class CrtestsStudentPointRequestWrapper : OAuthProtectedResourceRequestWrapperBase
    {
        private const string _destination = "crstests/student_point";

        // Fields index to be received in response
        private readonly IReadOnlyCollection<string> _fields = new List<string>()
        {
            "points",
            "grader",
            "comment",
            "last_changed"
        };

        /// <inheritdoc />
        public CrtestsStudentPointRequestWrapper(SecretService secretServiceInstance) : base(secretServiceInstance)
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

        public string GetRequestString(int nodeId)
        {
            var fieldsString = string.Join(separator: "%7C", values: _fields);

            var additionalParametersDict = new Dictionary<string, string>
            {
                {"fields", fieldsString},
                {"node_id", nodeId.ToString()}
            };

            var stringus = UnderlyingOAuthRequest.GetAuthorizationQuery(parameters: additionalParametersDict);

            Console.WriteLine(stringus);

            return $"{UnderlyingOAuthRequest.RequestUrl}?" +
                   UnderlyingOAuthRequest.GetAuthorizationQuery(parameters: additionalParametersDict);
        }
    }
}
