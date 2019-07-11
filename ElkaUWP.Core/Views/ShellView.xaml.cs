using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using ElkaUWP.Core.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Helpers;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace ElkaUWP.Core.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellView.xaml.
    public sealed partial class ShellView : Page
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public ShellView()
        {
            InitializeComponent();

            var formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;

            formattableTitleBar.BackgroundColor = Colors.Transparent;

            formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
            formattableTitleBar.ButtonForegroundColor = (Color)Resources["SystemBaseHighColor"];

            formattableTitleBar.ButtonPressedBackgroundColor = (Color)Resources["SystemAccentColorDark3"];
            formattableTitleBar.ButtonPressedForegroundColor = (Color)Resources["SystemBaseLowColor"];

            formattableTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            formattableTitleBar.ButtonInactiveForegroundColor = (Color)Resources["SystemChromeGrayColor"];

            formattableTitleBar.ButtonHoverBackgroundColor = (Color)Resources["SystemAccentColor"];
            formattableTitleBar.ButtonHoverForegroundColor = (Color)Resources["SystemAltMediumColor"];

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            
        }

        private void Nv_Loaded(object sender, RoutedEventArgs e)
        {
            // NavigationService for internal frame
            ViewModel.Initialize(internalFrame: ContentFrame);
        }

        private void Nv_ItemInvoked(WinUI.NavigationView navigationView, WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ViewModel.RequestInternalNavigation(navigationPath: PageTokens.SettingsViewToken,
                    transitionInfo: new EntranceNavigationTransitionInfo());
            }
            else if (args.InvokedItemContainer != null)
            {
                switch (args.InvokedItemContainer.Tag.ToString())
                {
                    case PageTokens.GradesModuleGradesView:
                        ViewModel.RequestInternalNavigation(navigationPath: PageTokens.GradesModuleGradesView,
                            transitionInfo: new EntranceNavigationTransitionInfo());
                        break;
                    //TODO: Remove when going to production
                    case "SampleViewToken":
                        ViewModel.RequestInternalNavigation(navigationPath: PageTokens.SampleViewToken,
                            transitionInfo: new EntranceNavigationTransitionInfo());
                        break;
                    //TODO: Remove when going to production
                    case "LoginToken":
                        ViewModel.RequestExternalNavigation(navigationPath: PageTokens.WelcomeViewToken,
                            transitionInfo: new EntranceNavigationTransitionInfo());
                        break;
                    case PageTokens.CalendarSummaryView:
                        ViewModel.RequestInternalNavigation(navigationPath: PageTokens.CalendarSummaryView,
                            transitionInfo: new EntranceNavigationTransitionInfo());
                        break;
                    case PageTokens.UserSummaryViewToken:
                        ViewModel.RequestInternalNavigation(navigationPath: PageTokens.UserSummaryViewToken,
                            transitionInfo: new EntranceNavigationTransitionInfo());
                        break;
                    default:
                        break;
                }
            }
        }

        private void Nv_OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            ViewModel.RequestInternalNavigation("../");
        }

        public TitleBarHelper TitleHelper => TitleBarHelper.Instance;
    }
}
