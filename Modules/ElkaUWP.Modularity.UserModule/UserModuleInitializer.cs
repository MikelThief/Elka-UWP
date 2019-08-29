using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.Infrastructure;
using ElkaUWP.Modularity.UserModule.ViewModels;
using ElkaUWP.Modularity.UserModule.Views;


namespace ElkaUWP.Modularity.UserModule
{
    public class UserModuleInitializer : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<UserView, UserViewModel>(key: PageTokens.UserSummaryViewToken);
        }
    }
}
