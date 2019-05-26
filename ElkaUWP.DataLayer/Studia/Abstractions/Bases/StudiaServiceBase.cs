using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.DataLayer.Studia.Resolvers;

namespace ElkaUWP.DataLayer.Studia.Abstractions.Bases
{
    public abstract class StudiaServiceBase
    {
        protected IPartialGradesEngine _partialGradesEngine;
        protected readonly SimpleStrategyResolver _strategyResolver;

        public StudiaServiceBase(SimpleStrategyResolver strategyResolver)
        {
            _strategyResolver = strategyResolver;
        }
    }
}
