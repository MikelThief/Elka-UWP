using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Web.Http;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Enums;

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
            var resolvedLogonStrategy = _logonStrategyResolver.Resolve(strategy: logonStrategy);
            try
            {
                await resolvedLogonStrategy.GetSessionCookieAsync(username: username, password: password);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
