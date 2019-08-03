using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Anotar.NLog;
using CSharpFunctionalExtensions;
using ElkaUWP.DataLayer.Studia.Abstractions.Bases;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Entities;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Resolvers;
using ElkaUWP.Infrastructure.Services;
using Flurl.Http;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Studia.Services
{
    public class GradeService : StudiaServiceBase
    {
        private readonly SecretService _secretService;
        private readonly IGradesStrategy _gradesProxy;
        private readonly ILogonStrategy _logonStrategy;

        private IReadOnlyList<string> BannedEntries = new List<string>
        {
            "Nazwisko",
            "Imiona",
            "Identyfikator serwera Studia",
            "Zapis",
            "Indeks",
            "Grupa",
            "Grupa zajęciowa LAB",
            "Grupa zajęciowa CWI",
            "Grupa zajęciowa PRO",
            "Grupa zajęciowa WYK"
        };

        /// <inheritdoc />
        public GradeService(SimpleStrategyResolver resolver, SecretService secretService) : base(secretService: secretService)
        {
            _secretService = secretService;
            _gradesProxy = resolver.Resolve<IGradesStrategy>(namedStrategy: Constants.LDAP_KEY);
            _logonStrategy = resolver.Resolve<ILogonStrategy>(namedStrategy: Constants.LDAP_KEY);
        }

        public async Task<Result<Maybe<List<PartialGradeItem>>>> GetAsync(string semesterLiteral, string subjectId)
        {
            var secret = _secretService.GetSecret(Constants.STUDIA_CREDENTIAL_CONTAINER_NAME);
            secret.RetrievePassword();


            if (!_logonStrategy.IsAuthenticated())
                await _logonStrategy.AuthenticateAsync(credential: secret).ConfigureAwait(false);

            try
            {
                var response = await _gradesProxy.GetAsync(subjectId: subjectId, semesterLiteral: semesterLiteral);
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Subject>(value: content);
                var partialGrades = result.PartialGrades;

                foreach (var partialGrade in partialGrades.ToList())
                {
                    if (BannedEntries.Contains(value: partialGrade.Title) || string.IsNullOrEmpty(value: partialGrade.Value))
                        partialGrades.Remove(item: partialGrade);
                }

                return Result.Ok(value: Maybe<List<PartialGradeItem>>.From(obj: partialGrades.Count > 0 ? partialGrades : null));
            }
            catch (FlurlHttpException fexc)
            {
                LogTo.ErrorException(message: $"Failed to retrieve partial grades for {subjectId}", exception: fexc);
                return Result.Fail<Maybe<List<PartialGradeItem>>>(error: ErrorCodes.STUDIA_HANDSHAKE_FAILED);
            }
            catch (JsonException jexc)
            {
                LogTo.ErrorException(message: $"Failed to deserialize partial grades json for {subjectId}", exception:
                    jexc);
                return Result.Fail<Maybe<List<PartialGradeItem>>>(error: ErrorCodes.STUDIA_BAD_DATA_RECEIVED);
            }
        }
    }
}
