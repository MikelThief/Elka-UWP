using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Abstractions.Interfaces;
using ElkaUWP.Infrastructure.Helpers;
using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.Extensions;
using Nito.Mvvm;
using Prism.Mvvm;
using Prism.Navigation;
using RavinduL.LocalNotifications;
using RavinduL.LocalNotifications.Notifications;

namespace ElkaUWP.Modularity.LoginModule.ViewModels
{
    public class UsosLoginViewModel : BindableBase, INavigatedAware
    {
        private INavigationService _navigationService;
        private readonly ResourceLoader _resourceLoader = ResourceLoaderHelper.GetResourceLoaderForView(viewType: typeof(LoginModuleInitializer));
        private readonly IUsosOAuthService _usosOAuthService;
        public LocalNotificationManager NotificationManager { get; set; }

        private bool _isSignInButtonEnabled;
        public bool IsSignInButtonEnabled
        {
            get => _isSignInButtonEnabled;
            set => SetProperty(storage: ref _isSignInButtonEnabled, value: value,
                propertyName: nameof(IsSignInButtonEnabled));
        }

        private bool _isContinueButtonVisible;
        public bool IsContinueButtonVisible
        {
            get => _isContinueButtonVisible;
            set => SetProperty(storage: ref _isContinueButtonVisible, value: value,
                propertyName: nameof(IsContinueButtonVisible));
        }

        public AsyncCommand AuthenticateUsosAccountCommand { get; private set; }
        public AsyncCommand ContinueCommand { get; private set; }

        public UsosLoginViewModel(IUsosOAuthService usosOAuthService)
        {
            AuthenticateUsosAccountCommand = new AsyncCommand(executeAsync: StartUsosAuthorizationProcessAsync);
            ContinueCommand = new AsyncCommand(executeAsync: Continue);
            _usosOAuthService = usosOAuthService;
            IsSignInButtonEnabled = default;
            IsContinueButtonVisible = default;
        }

        private async Task StartUsosAuthorizationProcessAsync()
        {
            if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                NotificationManager.Show(new SimpleNotification
                    {
                        TimeSpan = TimeSpan.FromSeconds(value: 4),
                        Text = _resourceLoader.GetString(resource: "No_Internet_Body"),
                        Glyph = "\uF384",
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Background = BrushFromColorHelper.GetSolidColorBrush(colorName: nameof(Colors.Red))
                    }
                );
                return;
            }

            await _usosOAuthService.StartAuthorizationAsync().ConfigureAwait(continueOnCapturedContext: false);
        }

        private async Task Continue()
        {
            await _navigationService.NavigateAsync(name: PageTokens.StudiaLoginViewToken);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            _navigationService = parameters.GetNavigationService();

            if (parameters.ContainsKey(key: NavigationParameterKeys.IS_USOS_AUTHORIZED)
                && parameters.GetValue<bool>(key: NavigationParameterKeys.IS_USOS_AUTHORIZED))
            {
                IsSignInButtonEnabled = false;
                IsContinueButtonVisible = true;
                NotificationManager.Show(new SimpleNotification
                    {
                        TimeSpan = TimeSpan.FromSeconds(value: 30),
                        Text = _resourceLoader.GetString(resource: "Usos_Login_Success_Notification"),
                        Glyph = "\uE8D7",
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Background = BrushFromColorHelper.GetSolidColorBrush(colorName: nameof(Colors.Green))
                    }
                );
            }
            else
            {
                IsSignInButtonEnabled = true;
                IsContinueButtonVisible = false;
            }
        }
    }
}
