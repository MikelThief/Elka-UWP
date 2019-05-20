using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.DataLayer.Studia.Exceptions;
using ElkaUWP.DataLayer.Studia.ResolverParameters;
using ElkaUWP.DataLayer.Studia.Strategies;

namespace ElkaUWP.DataLayer.Studia.Resolvers
{
    public class LogonStrategyResolver : ILogonStrategyResolver
    {
        private IEnumerable<ILogonStrategy> _logonStrategies;

        public LogonStrategyResolver(IEnumerable<ILogonStrategy> logonStrategies)
        {
            _logonStrategies = logonStrategies;
        }

        /// <inheritdoc />
        public ILogonStrategy Resolve(LogonStrategyParametersContainer container)
        {
            // more logic should come here if there will be more logon strategies

            // logic for LdapOnly
            switch (container.PreferredStrategy)
            {
                case LogonStrategies.StudiaForm:
                {
                    if (string.IsNullOrEmpty(value: container.Username) || string.IsNullOrEmpty(value: container.Password))
                        throw new LogonStrategyResolutionFailedException(
                                message: $"{nameof(LogonStrategyResolver)} was unable to resolve a strategy for a set of data.",
                                parametersContainer: container);
                    var matchingStrategy = _logonStrategies.Single(x => x.Name == LogonStrategies.StudiaForm);
                    matchingStrategy.InitializeAsync(parametersContainer: container);
                    return matchingStrategy;
                }

                case LogonStrategies.Unspecified:
                default:
                    throw new LogonStrategyResolutionFailedException(
                        message: $"{nameof(LogonStrategyResolver)} was unable to resolve a strategy for a set of data.",
                        parametersContainer: container);

            }
        }
    }
}