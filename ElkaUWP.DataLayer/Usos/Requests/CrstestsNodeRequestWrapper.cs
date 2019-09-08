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
    public class CrstestsNodeRequestWrapper : OAuthProtectedResourceRequestWrapperBase
    {
        private const string _destination = "crstests/node";

        // Fields index to be received in response
        private readonly IReadOnlyCollection<string> _fields = new List<string>()
        {
            "node_id",
            "parent_id",
            "name",
            "points_min",
            "points_max",
            "description",
            "visible_for_students",
            "root_id",
            "course_edition",
            "type",
            "subnodes",
            "order"
        };

        /// <inheritdoc />
        public CrstestsNodeRequestWrapper(SecretService secretServiceInstance) : base(secretServiceInstance)
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

        public string GetRequestString(int nodeId)
        {
            var fieldsString = string.Join(separator: "%7C", values: _fields);
            var additionalParameters = new NameValueCollection()
            {
                { "fields", fieldsString },
                { "recursive", true.ToString().ToLower() },
                { "node_id", nodeId.ToString() }
            };

            return $"{UnderlyingOAuthRequest.RequestUrl}?" + UnderlyingOAuthRequest.GetAuthorizationQuery(parameters: additionalParameters);
        }
    }

}
