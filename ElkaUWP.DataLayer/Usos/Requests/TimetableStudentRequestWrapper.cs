using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Services;
using NLog;
using OAuthClient;

namespace ElkaUWP.DataLayer.Usos.Requests
{
    /// <summary>
    /// Wraps https://apps.usos.pw.edu.pl/developers/api/services/tt/#student
    /// </summary>
    public class TimetableStudentRequestWrapper : OAuthProtectedResourceRequestWrapperBase
    {
        private const string _destination = "tt/student";

        // Fields index to be received in response
        // API may return more fields
        private readonly IReadOnlyCollection<string> _fields = new List<string>()
        {
            "start_time",
            "end_time",
            "name",
            "url",
            "course_id",
            "course_name",
            "classtype_name",
            "group_number",
            "building_name",
            "building_id",
            "room_number",
            "room_id",
            "unit_id",
            "classtype_id",
            "cgwm_id"
        };

        public string GetRequestString(DateTime startDate)
        {
            var fieldsString = string.Join(separator: "%7C", values: _fields);
            var additionalParameters = new NameValueCollection()
            {
                { "fields", fieldsString },
                { "start", startDate.ToString(format: "yyyy-MM-dd") }
            };

            return $"{UnderlyingOAuthRequest.RequestUrl}?" + UnderlyingOAuthRequest.GetAuthorizationQuery(parameters: additionalParameters);
        }

        /// <inheritdoc />
        public TimetableStudentRequestWrapper(SecretService secretServiceInstance) : base(secretServiceInstance)
        {
            SecretService = secretServiceInstance;

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
    }
}
