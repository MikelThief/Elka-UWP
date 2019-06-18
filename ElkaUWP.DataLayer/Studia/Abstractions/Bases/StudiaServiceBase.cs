using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.Infrastructure.Services;

namespace ElkaUWP.DataLayer.Studia.Abstractions.Bases
{
    public abstract class StudiaServiceBase
    {
        protected readonly SecretService _secretService;
        public StudiaServiceBase(SecretService secretService)
        {
            _secretService = secretService;
        }
    }
}
