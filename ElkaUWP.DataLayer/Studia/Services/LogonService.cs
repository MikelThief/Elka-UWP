using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Windows.Web.Http;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.DataLayer.Studia.ResolverParameters;

namespace ElkaUWP.DataLayer.Studia.Services
{
    public class LogonService
    {
        private readonly ILogonStrategyResolver _logonStrategyResolver;

        public LogonService(ILogonStrategyResolver logonStrategyResolver)
        {
            _logonStrategyResolver = logonStrategyResolver;
        }

        public async Task<bool> CheckIfCorrectLogin(LogonStrategies logonStrategy, string username, string password)
        {
            var container = new LogonStrategyParametersContainer(preferredStrategy: logonStrategy, username: username,
                password: password);

            var resolvedLogonStrategy = _logonStrategyResolver.Resolve(container: container);
            try
            {
                
                return true;
            }
            catch (CookieException ex)
            {
                return false;
            }
        }
    }
}
