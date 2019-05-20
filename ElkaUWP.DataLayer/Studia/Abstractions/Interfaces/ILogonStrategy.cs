using System.Net;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.DataLayer.Studia.ResolverParameters;

namespace ElkaUWP.DataLayer.Studia.Abstractions.Interfaces
{
    public interface ILogonStrategy
    {
        /// <summary>
        /// Name of the strategy used by <see cref="ILogonStrategyResolver"/>
        /// </summary>
        LogonStrategies Name { get; }

        /// <summary>
        /// Asynchronously retrieves Studia server session cookie.
        /// </summary>
        /// <param name="username">Username to the service</param>
        /// <param name="password">Password to the service</param>
        /// <remarks>Fills app-wide <see cref="FlurlClient"/> with proper authentication information.
        /// Hence there is no returning value.</remarks>
        Task InitializeAsync(LogonStrategyParametersContainer parametersContainer);
    }
}
