using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using ElkaUWP.Core.Helpers;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ElkaUWP.Infrastructure;
using Prism.Services;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace ElkaUWP.Core.ViewModels
{
    public class ShellViewModel : BindableBase, INavigatedAware
    {
        private bool isGradesViewSelected = default;

        private INavigationService _internalNavigationService;
        private INavigationService _externalNavigationService;

        private bool _canGoBack;

        public bool CanGoBack
        {
            get => _canGoBack;
            set => SetProperty(ref _canGoBack, value: value, nameof(CanGoBack));
        }

        public ShellViewModel()
        {

        }

        /// <summary>
        /// Called when View is loaded and internal navigation service can be created.
        /// </summary>
        public void Initialize(Frame internalFrame)
        {
            _internalNavigationService = Prism.Navigation.NavigationService.Create(frame: internalFrame, Gesture.Back,
                Gesture.Forward, Gesture.Refresh);
        }

        public async void RequestExternalNavigation(string navigationPath, INavigationParameters parameters = null)
        {
            if(parameters is null)
                await _externalNavigationService.NavigateAsync(name: "/" + navigationPath);
            else await _externalNavigationService.NavigateAsync(name: "/" + navigationPath, parameters: parameters);
        }

        public async void RequestInternalNavigation(string navigationPath, INavigationParameters parameters = null)
        {
            switch (navigationPath)
            {
                // TODO: Very dirty fix for an issue: https://github.com/windows-toolkit/WindowsCommunityToolkit/issues/2638
                case "../":
                    if (isGradesViewSelected)
                    {
                        isGradesViewSelected = false;
                        await _internalNavigationService.NavigateAsync(name: "/" + PageTokens.GradesModuleGradesView);
                        CanGoBack = false;
                    }
                    else
                    {
                        await _internalNavigationService.GoBackAsync();
                        CanGoBack = _internalNavigationService.CanGoBack();
                    }
                    break;
                default:
                    if (navigationPath == PageTokens.GradesModuleGradesView)
                        isGradesViewSelected = true;

                    await _internalNavigationService.NavigateAsync(name:"/" + navigationPath, parameters: parameters);
                    CanGoBack = false;
                    break;
            }


            
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }


        public void OnNavigatedTo(INavigationParameters parameters)
        {
            _externalNavigationService = parameters.GetNavigationService();
        }
    }
}
