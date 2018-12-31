using System;
using System.Collections.Generic;
using System.Linq;
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

        public Frame ContentViewFrame => this.ContentFrame;

        public ShellView()
        {
            InitializeComponent();
            ViewModelLocator.SetAutowireViewModel(obj: this, value: true);
        }

        private void nvSample_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            IEnumerable<NavigationViewItem> invokedItems = this.nvSample.MenuItems.Cast<NavigationViewItem>();
            
            NavigationViewItem invokedItem = invokedItems.FirstOrDefault(e => e.Content.Equals(args.InvokedItem));

            switch (invokedItem?.Tag)
            {
                case "TestViewToken":
                    (this.DataContext as ShellViewModel)._internalNavigationService.NavigateAsync(PageTokens.TestViewToken);
                    break;
                case "LoginToken":
                    (this.DataContext as ShellViewModel)._externalNavigationService.NavigateAsync(PageTokens.LoginViewToken);
                    break;
            }
        }

        private async void nvSample_Loaded(object sender, RoutedEventArgs e)
        {
            // NavigationService for internal frame
            ViewModel._internalNavigationService = Prism.Navigation.NavigationService.Create(frame: ContentViewFrame, Gesture.Back,
                Gesture.Forward, Gesture.Refresh);
        }
    }
}
