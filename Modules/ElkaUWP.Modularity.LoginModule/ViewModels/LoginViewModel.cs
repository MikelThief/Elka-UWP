using Windows.UI.Xaml.Media.Animation;
using ElkaUWP.Infrastructure;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.Modularity.LoginModule.ViewModels
{
    public class LoginViewModel : BindableBase, INavigationAware
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

        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            _navigationService = parameters.GetNavigationService();
        }
    }
}
