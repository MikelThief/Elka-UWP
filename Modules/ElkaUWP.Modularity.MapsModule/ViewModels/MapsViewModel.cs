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

namespace ElkaUWP.Modularity.MapsModule.ViewModels
{

    public class MapsViewModel : BindableBase, INavigationAware
    {
        public LocalNotificationManager NotificationManager { get; set; }
        private readonly ResourceLoader _resourceLoader =
         ResourceLoaderHelper.GetResourceLoaderForView(viewType: typeof(MapsModuleInitializer));

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

            //throw new NotImplementedException();

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
