using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace ElkaUWP.DataLayer.Studia.Abstractions.Interfaces
{
    public interface ILogonStrategy
    {
        /// <summary>
        /// Performs authentication of the strategy using supplied credential data in the system.
        /// </summary>
        Task AuthenticateAsync(PasswordCredential credential);

        /// <summary>
        /// Allows to check whether strategy is currently able to make requests
        /// without re-authentication.
        /// </summary>
        /// <returns>Yes if authenticated. No otherwise.</returns>
        bool IsAuthenticated();
    }
}
