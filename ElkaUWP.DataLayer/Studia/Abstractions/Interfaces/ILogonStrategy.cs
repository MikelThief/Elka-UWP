using System.Net;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Enums;

namespace ElkaUWP.DataLayer.Studia.Abstractions.Interfaces
{
    public interface ILogonStrategy
    {
        /// <summary>
        /// Name of the strategy used by <see cref="ILogonStrategyResolver"/>
        /// </summary>
        LogonStrategies Name { get; }

        /// <summary>
        /// Synchronously retrieves Studia server session cookie.
        /// </summary>
        /// <param name="username">Username to the service</param>
        /// <param name="password">Password to the service</param>
        /// <returns>Cookie which lifetime is 15 minutes if not reused</returns>
        Cookie GetSessionCookie(string username, string password);

        /// <summary>
        /// Asynchronously retrieves Studia server session cookie.
        /// </summary>
        /// <param name="username">Username to the service</param>
        /// <param name="password">Password to the service</param>
        /// <returns>Cookie which lifetime is 15 minutes if not reused</returns>
        Task<Cookie> GetSessionCookieAsync(string username, string password);
    }
}
