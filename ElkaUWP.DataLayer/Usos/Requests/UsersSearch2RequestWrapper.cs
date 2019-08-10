using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Services;
using OAuthClient;

namespace ElkaUWP.DataLayer.Usos.Requests
{
    public class UsersSearch2RequestWrapper : OAuthProtectedResourceRequestWrapperBase
    {
        private const string _destination = "users/search2";

        // Fields index to be received in response
        // API may return more fields
        private readonly IReadOnlyCollection<string> _fields = new List<string>()
        {
            "next_page",
            "items[user[id|titles|employment_functions|first_name|last_name]|match]"
        };

        /// <inheritdoc />
        public UsersSearch2RequestWrapper(SecretService secretServiceInstance) : base(secretServiceInstance)
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


        public string GetRequestString(string query, UserCategory userCategory, string lang)
        {
            var fieldsString = string.Join(separator: "%7C", values: _fields);

            string among;

            switch (userCategory)
            {
                case UserCategory.All:
                    among = "all";
                    break;
                case UserCategory.Students:
                    among = "students";
                    break;
                case UserCategory.CurrentStudents:
                    among = "current_students";
                    break;
                case UserCategory.Staff:
                    among = "staff";
                    break;
                case UserCategory.CurrentStaff:
                    among = "current_staff";
                    break;
                case UserCategory.CurrentTeachers:
                    among = "current_teachers";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(paramName: nameof(userCategory));
            }

            var additionalParameters = new NameValueCollection()
            {
                { "fields", fieldsString },
                { "lang", lang },
                { "num", 8.ToString() },
                { "among", among },
                { "query", query }
            };

            return $"{UnderlyingOAuthRequest.RequestUrl}?" +
                   UnderlyingOAuthRequest.GetAuthorizationQuery(parameters: additionalParameters);
        }


    }

    public enum UserCategory : short
    {
        All = 0, // all the users
        Students = 1, // users which are - or once have been - students.
                      // This includes ex-students and graduates

        CurrentStudents = 2, // users which currently are students.
                             // A "current student" is one that is currently
                             // studying in at at least one study programme

        Staff = 3, // users which are - or once have been - staff members.
                   // This includes all staff members in the USOS database,
                   // employed presently or in the past

        CurrentStaff = 4, // users which currently are staff members.
                          // This does not include staff members employed in the past

        CurrentTeachers = 5 // staff members which are currently teaching.
    }
}
