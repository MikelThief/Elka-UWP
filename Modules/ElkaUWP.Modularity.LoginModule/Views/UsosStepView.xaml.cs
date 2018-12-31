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
    public partial class UsosStepView : Page
    {
        private UsosStepViewModel ViewModel => DataContext as UsosStepViewModel;

        private LocalNotificationManager localNotificationmanager;

        private readonly ResourceLoader _resourceLoader =
            ResourceLoaderHelper.GetResourceLoaderForView(loginViewType: typeof(LoginModuleInitializer));

        public UsosStepView()
        {
            this.InitializeComponent();
            ViewModelLocator.SetAutowireViewModel(obj: this, value: true);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            localNotificationmanager = new RavinduL.LocalNotifications.LocalNotificationManager(grid: NotificationGrid);

            if (ViewModel.IsContinueButtonVisible)
            {
                localNotificationmanager.Show(new SimpleNotification
                    {
                        TimeSpan = TimeSpan.FromSeconds(value: 30),
                        Text = _resourceLoader.GetString(resource: "Usos_Login_Success_InAppNotification"),
                        Glyph = "\uE8D7",
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Background = BrushFromColorHelper.GetSolidColorBrush(colorName: nameof(Colors.LightGreen))
                    }
                );
            }
        }
    }
}
