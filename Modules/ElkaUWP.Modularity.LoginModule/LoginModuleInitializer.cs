using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Interfaces;
using ElkaUWP.Modularity.LoginModule.Service;
using ElkaUWP.Modularity.LoginModule.ViewModels;
using ElkaUWP.Modularity.LoginModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace ElkaUWP.Modularity.LoginModule
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
