using ElkaUWP.Modularity.MapsModule.ViewModels;
using Microsoft.Toolkit.Extensions;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Prism.Mvvm;
using RavinduL.LocalNotifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ElkaUWP.Modularity.MapsModule.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapsView : Page
    {
        private MapsViewModel ViewModel => DataContext as MapsViewModel;

        public MapsView()
        {
            this.InitializeComponent();
            ViewModelLocator.SetAutowireViewModel(obj: this, value: true);
        }



        private void NotificationGrid_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.NotificationManager = new LocalNotificationManager(grid: NotificationGrid);
        }

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainPivot.SelectedIndex == 1)
            {
                VirtualWalkRefreshButton.Visibility = Visibility.Visible;
                if (ViewModel.IsInternetAvailable())
                {
                    VirtualWalkWebview.Source = ViewModel.NavVisMapsUri;
                }
            }
            else
            {
                VirtualWalkRefreshButton.Visibility = Visibility.Collapsed;
            }
        }

        private void MapsArea_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (MapImage != null)
            {
                MapImage.Width = e.NewSize.Width;
                MapImage.Height = e.NewSize.Height;
            }
        }

        private void VirtualWalkRefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            if(ViewModel.IsInternetAvailable())
                VirtualWalkWebview.Refresh();
        }
    }
}
