using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anotar.NLog;
using ElkaUWP.DataLayer.Studia.Abstractions.Bases;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Entities;
using ElkaUWP.Infrastructure.Services;
using Flurl.Http;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Studia.Services
{
    public class GradeService : StudiaServiceBase
    {
        private readonly IGradesStrategy _proxy;

        /// <inheritdoc />
        public GradeService(IGradesStrategy proxy, SecretService secretService) : base(secretService: secretService)
        {
            _proxy = proxy;
        }

        public async Task<Subject> GetAsync(string semesterLiteral, string subjectId)
        {


            Subject result;
            try
            {
                var response = await _proxy.GetAsync(subjectId: subjectId, semesterLiteral: semesterLiteral);
                result = JsonConvert.DeserializeObject<Subject>(value: response.Content.ToString());
            }
            catch (FlurlHttpException fexc)
            {
                LogTo.ErrorException(message: $"Failed to retrieve partial grades for {subjectId}", exception: fexc);
                throw;
            }
            catch (JsonException jexc)
            {
                LogTo.ErrorException(message: $"Failed to deserialize partial grades json for {subjectId}", exception:
                    jexc);
                throw;
            }
            catch (Exception exc)
            {
                LogTo.ErrorException(
                    message: $"Unexpected exception occured while getting partial grades for {subjectId}",
                    exception: exc);
                throw;
            }

            return null;
        }
    }
}
