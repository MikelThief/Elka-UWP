using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.LoginModule.ViewModels;
using ElkaUWP.LoginModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace ElkaUWP.LoginModule
{
    public class LoginModuleInitializer : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>(key: nameof(LoginView));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public LoginModuleInitializer()
        {
            
        }
    }
}
