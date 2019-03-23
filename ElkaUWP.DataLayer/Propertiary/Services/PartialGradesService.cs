using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserDataAccounts.Provider;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Services;

namespace ElkaUWP.DataLayer.Propertiary.Services
{
    public class PartialGradesService
    {
        private CrstestsService UsosService;

        public PartialGradesService(CrstestsService usosService)
        {
            UsosService = usosService;
        }

        public Task Get(string subjectId)
        {
            
        }
    }
}
