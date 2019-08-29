using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using Windows.UI.Xaml.Controls;
using ElkaUWP.Infrastructure.Helpers;
using Microsoft.Toolkit.Uwp.Connectivity;
using RavinduL.LocalNotifications;
using RavinduL.LocalNotifications.Notifications;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using ElkaUWP.Infrastructure;
using Windows.UI;

namespace ElkaUWP.Modularity.MapsModule.ViewModels
{

    public class MapsViewModel : BindableBase, INavigationAware
    {
        public LocalNotificationManager NotificationManager { get; set; }
        private readonly ResourceLoader _resourceLoader =
         ResourceLoaderHelper.GetResourceLoaderForView(viewType: typeof(MapsModuleInitializer));

        public SolidColorBrush FloorUnder_background;
        public SolidColorBrush FloorZero_background;
        public SolidColorBrush FloorOne_background;
        public SolidColorBrush FloorTwo_background;
        public SolidColorBrush FloorThree_background;
        public SolidColorBrush FloorFour_background;
        public SolidColorBrush FloorFive_background;

        private Uri _img;
        public Uri Img { get => _img; private set => SetProperty(storage: ref _img, value: value, propertyName: nameof(Img)); }

        public string Source= "http://www.google.com";
       public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            Img = new Uri("ms-appx:///ElkaUWP.Modularity.MapsModule/Resources/FloorZero.png");
            ResetButtons();
        }

        public void ResetButtons()
        {
            FloorFive_background = new SolidColorBrush(Colors.Silver);
            FloorFour_background = new SolidColorBrush(Colors.Silver);
            FloorThree_background = new SolidColorBrush(Colors.Silver);
            FloorTwo_background = new SolidColorBrush(Colors.Silver);
            FloorOne_background = new SolidColorBrush(Colors.Silver);
            FloorZero_background = new SolidColorBrush(Colors.Silver);
            FloorUnder_background = new SolidColorBrush(Colors.Silver);

        }
        public Boolean InternetCheck()
        {
            if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                NotificationManager.Show(notification: new SimpleNotification
                {
                    TimeSpan = TimeSpan.FromSeconds(value: 4),
                    Text = _resourceLoader.GetString(resource: "No_Internet_Notification"),
                    Glyph = "\uF384",
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Background = new SolidColorBrush(color: Constants.RedColor)
                }
                );
                return false;
            }
            else
            {
                 return true;
            }
        }

        public void UnableToFind()
        {
            NotificationManager.Show(notification: new SimpleNotification
            {
                TimeSpan = TimeSpan.FromSeconds(value: 4),
                Text = _resourceLoader.GetString(resource: "Unable_to_find"),
                Glyph = "\uE721",
                VerticalAlignment = VerticalAlignment.Bottom,
                Background = new SolidColorBrush(color: Constants.RedColor)
            }
            );
        }


    }
}
