using ElkaUWP.DataLayer.Abstractions.Interfaces;
using ElkaUWP.Infrastructure.Services;
using NLog;
using OAuthClient;

namespace ElkaUWP.Infrastructure.Abstractions.Bases
{
    public abstract class OAuthProtectedResourceRequestWrapperBase : IOAuthProtectedResourceRequestWrapper

    {
        protected OAuthRequest UnderlyingOAuthRequest;
        protected SecretService secretService { get; set; }
        protected ILogger Logger { get; set; }

        protected OAuthProtectedResourceRequestWrapperBase(SecretService secretServiceInstance, ILogger logger)
        {
            Logger = logger;
            secretService = secretServiceInstance;
        }

        /// <inheritdoc />
        public abstract string GetRequestString();
    }
}
