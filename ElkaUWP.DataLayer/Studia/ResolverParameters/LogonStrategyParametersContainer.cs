using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Enums;

namespace ElkaUWP.DataLayer.Studia.ResolverParameters
{
    [Serializable]
    public class LogonStrategyParametersContainer
    {
        public readonly LogonStrategies PreferredStrategy;
        public readonly string Username;
        public readonly string Password;

        public LogonStrategyParametersContainer(LogonStrategies preferredStrategy = LogonStrategies.Unspecified, string username = "", string password = "")
        {
            PreferredStrategy = preferredStrategy;
            Username = username;
            Password = password;
        }
    }
}
