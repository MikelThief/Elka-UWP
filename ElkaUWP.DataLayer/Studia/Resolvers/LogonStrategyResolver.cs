using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Enums;
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
        public ILogonStrategy Resolve(LogonStrategies strategy)
        {
            if (!Enum.IsDefined(enumType: typeof(LogonStrategies), value: strategy))
                throw new InvalidEnumArgumentException(argumentName: nameof(strategy),
                    invalidValue: (int) strategy, enumClass: typeof(LogonStrategies));

            return _logonStrategies.Single(x => x.Name == strategy);
        }
    }
}
