using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Windows.Web.Http;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;

namespace ElkaUWP.DataLayer.Studia.Services
{
    public class SubjectsPartialGradesService
    {
        private readonly IPartialGradesEngine _partialGradesEngine;

        public SubjectsPartialGradesService(IPartialGradesEngine partialGradesEngine)
        {
            _partialGradesEngine = partialGradesEngine;
        }

        public async Task<bool> CheckIfCorrectLogin()
        {

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
