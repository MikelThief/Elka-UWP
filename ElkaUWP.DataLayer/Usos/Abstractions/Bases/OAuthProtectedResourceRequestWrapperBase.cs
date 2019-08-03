using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Services;
using NLog;
using OAuthClient;

namespace ElkaUWP.DataLayer.Usos.Abstractions.Bases
{
    public abstract class OAuthProtectedResourceRequestWrapperBase
    {
        protected OAuthRequest UnderlyingOAuthRequest;
        protected SecretService SecretService { get; set; }

        protected OAuthProtectedResourceRequestWrapperBase(SecretService secretServiceInstance)
        {
            SecretService = secretServiceInstance;
        }
    }
}
