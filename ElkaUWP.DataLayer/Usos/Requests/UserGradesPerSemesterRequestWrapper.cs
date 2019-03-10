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
    public class UserGradesPerSemesterRequestWrapper : OAuthProtectedResourceRequestWrapperBase
    {
        private const string _destination = "grades/terms2";

        private readonly IReadOnlyCollection<string> _fields = new List<string>()
        {
            "value_symbol",
            "passes",
            "exam_session_number",
            "counts_into_average"
        };

        private IReadOnlyCollection<string> _semesterIds = new List<string>
        {
            "2010L",
            "2010Z",
            "2011L",
            "2011Z",
            "2012L",
            "2013Z",
            "2013L",
            "2014Z",
            "2014L",
            "2015Z",
            "2015L",
            "2016Z",
            "2016L",
            "2017Z",
            "2017L",
            "2018Z",
            "2018L",
            "2019Z",
            "2020L",
            "2020Z",
            "2021L",
            "2021Z",
            "2022L",
            "2022Z",
            "2023L",
            "2023Z",
            "2024L",
            "2024Z",
            "2025L",
            "2025Z",
            "2026L",
            "2026Z",
            "2027L",
            "2027Z",
            "2028L",
            "2028Z",
            "2029L",
            "2029Z",
            "2030L",
            "2030Z",
        };

        public UserGradesPerSemesterRequestWrapper(SecretService secretServiceInstance, ILogger logger) : base(secretServiceInstance, logger)
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

        //public string GetRequestString(params string[] semesterIds)
        //{
        //    if (semesterIds is null)
        //        return GetRequestString();

        //    var fieldsString = string.Join(separator: "%7C", values: _fields);
        //    var semesterIdsString = string.Join(separator: "%7C", values: semesterIds.ToList());
        //    var additionalParameters = new NameValueCollection()
        //    {
        //        { "term_ids", semesterIdsString },
        //        { "fields", fieldsString }
        //    };

        //    return $"{UnderlyingOAuthRequest.RequestUrl}?" + UnderlyingOAuthRequest.GetAuthorizationQuery(parameters: additionalParameters);
        //}

        public override string GetRequestString()
        {
            var fieldsString = string.Join(separator: "%7C", values: _fields);
            var semesterIdsString = string.Join(separator: "%7C", values: _semesterIds);
            var additionalParameters = new NameValueCollection()
            {
                { "term_ids", semesterIdsString },
                { "fields", fieldsString }
            };

            return $"{UnderlyingOAuthRequest.RequestUrl}?" + UnderlyingOAuthRequest.GetAuthorizationQuery(parameters: additionalParameters);
        }
    }
}
