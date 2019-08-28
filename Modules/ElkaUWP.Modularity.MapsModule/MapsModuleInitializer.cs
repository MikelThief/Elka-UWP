using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.Infrastructure;
using ElkaUWP.Modularity.MapsModule.ViewModels;
using ElkaUWP.Modularity.MapsModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace ElkaUWP.Modularity.MapsModule
{
    public class MapsModuleInitializer : IModule
    {
        /// <inheritdoc />
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MapsView, MapsViewModel>(key: PageTokens.MapsViewToken);
        }

        /// <inheritdoc />
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
