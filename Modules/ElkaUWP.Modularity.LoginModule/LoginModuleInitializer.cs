using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Abstractions.Interfaces;
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
            // Register View-VieModel pairs for navigation
            containerRegistry.RegisterForNavigation<WelcomeView, WelcomeViewModel>(key: PageTokens.LoginViewToken);
            containerRegistry.RegisterForNavigation<UsosLoginView, UsosLoginViewModel>(key: PageTokens.UsosLoginViewToken);
            containerRegistry.RegisterForNavigation<StudiaLoginView, StudiaLoginViewModel>(key: PageTokens.StudiaLoginViewToken);


        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public LoginModuleInitializer()
        {
            
        }
    }
}
