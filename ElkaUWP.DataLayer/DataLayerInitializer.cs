using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Requests;
using ElkaUWP.DataLayer.Usos.Services;
using Prism.Ioc;
using Prism.Modularity;

namespace ElkaUWP.DataLayer
{
    public class DataLayerInitializer : IModule
    {
        /// <inheritdoc />
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register USOS request wrappers
            containerRegistry.RegisterSingleton<UserCoursesPerSemesterRequestWrapper>();
            containerRegistry.RegisterSingleton<StudentTimeTableRequestWrapper>();
            containerRegistry.RegisterSingleton<BuildingIndexRequestWrapper>();
            containerRegistry.RegisterSingleton<UpcomingICalRequestWrapper>();
            containerRegistry.RegisterSingleton<UpcomingWebCalFeedRequestWrapper>();

            // Register services
            containerRegistry.RegisterSingleton<GradesService>();
            containerRegistry.RegisterSingleton<TimeTableService>();
        }

        /// <inheritdoc />
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
