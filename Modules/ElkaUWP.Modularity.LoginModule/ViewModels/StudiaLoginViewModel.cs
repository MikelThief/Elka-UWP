using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.DataLayer.Studia.Services;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Abstractions.Interfaces;
using ElkaUWP.Infrastructure.Helpers;
using ElkaUWP.Infrastructure.Services;
using Microsoft.Toolkit.Uwp.Connectivity;
using Nito.Mvvm;
using Prism.Mvvm;
using Prism.Navigation;
using RavinduL.LocalNotifications;
using RavinduL.LocalNotifications.Notifications;

namespace ElkaUWP.Modularity.LoginModule.ViewModels
{
    public class StudiaLoginViewModel : BindableBase, INavigatedAware
    {
        private readonly LogonService _logonService;
        private INavigationService _navigationService;
        private readonly ResourceLoader _resourceLoader = 
            ResourceLoaderHelper.GetResourceLoaderForView(viewType: typeof(LoginModuleInitializer));

        public LocalNotificationManager NotificationManager { get; set; }

        private bool _isAuthenticationSuccesful;

        public bool IsAuthenticationSuccesful
        {
            get => _isAuthenticationSuccesful;
            set => SetProperty(storage: ref _isAuthenticationSuccesful, value: value,
                propertyName: nameof(IsAuthenticationSuccesful));
        }

        private string _username;

        public string Username
        {
            get => _username;
            set => SetProperty(storage: ref (_username), value: value,
                propertyName: nameof(Username));
        }

        private string _password;
        private readonly SecretService _secretService;

        public string Password
        {
            get => _password;
            set => SetProperty(storage: ref (_password), value: value,
                propertyName: nameof(Password));
        }

        public AsyncCommand AuthenticateCommand { get; private set; }
        public AsyncCommand ContinueCommand { get; private set; }

        public StudiaLoginViewModel(LogonService logonService, SecretService secretService)
        {
            _logonService = logonService;
            ContinueCommand = new AsyncCommand(executeAsync: Continue);
            AuthenticateCommand = new AsyncCommand(executeAsync: AuthenticateAsync);
            IsAuthenticationSuccesful = default;
            _secretService = secretService;
        }

        private async Task AuthenticateAsync()
        {
            _logonService.ProvideUsernameAndPassword(username: Username, password: Password);

            if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                NotificationManager.Show(notification: new SimpleNotification
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

            var validationResult = await _logonService.ValidateCredentialsAsync()
                .ConfigureAwait(continueOnCapturedContext: true);


            if (validationResult)
            {
                NotificationManager.Show(notification: new SimpleNotification
                    {
                        TimeSpan = TimeSpan.FromSeconds(value: 30),
                        Text = _resourceLoader.GetString(resource: "Studia_Login_Success_Notification"),
                        Glyph = "\uE8D7",
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Background = BrushFromColorHelper.GetSolidColorBrush(colorName: nameof(Colors.Green))
                    }
                );
                IsAuthenticationSuccesful = true;
            }
            else
            {
                NotificationManager.Show(notification: new SimpleNotification
                    {
                        TimeSpan = TimeSpan.FromSeconds(value: 3),
                        Text = _resourceLoader.GetString(resource: "Studia_Login_Failure_Notification"),
                        Glyph = "\uEB90",
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Background = BrushFromColorHelper.GetSolidColorBrush(colorName: nameof(Colors.Red))
                    }
                );
            }

        }

        private Task Continue()
        {
            return _navigationService.NavigateAsync(name: PageTokens.ShellViewToken);
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
