using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.Infrastructure;
using OAuth;

namespace ElkaUWP.LoginModule.Service
{
    public class UsosOAuthService
    {
        public UsosOAuthService()
        {
            var req = new OAuthRequest()
            {
                Method = "GET",
                Version = "1.0",
                ConsumerKey = Constants.ELKA_CUSTOMERKEY_FOR_USOS,
                ConsumerSecret = Constants.ELKA_CUSTOMERSECRET_FOR_USOS,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                RequestUrl = "https://" + Constants.USOSAPI_BASE_URL,
            };

            string auth = req.GetAuthorizationQuery();
        }
    }
}
