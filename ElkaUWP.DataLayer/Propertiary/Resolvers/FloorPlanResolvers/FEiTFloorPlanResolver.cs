using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ElkaUWP.DataLayer.Propertiary.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Propertiary.Entities;
using Windows.Storage;
using Windows.UI.Xaml;

namespace ElkaUWP.DataLayer.Propertiary.Resolvers.FloorPlanResolvers
{
    public class FEiTFloorPlanResolver : IFloorPlanResolver
    {
        private List<FloorPlan> floorPlansLight = new List<FloorPlan>
        {
            new FloorPlan(level: -1,
                imageUri: new Uri(
                    uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Basement-theme-light.png")),
            new FloorPlan(level: 0,
                imageUri: new Uri(
                    uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor0-theme-light.png")),
            new FloorPlan(level: 1,
                imageUri: new Uri(
                    uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor1-theme-light.png")),
            new FloorPlan(level: 2,
                imageUri: new Uri(
                    uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor2-theme-light.png")),
            new FloorPlan(level: 3,
                imageUri: new Uri(
                    uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor3-theme-light.png")),
            new FloorPlan(level: 4,
                imageUri: new Uri(
                    uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor4-theme-light.png")),
            new FloorPlan(level: 5,
                imageUri: new Uri(
                    uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor5-theme-light.png"))
        };
        private List<FloorPlan> floorPlansDark = new List<FloorPlan>
        {
            new FloorPlan(level: -1,
                imageUri: new Uri(
                    uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Basement-theme-dark.png")),
            new FloorPlan(level: 0,
                imageUri: new Uri(
                    uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor0-theme-dark.png")),
            new FloorPlan(level: 1,
                imageUri: new Uri(
                    uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor1-theme-dark.png")),
            new FloorPlan(level: 2,
                imageUri: new Uri(
                    uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor2-theme-dark.png")),
            new FloorPlan(level: 3,
                imageUri: new Uri(
                    uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor3-theme-dark.png")),
            new FloorPlan(level: 4,
                imageUri: new Uri(
                    uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor4-theme-dark.png")),
            new FloorPlan(level: 5,
                imageUri: new Uri(
                    uriString: "ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorPlans/FEiT/Floor5-theme-dark.png"))
        };

        /// <inheritdoc />
        public IEnumerable<FloorPlan> ResolveAll()
        {
            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                if (frameworkElement.RequestedTheme == ElementTheme.Light)
                    return floorPlansLight;
                else if (frameworkElement.RequestedTheme == ElementTheme.Dark)
                    return floorPlansDark;
                else
                    return floorPlansLight;
            }
            else
                return floorPlansLight;
                
        }
    }
}
