using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.DataLayer.Studia.Services;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Abstractions.Interfaces;
using ElkaUWP.Infrastructure.Extensions;
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

        public string Password
        {
            get => _password;
            set => SetProperty(storage: ref (_password), value: value,
                propertyName: nameof(Password));
        }

        public AsyncCommand AuthenticateCommand { get; private set; }
        public AsyncCommand ContinueCommand { get; private set; }

        public StudiaLoginViewModel(LogonService logonService)
        {
            _logonService = logonService;
            ContinueCommand = new AsyncCommand(executeAsync: Continue);
            AuthenticateCommand = new AsyncCommand(executeAsync: AuthenticateAsync);
            IsAuthenticationSuccesful = default;
        }

        private async Task AuthenticateAsync()
        {
            if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                NotificationManager.Show(notification: new SimpleNotification
                    {
                        TimeSpan = TimeSpan.FromSeconds(value: 4),
                        Text = _resourceLoader.GetString(resource: "No_Internet_Notification"),
                        Glyph = "\uF384",
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Background = new SolidColorBrush(color: Constants.RedColor)
                    }
                );
                return;
            }

            _logonService.ProvideUsernameAndPassword(username: Username, password: Password);
            var validationResult = await _logonService.ValidateCredentialsAsync()
                .ConfigureAwait(continueOnCapturedContext: true);


            if (validationResult.IsSuccess)
            {
                ApplicationData.Current.RoamingSettings.SaveString(key: SettingsKeys.StudiaStrategyKey,
                    value: Constants.LDAP_KEY);

                NotificationManager.Show(notification: new SimpleNotification
                    {
                        TimeSpan = TimeSpan.FromSeconds(value: 30),
                        Text = _resourceLoader.GetString(resource: "Studia_Login_Success_Notification"),
                        Glyph = "\uE8D7",
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Background = new SolidColorBrush(color: Constants.GreenColor)
                }
                );
                IsAuthenticationSuccesful = true;
            }
            else if(validationResult.IsFailure && validationResult.Error == ErrorCodes.STUDIA_HANDSHAKE_FAILED)
            {
                NotificationManager.Show(notification: new SimpleNotification
                    {
                        TimeSpan = TimeSpan.FromSeconds(value: 3),
                        Text = _resourceLoader.GetString(resource: "Studia_Handshake_Failed_Notification"),
                        Glyph = "\uEB90",
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Background = new SolidColorBrush(color: Constants.RedColor)
                    }
                );
            }
            else if(validationResult.IsFailure && validationResult.Error == ErrorCodes.STUDIA_BAD_DATA_RECEIVED)
            {
                NotificationManager.Show(notification: new SimpleNotification
                    {
                        TimeSpan = TimeSpan.FromSeconds(value: 3),
                        Text = _resourceLoader.GetString(resource: "Studia_Invalid_Credentials_Notification"),
                        Glyph = "\uEB90",
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Background = new SolidColorBrush(color: Constants.RedColor)
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
