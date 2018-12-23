using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.ApplicationModel.Store.Preview.InstallControl;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Helpers;
using ElkaUWP.Infrastructure.Interfaces;
using Microsoft.Toolkit.Uwp.Connectivity;
using Nito.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.LoginModule.ViewModels
{
    public class UsosStepViewModel : BindableBase, INavigatedAware
    {
        private INavigationService _navigationService;
        private readonly ResourceLoader _resourceLoader = ResourceLoaderHelper.GetResourceLoaderForView(loginViewType: typeof(LoginModuleInitializer));

        private readonly IUsosOAuthService _usosOAuthService;

        #region ControlsBindingVariables
        private bool _isSignInButtonEnabled;
        public bool IsSignInButtonEnabled
        {
            get => _isSignInButtonEnabled;
            set => SetProperty(storage: ref _isSignInButtonEnabled, value: value);
        }

        #endregion

        public AsyncCommand StartUsosAuthorizationProcessCommand { get; private set; }

        public UsosStepViewModel(IUsosOAuthService usosOAuthService)
        {
            StartUsosAuthorizationProcessCommand = new AsyncCommand(executeAsync: StartUsosAuthorizationProcessAsync);
            _usosOAuthService = usosOAuthService;
            IsSignInButtonEnabled = default(bool);
        }

        private async Task StartUsosAuthorizationProcessAsync()
        {
            if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                var noInternetDialog = new ContentDialog()
                {
                    Title = _resourceLoader.GetString(resource: "No_Internet_Title"),
                    Content = _resourceLoader.GetString(resource: "No_Internet_Body"),
                    CloseButtonText = "OK"
                };
                await noInternetDialog.ShowAsync();
                return;
            }

            await _usosOAuthService.AuthorizeAsync();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            _navigationService = parameters.GetNavigationService();



            if (parameters.ContainsKey(key: "usos_authorized") && parameters.GetValue<bool>(key: "usos_authorized"))
                IsSignInButtonEnabled = false;
            else
                IsSignInButtonEnabled = true;
        }
    }
}
