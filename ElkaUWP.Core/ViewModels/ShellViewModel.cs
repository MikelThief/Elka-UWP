using System;
using System.Linq;
using System.Windows.Input;

using ElkaUWP.Core.Helpers;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WinUI = Microsoft.UI.Xaml.Controls;

namespace ElkaUWP.Core.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private static INavigationService _navigationService;
        private WinUI.NavigationView _navigationView;
        private bool _isBackEnabled;
        private WinUI.NavigationViewItem _selected;

        public ICommand ItemInvokedCommand { get; }

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set => SetProperty(storage: ref _isBackEnabled, value: value);
        }

        public WinUI.NavigationViewItem Selected
        {
            get { return _selected; }
            set => SetProperty(storage: ref _selected, value: value);
        }

        public ShellViewModel(INavigationService navigationServiceInstance)
        {
            _navigationService = navigationServiceInstance;
            ItemInvokedCommand = new DelegateCommand<WinUI.NavigationViewItemInvokedEventArgs>(executeMethod: OnItemInvoked);
        }

        public void Initialize(Frame frame, WinUI.NavigationView navigationView)
        {
            _navigationView = navigationView;
            frame.Navigated += Frame_Navigated;
            _navigationView.BackRequested += OnBackRequested;
        }

        private async void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            var item = _navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            var pageKey = item.GetValue(dp: NavHelper.NavigateToProperty) as string;
            await _navigationService.NavigateAsync(name: pageKey, null);
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = _navigationService.CanGoBack();
            Selected = _navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem: menuItem, sourcePageType: e.SourcePageType));
        }

        private async void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            await _navigationService.GoBackAsync();
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            var sourcePageKey = sourcePageType.Name;
            sourcePageKey = sourcePageKey.Substring(0, length: sourcePageKey.Length - 4);
            var pageKey = menuItem.GetValue(dp: NavHelper.NavigateToProperty) as string;
            return pageKey == sourcePageKey;
        }
    }
}
