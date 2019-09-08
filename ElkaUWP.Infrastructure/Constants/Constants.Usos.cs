namespace ElkaUWP.Infrastructure
{
    public static partial class Constants
    {
        public const string USOSAPI_SECURE_BASE_URL = "https://apps.usos.pw.edu.pl/services/";

        public const string USOSAPI_REQUEST_URL = USOSAPI_SECURE_BASE_URL + "oauth/request_token";

        public const string USOSAPI_AUTHORIZE_URL = USOSAPI_SECURE_BASE_URL + "oauth/authorize";

        public const string USOSAPI_ACCESS_TOKEN_URL = USOSAPI_SECURE_BASE_URL + "oauth/access_token";

        public const string USOSAPI_ACCESS_TOKEN_KEY = "UsosApiAccessTokenKey";

        public const string USOSAPI_USER_ID_SETTING_NAME = "UsosApiUserIdSetting";
    }
}
