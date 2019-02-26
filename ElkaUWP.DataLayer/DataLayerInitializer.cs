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
            containerRegistry.RegisterSingleton<StudentTimeTableRequestWrapper>();
            containerRegistry.RegisterSingleton<BuildingIndexRequestWrapper>();
            containerRegistry.RegisterSingleton<UpcomingICalRequestWrapper>();
            containerRegistry.RegisterSingleton<UpcomingWebCalFeedRequestWrapper>();
            containerRegistry.RegisterSingleton<UserRequestWrapper>();

            // Register services
            containerRegistry.RegisterSingleton<TimeTableService>();
            containerRegistry.RegisterSingleton<UserService>();
        }

        /// <inheritdoc />
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
