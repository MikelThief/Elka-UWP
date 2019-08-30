using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Propertiary.Entities;

namespace ElkaUWP.DataLayer.Propertiary.Abstractions.Interfaces
{
    public interface IFloorPlanResolver
    {
        IEnumerable<FloorPlan> ResolveAll();
    }
}
