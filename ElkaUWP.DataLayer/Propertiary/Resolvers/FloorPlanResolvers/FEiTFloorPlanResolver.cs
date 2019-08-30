using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Propertiary.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Propertiary.Entities;

namespace ElkaUWP.DataLayer.Propertiary.Resolvers.FloorPlanResolvers
{
    public class FEiTFloorPlanResolver : IFloorPlanResolver
    {
        /// <inheritdoc />
        public IEnumerable<FloorPlan> ResolveAll()
        {
            return new List<FloorPlan>
            {
                new FloorPlan(level: -1, imageUri: new Uri(uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Basement1.jpg")),
                new FloorPlan(level: 0, imageUri: new Uri(uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor0.png")),
                new FloorPlan(level: 1, imageUri: new Uri(uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor1.jpg")),
                new FloorPlan(level: 2, imageUri: new Uri(uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor2.jpg")),
                new FloorPlan(level: 3, imageUri: new Uri(uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor3.jpg")),
                new FloorPlan(level: 4, imageUri: new Uri(uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor4.jpg")),
                new FloorPlan(level: 5 ,imageUri: new Uri(uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor5.jpg"))
            };
        }
    }
}
