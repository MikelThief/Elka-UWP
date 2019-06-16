using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ElkaUWP.Infrastructure.Helpers;
using ElkaUWP.Modularity.LoginModule.ViewModels;
using RavinduL.LocalNotifications;
using RavinduL.LocalNotifications.Notifications;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ElkaUWP.Modularity.LoginModule.Views
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StudiaLoginView : Page
    {
        private StudiaLoginViewModel ViewModel => DataContext as StudiaLoginViewModel;

        private LocalNotificationManager localNotificationmanager;

        private readonly ResourceLoader _resourceLoader =
            ResourceLoaderHelper.GetResourceLoaderForView(viewType: typeof(LoginModuleInitializer));

        public StudiaLoginView()
        {
            this.InitializeComponent();
        }

        private void StudiaLoginView_OnLoaded(object sender, RoutedEventArgs e)
        {
            localNotificationmanager = new RavinduL.LocalNotifications.LocalNotificationManager(grid: NotificationGrid);
        }
    }
}
