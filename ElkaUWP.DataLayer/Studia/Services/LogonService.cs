using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using Windows.Web.Http;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Anotar.NLog;
using CSharpFunctionalExtensions;
using ElkaUWP.DataLayer.Studia.Abstractions.Bases;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Entities;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Resolvers;
using ElkaUWP.Infrastructure.Services;
using Flurl.Http;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Studia.Services
{
    public class LogonService : StudiaServiceBase
    {

        private readonly ILogonStrategy _logonStrategy;
        private readonly IPersonStrategy _personStrategy;

        // possible login data fields
        private string _username;
        private string _password;

        /// <inheritdoc />
        public LogonService(SimpleStrategyResolver resolver, SecretService secretService) : base(secretService: secretService)
        {
            _logonStrategy = resolver.Resolve<ILogonStrategy>(namedStrategy: Constants.LDAP_KEY);
            _personStrategy = resolver.Resolve<IPersonStrategy>(namedStrategy: Constants.LDAP_KEY);
        }

        public async Task<Result> ValidateCredentialsAsync()
        {
            try
            {
                var credential = new PasswordCredential(resource: Constants.STUDIA_CREDENTIAL_CONTAINER_NAME, userName: _username,
                    password: _password);

                await _logonStrategy.AuthenticateAsync(credential: credential).ConfigureAwait(false);
                var checkResponse = await _personStrategy.GetAsync().ConfigureAwait(continueOnCapturedContext: false);

                // throws an exception if the response does not contain json-formatted person data
                // which means that login credential is invalid. HTTP Response code check or checking
                // redirects is difficult due to server/UWP behaviors
                JsonConvert.DeserializeObject<PersonContainer>(value: await checkResponse.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
                checkResponse.Dispose(); // redundant at this point

                _secretService.CreateOrUpdateSecret(container: Constants.STUDIA_CREDENTIAL_CONTAINER_NAME,
                    key: _username, secret: _password);
                return Result.Ok();
            }
            catch (FlurlHttpException fhexc)
            {
                LogTo.ErrorException(message: "Failed to make a request or get an expected response.", exception: fhexc);
                return Result.Fail(error: ErrorCodes.STUDIA_HANDSHAKE_FAILED);
            }
            catch (JsonReaderException jsexc)
            {
                LogTo.WarnException(message: "Failed to deserialize incoming data. Login credentials are likely invalid.", exception: jsexc);
                return Result.Fail(error: ErrorCodes.STUDIA_BAD_DATA_RECEIVED);
            }
        }

        public void ProvideUsernameAndPassword(string username, string password)
        {
            _username = username;
            _password = password;
        }
    }
}
