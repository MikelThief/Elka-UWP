using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace OAuthClient
{
    /// <summary>
    /// A request wrapper for the OAuth 1.0a specification.
    /// </summary>
    /// <seealso href="http://oauth.net/"/>
    public class OAuthRequest
    {
        public virtual OAuthSignatureMethod SignatureMethod { get; set; }
        public virtual OAuthSignatureTreatment SignatureTreatment { get; set; }
        public virtual OAuthRequestType Type { get; set; }

        public virtual string Method { get; set; }
        public virtual string Realm { get; set; }
        public virtual string ConsumerKey { get; set; }
        public virtual string ConsumerSecret { get; set; }
        public virtual string Token { get; set; }
        public virtual string TokenSecret { get; set; }
        public virtual string Verifier { get; set; }
        public virtual string ClientUsername { get; set; }
        public virtual string ClientPassword { get; set; }
        public virtual string CallbackUrl { get; set; }
        public virtual string Version { get; set; }
        public virtual string SessionHandle { get; set; }

        /// <seealso cref="http://oauth.net/core/1.0#request_urls"/>
        public virtual string RequestUrl { get; set; }

        #region Authorization Header

#if !WINRT
        public string GetAuthorizationHeader(NameValueCollection parameters)
        {
            var collection = new WebParameterCollection(collection: parameters);

            return GetAuthorizationHeader(parameters: collection);
        }
#endif

        public string GetAuthorizationHeader(IDictionary<string, string> parameters)
        {
            var collection = new WebParameterCollection(collection: parameters);

            return GetAuthorizationHeader(parameters: collection);
        }

        public string GetAuthorizationHeader()
        {
            var collection = new WebParameterCollection(0);

            return GetAuthorizationHeader(parameters: collection);
        }

        public string GetAuthorizationHeader(WebParameterCollection parameters)
        {
            switch (Type)
            {
                case OAuthRequestType.RequestToken:
                    ValidateRequestState();
                    return GetSignatureAuthorizationHeader(parameters: parameters);
                case OAuthRequestType.AccessToken:
                    ValidateAccessRequestState();
                    return GetSignatureAuthorizationHeader(parameters: parameters);
                case OAuthRequestType.ProtectedResource:
                    ValidateProtectedResourceState();
                    return GetSignatureAuthorizationHeader(parameters: parameters);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetSignatureAuthorizationHeader(WebParameterCollection parameters)
        {
            var signature = GetNewSignature(parameters: parameters);

            parameters.Add("oauth_signature", value: signature);

            return WriteAuthorizationHeader(parameters: parameters);
        }

        private string WriteAuthorizationHeader(WebParameterCollection parameters)
        {
            var sb = new StringBuilder("OAuth ");

            if (!IsNullOrBlank(value: Realm))
            {
                sb.AppendFormat("realm=\"{0}\",", arg0: OAuthTools.UrlEncodeRelaxed(value: Realm));
            }

            parameters.Sort((l, r) => string.Compare(strA: l.Name, strB: r.Name, comparisonType: StringComparison.Ordinal));

            foreach (var parameter in parameters.Where(parameter =>
                !IsNullOrBlank(value: parameter.Name) &&
                !IsNullOrBlank(value: parameter.Value) &&
                (parameter.Name.StartsWith("oauth_") || parameter.Name.StartsWith("x_auth_"))))
            {
                sb.AppendFormat("{0}=\"{1}\",", arg0: parameter.Name, arg1: parameter.Value);
            }

            sb.Remove(startIndex: sb.Length - 1, length: 1);

            var authorization = sb.ToString();
            return authorization;
        }

        #endregion

        #region Authorization Query

#if !WINRT
        public string GetAuthorizationQuery(NameValueCollection parameters)
        {
            var collection = new WebParameterCollection(collection: parameters);

            return GetAuthorizationQuery(parameters: collection);
        }
#endif

        public string GetAuthorizationQuery(IDictionary<string, string> parameters)
        {
            var collection = new WebParameterCollection(collection: parameters);

            return GetAuthorizationQuery(parameters: collection);
        }

        public string GetAuthorizationQuery()
        {
            var collection = new WebParameterCollection(capacity: 0);

            return GetAuthorizationQuery(parameters: collection);
        }

        private string GetAuthorizationQuery(WebParameterCollection parameters)
        {
            switch (Type)
            {
                case OAuthRequestType.RequestToken:
                    ValidateRequestState();
                    return GetSignatureAuthorizationQuery(parameters: parameters);
                case OAuthRequestType.AccessToken:
                    ValidateAccessRequestState();
                    return GetSignatureAuthorizationQuery(parameters: parameters);
                case OAuthRequestType.ProtectedResource:
                    ValidateProtectedResourceState();
                    return GetSignatureAuthorizationQuery(parameters: parameters);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetSignatureAuthorizationQuery(WebParameterCollection parameters)
        {
            var signature = GetNewSignature(parameters: parameters);

            parameters.Add("oauth_signature", value: signature);

            return WriteAuthorizationQuery(parameters: parameters);
        }

        private static string WriteAuthorizationQuery(WebParameterCollection parameters)
        {
            var sb = new StringBuilder();

            parameters.Sort(comparison: (l, r) => string.Compare(strA: l.Name, strB: r.Name, comparisonType: StringComparison.Ordinal));

            var count = 0;

            foreach (var parameter in parameters.Where(parameter =>
                !IsNullOrBlank(value: parameter.Name) &&
                !IsNullOrBlank(value: parameter.Value)))
            {
                count++;
                var format = count < parameters.Count ? "{0}={1}&" : "{0}={1}";
                sb.AppendFormat(format: format, arg0: parameter.Name, arg1: parameter.Value);
            }

            var authorization = sb.ToString();
            return authorization;
        }

        #endregion

        private string GetNewSignature(WebParameterCollection parameters)
        {
            var timestamp = OAuthTools.GetTimestamp();

            var nonce = OAuthTools.GetNonce();

            AddAuthParameters(parameters: parameters, timestamp: timestamp, nonce: nonce);

            var signatureBase =
                OAuthTools.ConcatenateRequestElements(method: Method.ToUpperInvariant(), url: RequestUrl, parameters: parameters);

            var signature = OAuthTools.GetSignature(signatureMethod: SignatureMethod, signatureTreatment: SignatureTreatment, signatureBase: signatureBase, consumerSecret: ConsumerSecret,
                tokenSecret: TokenSecret);

            return signature;
        }

        #region Static Helpers

        public static OAuthRequest ForRequestToken(string consumerKey, string consumerSecret)
        {
            var credentials = new OAuthRequest
            {
                Method = "GET",
                Type = OAuthRequestType.RequestToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                SignatureTreatment = OAuthSignatureTreatment.Escaped,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret
            };
            return credentials;
        }

        public static OAuthRequest ForRequestToken(string consumerKey, string consumerSecret, string callbackUrl)
        {
            var credentials = ForRequestToken(consumerKey: consumerKey, consumerSecret: consumerSecret);
            credentials.CallbackUrl = callbackUrl;
            return credentials;
        }

        public static OAuthRequest ForAccessToken(string consumerKey, string consumerSecret, string requestToken,
            string requestTokenSecret)
        {
            var credentials = new OAuthRequest
            {
                Method = "GET",
                Type = OAuthRequestType.AccessToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                SignatureTreatment = OAuthSignatureTreatment.Escaped,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                Token = requestToken,
                TokenSecret = requestTokenSecret
            };
            return credentials;
        }

        public static OAuthRequest ForAccessToken(string consumerKey, string consumerSecret, string requestToken,
            string requestTokenSecret, string verifier)
        {
            var credentials = ForAccessToken(consumerKey: consumerKey, consumerSecret: consumerSecret, requestToken: requestToken, requestTokenSecret: requestTokenSecret);
            credentials.Verifier = verifier;
            return credentials;
        }

        public static OAuthRequest ForAccessTokenRefresh(string consumerKey, string consumerSecret, string accessToken,
            string accessTokenSecret, string sessionHandle)
        {
            var credentials = ForAccessToken(consumerKey: consumerKey, consumerSecret: consumerSecret, requestToken: accessToken, requestTokenSecret: accessTokenSecret);
            credentials.SessionHandle = sessionHandle;
            return credentials;
        }

        public static OAuthRequest ForAccessTokenRefresh(string consumerKey, string consumerSecret, string accessToken,
            string accessTokenSecret, string sessionHandle, string verifier)
        {
            var credentials = ForAccessToken(consumerKey: consumerKey, consumerSecret: consumerSecret, requestToken: accessToken, requestTokenSecret: accessTokenSecret);
            credentials.SessionHandle = sessionHandle;
            credentials.Verifier = verifier;
            return credentials;
        }

        public static OAuthRequest ForProtectedResource(string method, string consumerKey, string consumerSecret,
            string accessToken, string accessTokenSecret)
        {
            var credentials = new OAuthRequest
            {
                Method = method ?? "GET",
                Type = OAuthRequestType.ProtectedResource,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                SignatureTreatment = OAuthSignatureTreatment.Escaped,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                Token = accessToken,
                TokenSecret = accessTokenSecret
            };
            return credentials;
        }

        #endregion

        private void ValidateRequestState()
        {
            if (IsNullOrBlank(value: Method))
            {
                throw new ArgumentException("You must specify a HTTP method");
            }

            if (IsNullOrBlank(value: RequestUrl))
            {
                throw new ArgumentException("You must specify a request token URL");
            }

            if (IsNullOrBlank(value: ConsumerKey))
            {
                throw new ArgumentException("You must specify a consumer key");
            }

            if (IsNullOrBlank(value: ConsumerSecret))
            {
                throw new ArgumentException("You must specify a consumer secret");
            }
        }

        private void ValidateAccessRequestState()
        {
            if (IsNullOrBlank(value: Method))
            {
                throw new ArgumentException("You must specify a HTTP method");
            }

            if (IsNullOrBlank(value: RequestUrl))
            {
                throw new ArgumentException("You must specify an access token URL");
            }

            if (IsNullOrBlank(value: ConsumerKey))
            {
                throw new ArgumentException("You must specify a consumer key");
            }

            if (IsNullOrBlank(value: ConsumerSecret))
            {
                throw new ArgumentException("You must specify a consumer secret");
            }

            if (IsNullOrBlank(value: Token))
            {
                throw new ArgumentException("You must specify a token");
            }
        }

        private void ValidateProtectedResourceState()
        {
            if (IsNullOrBlank(value: Method))
            {
                throw new ArgumentException("You must specify an HTTP method");
            }

            if (IsNullOrBlank(value: ConsumerKey))
            {
                throw new ArgumentException("You must specify a consumer key");
            }

            if (IsNullOrBlank(value: ConsumerSecret))
            {
                throw new ArgumentException("You must specify a consumer secret");
            }
        }

        private void AddAuthParameters(ICollection<WebParameter> parameters, string timestamp, string nonce)
        {
            var authParameters = new WebParameterCollection
            {
                new WebParameter("oauth_consumer_key", value: ConsumerKey),
                new WebParameter("oauth_nonce", value: nonce),
                new WebParameter("oauth_signature_method", value: ToRequestValue(signatureMethod: SignatureMethod)),
                new WebParameter("oauth_timestamp", value: timestamp),
                new WebParameter("oauth_version", value: Version ?? "1.0")
            };

            if (!IsNullOrBlank(value: Token))
            {
                authParameters.Add(parameter: new WebParameter("oauth_token", value: Token));
            }

            if (!IsNullOrBlank(value: CallbackUrl))
            {
                authParameters.Add(parameter: new WebParameter("oauth_callback", value: CallbackUrl));
            }

            if (!IsNullOrBlank(value: Verifier))
            {
                authParameters.Add(parameter: new WebParameter("oauth_verifier", value: Verifier));
            }

            if (!IsNullOrBlank(value: SessionHandle))
            {
                authParameters.Add(parameter: new WebParameter("oauth_session_handle", value: SessionHandle));
            }

            foreach (var authParameter in authParameters)
            {
                parameters.Add(item: authParameter);
            }
        }

        public static string ToRequestValue(OAuthSignatureMethod signatureMethod)
        {
            var value = signatureMethod.ToString().ToUpper();
            var shaIndex = value.IndexOf("SHA1", comparisonType: StringComparison.Ordinal);
            return shaIndex > -1 ? value.Insert(startIndex: shaIndex, value: "-") : value;
        }

        private static bool IsNullOrBlank(string value) => string.IsNullOrEmpty(value: value) || (!string.IsNullOrEmpty(value: value) && value.Trim() == string.Empty);
    }
}
