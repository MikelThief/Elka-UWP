using ElkaUWP.Infrastructure.Services;
using NLog;

namespace ElkaUWP.Infrastructure.Abstractions.Bases
{
    public abstract class OAuthProtectedResourceRequestWrapperBase
    {
        protected SecretService secretService { get; set; }
        protected ILogger Logger { get; set; }

        protected OAuthProtectedResourceRequestWrapperBase(SecretService secretServiceInstance, ILogger logger)
        {
            Logger = logger;
            secretService = secretServiceInstance;
        }
    }
}
