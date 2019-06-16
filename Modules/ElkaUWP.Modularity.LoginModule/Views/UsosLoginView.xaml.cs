using System;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ElkaUWP.Infrastructure.Helpers;
using ElkaUWP.Modularity.LoginModule.ViewModels;
using Prism.Mvvm;
using RavinduL.LocalNotifications;
using RavinduL.LocalNotifications.Notifications;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ElkaUWP.Modularity.LoginModule.Views
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class UsosLoginView : Page
    {
        private UsosLoginViewModel ViewModel => DataContext as UsosLoginViewModel;

        private readonly ResourceLoader _resourceLoader =
            ResourceLoaderHelper.GetResourceLoaderForView(viewType: typeof(LoginModuleInitializer));

        public UsosLoginView()
        {
            this.InitializeComponent();
            ViewModelLocator.SetAutowireViewModel(obj: this, value: true);
        }

        private void UsosLoginView_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.NotificationManager = new LocalNotificationManager(grid: NotificationGrid);
        }
    }
}
