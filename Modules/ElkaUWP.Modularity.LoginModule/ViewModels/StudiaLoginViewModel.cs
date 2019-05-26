using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.DataLayer.Studia.Services;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Abstractions.Interfaces;
using ElkaUWP.Infrastructure.Helpers;
using Nito.Mvvm;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.Modularity.LoginModule.ViewModels
{
    public class StudiaLoginViewModel : BindableBase, INavigatedAware
    {
        private readonly LogonService _logonService;
        private INavigationService _navigationService;
        private readonly ResourceLoader _resourceLoader = 
            ResourceLoaderHelper.GetResourceLoaderForView(loginViewType: typeof(LoginModuleInitializer));

        private bool _isAuthenticationSuccesful;

        private bool? _isLdapLogonChecked;

        public bool? IsLdapLogonChecked
        {
            get => _isLdapLogonChecked;
            set
            {
                SetProperty(storage: ref _isLdapLogonChecked, value: value,
                    propertyName: nameof(IsAuthenticationSuccesful));
                RaisePropertyChanged(propertyName: nameof(IsAuthenticationSuccesful));
            }
        }

        public bool IsLoginAndPasswordFieldsVisible =>
            IsLdapLogonChecked.HasValue && IsLdapLogonChecked.Value;

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

        public AsyncCommand LogInCommand { get; private set; }
        public AsyncCommand ContinueCommand { get; private set; }

        public StudiaLoginViewModel(LogonService logonService)
        {
            _logonService = logonService;
            ContinueCommand = new AsyncCommand(executeAsync: Continue);
            LogInCommand = new AsyncCommand(executeAsync: LogInAsync);
            IsAuthenticationSuccesful = default;
        }

        public async Task LogInAsync()
        {
            await _logonService.ValidateCredentials(PartialGradesEngines.LdapFormPartialGradeEngine);
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
