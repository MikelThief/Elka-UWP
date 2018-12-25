using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Interfaces;
using ElkaUWP.LoginModule.Service;
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
            // Register services
            containerRegistry.RegisterSingleton<IUsosOAuthService, UsosOAuthService>();

            // Register View-VieModel pairs for navigation
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>(key: PageTokens.LoginViewToken);
            containerRegistry.RegisterForNavigation<UsosStepView, UsosStepViewModel>(key: PageTokens.UsosStepViewToken);

            // Register other types
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public LoginModuleInitializer()
        {
            
        }
    }
}
