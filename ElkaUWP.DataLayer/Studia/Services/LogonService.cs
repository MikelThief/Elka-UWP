using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using Windows.Web.Http;
using System.Threading.Tasks;
using Anotar.NLog;
using ElkaUWP.DataLayer.Studia.Abstractions.Bases;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Resolvers;
using ElkaUWP.Infrastructure.Services;
using Flurl.Http;

namespace ElkaUWP.DataLayer.Studia.Services
{
    public class LogonService : StudiaServiceBase
    {

        private readonly IGradesProxy _proxy;

        // possible login data fields
        private string _username;
        private string _password;

        /// <inheritdoc />
        public LogonService(IGradesProxy proxy, SecretService secretService) : base(secretService: secretService)
        {
            _proxy = proxy;
        }

        public async Task<bool> ValidateCredentials()
        {
            try
            {
                _secretService.CreateOrUpdateSecret(container: Constants.STUDIA_RESOURCE_TOKEN, key: _username,
                    secret: _password);
                await _proxy.Authenticate().ConfigureAwait(false);
                return true;
            }
            catch (FlurlHttpException fhexc)
            {
                LogTo.FatalException(message: "Failed to authenticate user against Studia", exception: fhexc);
                _secretService.RemoveSecret(container: Constants.STUDIA_RESOURCE_TOKEN, key: _username);
            }
            catch (InvalidOperationException iopexc)
            {
                LogTo.FatalException(message: "Studia server could have changed authentication workflow",
                    exception: iopexc);
                _secretService.RemoveSecret(container: Constants.STUDIA_RESOURCE_TOKEN, key: _username);
            }
            return false;
        }

        public void ProvideUsernameAndPassword(string username, string password)
        {
            _username = username;
            _password = password;
        }
    }
}
