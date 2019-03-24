using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
    public class CrstestsUserPointsRequestWrapper : OAuthProtectedResourceRequestWrapperBase
    {
        private const string _destination = "crstests/user_points";

        /// <inheritdoc />
        public CrstestsUserPointsRequestWrapper(SecretService secretServiceInstance, ILogger logger) : base(secretServiceInstance, logger)
        {
            var oAuthSecret = SecretService.GetSecret(container: Constants.USOS_CREDENTIAL_CONTAINER_NAME,
                key: Windows.Storage.ApplicationData.Current.LocalSettings.Values[key: Constants.USOSAPI_ACCESS_TOKEN_KEY].ToString());
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
            throw new InvalidOperationException("Not supported by USOS API. Call requires at least one node id.");
        }

        public string GetRequestString(params int[] nodeIds)
        {
            var nodeIdsString = string.Join(separator: "%7C", values: nodeIds.ToList());
            var additionalParameters = new NameValueCollection()
            {
                { "node_ids", nodeIdsString }
            };

            return $"{UnderlyingOAuthRequest.RequestUrl}?" + UnderlyingOAuthRequest.GetAuthorizationQuery(parameters: additionalParameters);
        }

        public string GetRequestString(List<int> nodeIds)
        {
            var nodeIdsString = string.Join(separator: "%7C", values: nodeIds.ToList());
            var additionalParameters = new NameValueCollection()
            {
                { "node_ids", nodeIdsString }
            };

            return $"{UnderlyingOAuthRequest.RequestUrl}?" + UnderlyingOAuthRequest.GetAuthorizationQuery(parameters: additionalParameters);
        }
    }
}
