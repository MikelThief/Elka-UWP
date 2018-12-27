using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.Infrastructure.Interfaces
{
    public interface IUsosOAuthService
    {
        Task StartAuthorizationAsync();
        Task GetAccessAsync(string authorizedRequestToken, string oauthVerifier);
    }
}
