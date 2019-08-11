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
    public class UsersUserRequestWrapper : OAuthProtectedResourceRequestWrapperBase
    {
        private const string _destination = "users/user";

        // Fields index to be received in response
        // API may return more fields
        private readonly IReadOnlyCollection<string> _commonFields = new List<string>()
        {
            "id",
            "first_name",
            "middle_names",
            "last_name",
            "email_access",
            "email",
            "homepage_url",
            "profile_url",
            "has_photo",
            "photo_urls[400x400]"
        };

        private readonly IReadOnlyCollection<string> _staffFields = new List<string>()
        {
            "titles",
            "staff_status",
            "office_hours",
            "employment_positions",
            "course_editions_conducted",
            "staff_status",
            "phone_numbers",
            "mobile_numbers"
        };

        private readonly IReadOnlyCollection<string> _studentFields = new List<string>()
        {
            "student_status",
            "student_number",
            "student_programmes"
        };

        /// <inheritdoc />
        public UsersUserRequestWrapper(SecretService secretServiceInstance) : base(secretServiceInstance)
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

        public string GetRequestString(int userId, bool includeStaffProperties, bool includeStudentProperties)
        {
            var fields = new List<string>(collection: _commonFields);

            if (includeStaffProperties)
            {
                fields.AddRange(collection: _staffFields);
            }

            if (includeStudentProperties)
            {
                fields.AddRange(collection: _studentFields);
            }


            var fieldsString = string.Join(separator: "%7C", values: fields);

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
