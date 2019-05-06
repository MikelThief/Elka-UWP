using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Abstractions.Interfaces;
using ElkaUWP.Infrastructure.Helpers;
using Microsoft.Toolkit.Uwp.Connectivity;
using Nito.Mvvm;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.Modularity.LoginModule.ViewModels
{
    public class StudiaLoginViewModel : BindableBase
    {
        private INavigationService _navigationService;
        private readonly ResourceLoader _resourceLoader = ResourceLoaderHelper.GetResourceLoaderForView(loginViewType: typeof(LoginModuleInitializer));

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

        public AsyncCommand AuthenticateStudiaAccountCommand { get; private set; }
        public AsyncCommand ContinueCommand { get; private set; }

        public StudiaLoginViewModel()
        {
            ContinueCommand = new AsyncCommand(executeAsync: Continue);
            AuthenticateStudiaAccountCommand = 
                new AsyncCommand(executeAsync: AuthenticateStudiaAccountAsync);
            IsAuthenticationSuccesful = default;
        }

        public async Task AuthenticateStudiaAccountAsync()
        {

        }

        private async Task Continue()
        {
            await _navigationService.NavigateAsync(name: PageTokens.ShellViewToken);
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
