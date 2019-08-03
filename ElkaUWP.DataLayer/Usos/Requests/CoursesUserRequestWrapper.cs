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
    /// <summary>
    /// Wraps https://apps.usos.pw.edu.pl/developers/api/services/examrep/exam
    /// </summary>
    public class CoursesUserRequestWrapper : OAuthProtectedResourceRequestWrapperBase
    {
        private const string _destination = "courses/user";

        private readonly IReadOnlyCollection<string> _fields = new List<string>()
        {
            "course_editions[course_id|course_name|coordinators|lecturers|profile_url|term_id|passing_status]"
        };

        /// <inheritdoc />
        public CoursesUserRequestWrapper(SecretService secretServiceInstance) : base(secretServiceInstance)
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

        /// <inheritdoc />
        public string GetRequestString()
        {
            var fieldsString = string.Join(separator: "%7C", values: _fields);
            var additionalParameters = new NameValueCollection()
            {
                { "fields", fieldsString },
                { "active_terms_only", false.ToString().ToLower() }
            };

            return $"{UnderlyingOAuthRequest.RequestUrl}?" + UnderlyingOAuthRequest.GetAuthorizationQuery(parameters: additionalParameters);
        }
    }
}
