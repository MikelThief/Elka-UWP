﻿using System.Collections.Generic;
using System.Collections.Specialized;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.Infrastructure;
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

        public BuildingIndexRequestWrapper(SecretService secretServiceInstance) : base(secretServiceInstance: secretServiceInstance)
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
