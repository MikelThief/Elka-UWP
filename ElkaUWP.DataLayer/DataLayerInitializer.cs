using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Propertiary;
using ElkaUWP.DataLayer.Propertiary.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Propertiary.Resolvers.FloorPlanResolvers;
using ElkaUWP.DataLayer.Propertiary.Services;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.DataLayer.Studia.Proxies;
using ElkaUWP.DataLayer.Usos.Requests;
using ElkaUWP.DataLayer.Usos.Services;
using Flurl.Http.Configuration;
using Prism.Ioc;
using Prism.Modularity;
using Usos = ElkaUWP.DataLayer.Usos;
using ElkaUWP.DataLayer.Studia;
using Infrastructure = ElkaUWP.Infrastructure;

namespace ElkaUWP.DataLayer
{
    public class DataLayerInitializer : IModule
    {
        /// <inheritdoc />
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register strategies
            containerRegistry.RegisterSingleton<IGradesStrategy, LdapFormGradesStrategy>(name: Infrastructure.Constants.LDAP_KEY);
            containerRegistry.RegisterSingleton<ILogonStrategy, LdapFormLogonStrategy>(name: Infrastructure.Constants.LDAP_KEY);
            containerRegistry.RegisterSingleton<IPersonStrategy, LdapFormPersonStrategy>(name: Infrastructure.Constants.LDAP_KEY);
            containerRegistry.RegisterSingleton<IFloorPlanResolver, FEiTFloorPlanResolver>(name: Infrastructure.Buildings.WUTW_FEIT_BUILDING);

            //Register other types
            containerRegistry.RegisterSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();

        }

        /// <inheritdoc />
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
