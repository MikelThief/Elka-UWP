using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Propertiary.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.Infrastructure.Resolvers;

namespace ElkaUWP.DataLayer.Propertiary.Services
{
    public class MapsService
    {
        private readonly SimpleStrategyResolver _strategyResolver;

        public MapsService(SimpleStrategyResolver strategyResolver)
        {
            _strategyResolver = strategyResolver;
        }

        public IEnumerable<FloorPlan> GetFloorPlans(string building)
        {
            var floorPlanResolver = _strategyResolver.Resolve<IFloorPlanResolver>(namedStrategy: building);
            return floorPlanResolver.ResolveAll();
        }
    }
}
