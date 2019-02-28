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
            ViewModelLocator.SetAutowireViewModel(obj: this, value: true);

            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = null;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = false;
        }

        private void Nv_Loaded(object sender, RoutedEventArgs e)
        {
            // NavigationService for internal frame
            ViewModel.Initialize(internalFrame: ContentFrame);
        }

        private void Nv_ItemInvoked(NavigationView navigationView, NavigationViewItemInvokedEventArgs args)
        {
            // check if item belongs to Nv
            var invokedItems = this.Nv.MenuItems.Cast<NavigationViewItem>();

            var invokedItem = invokedItems.FirstOrDefault(e => e.Content.Equals(obj: args.InvokedItem));

            switch (invokedItem?.Tag)
            {
                case PageTokens.GradesPerSemesterView:
                    ViewModel.RequestInternalNavigation(navigationPath: PageTokens.GradesPerSemesterView);
                    break;
                case "TestViewToken":
                    ViewModel.RequestInternalNavigation(navigationPath: PageTokens.TestViewToken);
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
            }
        }
    }
}
