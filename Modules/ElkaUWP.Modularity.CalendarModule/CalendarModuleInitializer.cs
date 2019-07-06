using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.Infrastructure;
using ElkaUWP.Modularity.CalendarModule.ViewModels;
using ElkaUWP.Modularity.CalendarModule.Views;
using MvvmDialogs;
using MvvmDialogs.ContentDialogFactories;
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
            containerRegistry.RegisterForNavigation<ScheduleView, ScheduleViewModel>(key: PageTokens.CalendarSummaryView);

            var dialogservice = new DialogService();
            // Register types
            containerRegistry.RegisterInstance<IDialogService>(instance: dialogservice);
        }

        /// <inheritdoc />
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
