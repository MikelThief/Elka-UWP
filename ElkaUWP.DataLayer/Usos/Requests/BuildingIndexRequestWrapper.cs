using System.Collections.Generic;
using System.Collections.Specialized;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Abstractions.Bases;
using ElkaUWP.Infrastructure.Services;
using NLog;
using OAuthClient;

namespace ElkaUWP.DataLayer.Usos.Requests
{
    /// <summary>
    /// Wraps https://apps.usos.pw.edu.pl/developers/api/services/geo/#building_index
    /// </summary>
    public class BuildingIndexRequestWrapper : OAuthProtectedResourceRequestWrapperBase
    {
        private const string _destination = "geo/building_index";
        // Fields index to be received in response
        private readonly IReadOnlyCollection<string> _fields = new List<string>()
        {
            "id",
            "name",
            "profile_url",
            "campus_name",
            "location",
            "marker_style",
            "phone_numbers"
        };

        private OAuthRequest UnderlyingOAuthRequest;

        public BuildingIndexRequestWrapper(SecretService secretServiceInstance, ILogger logger) : base(secretServiceInstance: secretServiceInstance, logger: logger)
        {
            Logger = logger;
            secretService = secretServiceInstance;

            var oAuthSecret = secretService.GetSecret(container: Constants.USOS_CREDENTIAL_CONTAINER_NAME,
                key: Constants.USOSAPI_ACCESS_TOKEN_KEY);
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

        public override string GetRequestString()
        {
            var additionalParameters = new NameValueCollection();
            var fieldsString = string.Join(separator: "%7C", values: _fields);
            additionalParameters.Add(name: "fields", value: fieldsString);

            return UnderlyingOAuthRequest.GetAuthorizationQuery(parameters: additionalParameters);
        }

    }
}
