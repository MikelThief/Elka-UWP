using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Abstractions.Bases;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Entities;
using ElkaUWP.Infrastructure.Services;

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

        public Task<Subject> GetAsync(string semesterLiteral, string subjectId)
        {
            throw new NotImplementedException();
        }
    }
}
