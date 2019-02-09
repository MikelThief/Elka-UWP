using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.Infrastructure;
using ElkaUWP.Modularity.CalendarModule.ViewModels;
using ElkaUWP.Modularity.CalendarModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace ElkaUWP.Modularity.CalendarModule
{
    public class CalendarModuleInitializer : IModule
    {
        /// <inheritdoc />
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register View-Viewmodel pairs
            containerRegistry.RegisterForNavigation<SummaryView, SummaryViewModel>(key: PageTokens.CalendarSummaryView);
        }

        /// <inheritdoc />
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
