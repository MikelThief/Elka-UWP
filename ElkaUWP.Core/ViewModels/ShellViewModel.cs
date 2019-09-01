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
using Windows.UI.Xaml.Media.Animation;
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

        public async void RequestExternalNavigation(string navigationPath, INavigationParameters parameters = null, NavigationTransitionInfo transitionInfo = null)
        {
            if (transitionInfo == null)
                transitionInfo = new SuppressNavigationTransitionInfo();

            await _externalNavigationService.NavigateAsync(path: "/" + navigationPath, parameter: parameters,
                infoOverride: transitionInfo);
        }

        public async void RequestInternalNavigation(string navigationPath, INavigationParameters parameters = null, NavigationTransitionInfo transitionInfo = null)
        {
            if(transitionInfo == null)
                transitionInfo = new SuppressNavigationTransitionInfo();

            switch (navigationPath)
            {
                // TODO: Very dirty fix for an issue: https://github.com/windows-toolkit/WindowsCommunityToolkit/issues/2638
                case "../":
                    if (isGradesViewSelected)
                    {
                        await _internalNavigationService.NavigateAsync(path: "/" + PageTokens.GradesGradesView, parameter: parameters, infoOverride: transitionInfo);
                        CanGoBack = false;
                    }
                    else
                    {
                        await _internalNavigationService.GoBackAsync();
                        CanGoBack = _internalNavigationService.CanGoBack();
                    }
                    break;
                default:
                    if (navigationPath == PageTokens.GradesGradesView)
                        isGradesViewSelected = true;
                    else
                        isGradesViewSelected = false;
                    await _internalNavigationService.NavigateAsync(path: "/" + navigationPath, parameter: parameters, infoOverride: transitionInfo);
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
