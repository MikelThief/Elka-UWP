using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Abstractions.Bases;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Entities;
using ElkaUWP.Infrastructure.Services;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Studia.Services
{
    public class GradeService : StudiaServiceBase
    {
        private readonly IGradesProxy _proxy;

        /// <inheritdoc />
        public GradeService(IGradesProxy proxy, SecretService secretService) : base(secretService: secretService)
        {
            _proxy = proxy;
        }

        public async Task<Subject> GetAsync(string semesterLiteral, string subjectId)
        {
            if (!_proxy.IsAuthenticated())
                await _proxy.Authenticate();

            Subject result;
            try
            {
                var response = await _proxy.GetAsync(subjectId: subjectId, semesterLiteral: semesterLiteral);
                result = JsonConvert.DeserializeObject<Subject>(value: response.Content.ToString());
            }
            catch(Exception exc)
            {

            }

            return null;
        }
    }
}
