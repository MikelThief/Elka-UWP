using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using ElkaUWP.Core.ViewModels;

using Windows.UI.Xaml.Controls;
using ElkaUWP.Infrastructure;
using Prism.Mvvm;
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


            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
            formattableTitleBar.InactiveBackgroundColor = Colors.Transparent;
            formattableTitleBar.ButtonForegroundColor = (Color)Resources["SystemBaseHighColor"];

            // Hide default title bar.
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
            // check if item belongs to Nv
            var invokedItems = this.NavigationViewControl.MenuItems.Cast<WinUI.NavigationViewItem>();

            var invokedItem = invokedItems.FirstOrDefault(e => e.Content.Equals(obj: args.InvokedItem));

            switch (invokedItem?.Tag)
            {
                case PageTokens.GradesModuleGradesView:
                    ViewModel.RequestInternalNavigation(navigationPath: PageTokens.GradesModuleGradesView);
                    break;
                case "SampleViewToken":
                    ViewModel.RequestInternalNavigation(navigationPath: PageTokens.SampleViewToken);
                    break;
                case "LoginToken":
                    ViewModel.RequestExternalNavigation(navigationPath: PageTokens.LoginViewToken);
                    break;
                case "Profile":
                    ViewModel.RequestInternalNavigation(navigationPath: PageTokens.UserSummaryViewToken);
                    break;
                case PageTokens.CalendarSummaryView:
                    ViewModel.RequestInternalNavigation(navigationPath: PageTokens.CalendarSummaryView);
                    break;
                case PageTokens.UserSummaryViewToken:
                    ViewModel.RequestInternalNavigation(navigationPath: PageTokens.UserSummaryViewToken);
                    break;
            }
        }

        private void Nv_OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            ViewModel.RequestInternalNavigation("../");
        }
    }
}
