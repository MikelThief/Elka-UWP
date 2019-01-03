using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace ElkaUWP.Infrastructure.Abstractions.Interfaces
{
    public interface IUsosOAuthService
    {
        Task StartAuthorizationAsync();
        Task<PasswordCredential> GetAccessAsync(string authorizedRequestToken, string oauthVerifier);
    }
}
