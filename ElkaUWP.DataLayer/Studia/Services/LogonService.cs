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
using ElkaUWP.DataLayer.Studia.Resolvers;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Services;
using Flurl.Http;

namespace ElkaUWP.DataLayer.Studia.Services
{
    public class LogonService : StudiaServiceBase
    {
        private readonly SecretService _secretService;

        /// <inheritdoc />
        public LogonService(SimpleStrategyResolver strategyResolver, SecretService secretService) : base(strategyResolver: strategyResolver)
        {
            _secretService = secretService;
        }

        public async Task<bool> ValidateCredentials(PartialGradesEngines engine)
        {
            if (!Enum.IsDefined(enumType: typeof(PartialGradesEngines), value: engine))
                throw new InvalidEnumArgumentException(argumentName: nameof(engine), invalidValue: (int) engine,
                    enumClass: typeof(PartialGradesEngines));

            _partialGradesEngine = _strategyResolver.Resolve<IPartialGradesEngine>
                (namedStrategy: nameof(engine));

            try
            {
                await _partialGradesEngine.Authenticate().ConfigureAwait(false);
                return true;
            }
            catch (FlurlHttpException fhexc)
            {
                LogTo.FatalException(message: "Failed to authenticate user against Studia", exception: fhexc);
            }
            catch (InvalidOperationException iopexc)
            {
                LogTo.FatalException(message: "Studia server could have changed authentication workflow",
                    exception: iopexc);
            }
            return false;
        }

        public void ProvideUsernameAndPassword(string username, string password) =>
            _secretService.CreateOrUpdateSecret(container: Constants.STUDIA_RESOURCE_TOKEN, key: username, secret: password);
    }
}
