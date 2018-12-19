using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using ElkaUWP.LoginModule.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.LoginModule.ViewModels
{
    public class LoginViewModel : BindableBase, INavigatedAware
    {
        public DelegateCommand StartWizardDelegateCommand { get; private set; }

        private INavigationService _navigationService;

        

        public LoginViewModel()
        {
            StartWizardDelegateCommand = new DelegateCommand(executeMethod: NavigateToUsosStep);
        }

        private async void NavigateToUsosStep()
        {
            await _navigationService.NavigateAsync(path: nameof(UsosStepView), parameter: new NavigationParameters(), infoOverride: new SlideNavigationTransitionInfo()
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
