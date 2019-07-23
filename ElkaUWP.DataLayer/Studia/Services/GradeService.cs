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
        private readonly IGradesStrategy _proxy;

        /// <inheritdoc />
        public GradeService(SimpleStrategyResolver resolver, SecretService secretService) : base(secretService: secretService)
        {
            _proxy = resolver.Resolve<IGradesStrategy>(namedStrategy: Constants.LDAP_KEY);
        }

        public async Task<Result<Subject>> GetAsync(string semesterLiteral, string subjectId)
        {
            Subject result;
            try
            {
                var response = await _proxy.GetAsync(subjectId: subjectId, semesterLiteral: semesterLiteral);
                result = JsonConvert.DeserializeObject<Subject>(value: response.Content.ToString());
                return Result.Ok(value: result);
            }
            catch (FlurlHttpException fexc)
            {
                LogTo.ErrorException(message: $"Failed to retrieve partial grades for {subjectId}", exception: fexc);
            }
            catch (JsonException jexc)
            {
                LogTo.ErrorException(message: $"Failed to deserialize partial grades json for {subjectId}", exception:
                    jexc);
            }

            return Result.Fail<Subject>("");
        }
    }
}
