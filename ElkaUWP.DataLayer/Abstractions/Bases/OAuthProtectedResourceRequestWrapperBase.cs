using ElkaUWP.DataLayer.Abstractions.Interfaces;
using ElkaUWP.Infrastructure.Services;
using NLog;
using OAuthClient;

namespace ElkaUWP.Infrastructure.Abstractions.Bases
{
    public abstract class OAuthProtectedResourceRequestWrapperBase : IOAuthProtectedResourceRequestWrapper

    {
        protected OAuthRequest UnderlyingOAuthRequest;
        protected SecretService SecretService { get; set; }
        protected ILogger Logger { get; set; }

        protected OAuthProtectedResourceRequestWrapperBase(SecretService secretServiceInstance, ILogger logger)
        {
            Logger = logger;
            SecretService = secretServiceInstance;
        }

        /// <inheritdoc />
        public abstract string GetRequestString();
    }
}
