using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using ElkaUWP.Infrastructure;
using ElkaUWP.LoginModule.Views;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.LoginModule.ViewModels
{
    public class LoginViewModel : BindableBase, INavigatedAware
    {
        public DelegateCommand StartWizardDelegateCommand { get; private set; }

        protected INavigationService _navigationService;
        protected ILogger Logger { get; }

        public LoginViewModel(ILogger logger)
        {
            StartWizardDelegateCommand = new DelegateCommand(executeMethod: NavigateToUsosStep);
            Logger = logger;
        }

        private async void NavigateToUsosStep()
        {
            await _navigationService.NavigateAsync(path: PageTokens.UsosStepViewToken, infoOverride: new SlideNavigationTransitionInfo()
                { Effect = SlideNavigationTransitionEffect.FromRight }) ;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            _navigationService = parameters.GetNavigationService();
        }
    }
}
