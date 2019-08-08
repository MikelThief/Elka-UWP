using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Propertiary;
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
using ElkaUWP.Infrastructure;

namespace ElkaUWP.DataLayer
{
    public class DataLayerInitializer : IModule
    {
        /// <inheritdoc />
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register USOS request wrappers
            containerRegistry.RegisterSingleton<TimetableStudentRequestWrapper>();
            containerRegistry.RegisterSingleton<BuildingIndexRequestWrapper>();
            containerRegistry.RegisterSingleton<TimetableUpcomingICalRequestWrapper>();
            containerRegistry.RegisterSingleton<TimetableUpcomingWebCalRequestWrapper>();
            containerRegistry.RegisterSingleton<UserInfoRequestWrapper>();

            // Register services
            containerRegistry.RegisterSingleton<Usos.Services.TimetableService>();
            containerRegistry.RegisterSingleton<UserService>();
            containerRegistry.RegisterSingleton<Usos.Services.LogonService>();
            containerRegistry.RegisterSingleton<SearchService>();

            // Register proxies
            containerRegistry.RegisterSingleton<IGradesStrategy, LdapFormGradesStrategy>(name: Constants.LDAP_KEY);
            containerRegistry.RegisterSingleton<ILogonStrategy, LdapFormLogonStrategy>(name: Constants.LDAP_KEY);
            containerRegistry.RegisterSingleton<IPersonStrategy, LdapFormPersonStrategy>(name: Constants.LDAP_KEY);

            //Register other types
            containerRegistry.RegisterSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();

        }

        /// <inheritdoc />
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
