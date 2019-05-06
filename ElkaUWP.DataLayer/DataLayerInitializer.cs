using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Propertiary;
using ElkaUWP.DataLayer.Propertiary.Services;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.DataLayer.Studia.Resolvers;
using ElkaUWP.DataLayer.Studia.Strategies;
using ElkaUWP.DataLayer.Usos.Requests;
using ElkaUWP.DataLayer.Usos.Services;
using Flurl.Http.Configuration;
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
            containerRegistry.RegisterSingleton<UserInfoRequestWrapper>();

            // Register services
            containerRegistry.RegisterSingleton<TimeTableService>();
            containerRegistry.RegisterSingleton<UserService>();

            // Register strategies
            containerRegistry.RegisterSingleton<ILogonStrategy, LdapLogonStrategy>(name: nameof(LogonStrategies.LdapAsForm));

            // Register Resolvers
            containerRegistry.Register<ILogonStrategyResolver, LogonStrategyResolver>();

            //Register other types
            containerRegistry.RegisterSingleton<IFlurlClientFactory, PerHostFlurlClientFactory>();

        }

        /// <inheritdoc />
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
