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
  
        private void FloorUnder_Click(object sender, RoutedEventArgs e)
        {
            ResetColors();
            FloorUnder.Background = new SolidColorBrush(Colors.DarkViolet);
            ImageEx img = map as ImageEx;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(img.BaseUri, "/ElkaUWP.Modularity.MapsModule/Resources/FloorUnder.jpg");
            img.Source = bitmapImage;
        }

        private void FloorZero_Click(object sender, RoutedEventArgs e)
        {
            ResetColors();
            FloorZero.Background = new SolidColorBrush(Colors.Red);
            ImageEx img = map as ImageEx;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(img.BaseUri, "/ElkaUWP.Modularity.MapsModule/Resources/FloorZero.png");
            img.Source = bitmapImage;
        }

        private void FloorOne_Click(object sender, RoutedEventArgs e)
        {
            ResetColors();
            FloorOne.Background = new SolidColorBrush(Colors.Orange);
            ImageEx img = map as ImageEx;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(img.BaseUri, "/ElkaUWP.Modularity.MapsModule/Resources/FloorOne.jpg");
            img.Source = bitmapImage;
        }

        private void FloorTwo_Click(object sender, RoutedEventArgs e)
        {
            ResetColors();
            FloorTwo.Background = new SolidColorBrush(Colors.Gold);
            ImageEx img = map as ImageEx;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(img.BaseUri, "/ElkaUWP.Modularity.MapsModule/Resources/FloorTwo.jpg");
            img.Source = bitmapImage;
        }

        private void FloorThree_Click(object sender, RoutedEventArgs e)
        {
            ResetColors();
            FloorThree.Background = new SolidColorBrush(Colors.LimeGreen);
            ImageEx img = map as ImageEx;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(img.BaseUri, "/ElkaUWP.Modularity.MapsModule/Resources/FloorThree.jpg");
            img.Source = bitmapImage;
        }

        private void FloorFour_Click(object sender, RoutedEventArgs e)
        {
            ResetColors();
            FloorFour.Background = new SolidColorBrush(Colors.DeepSkyBlue);
            ImageEx img = map as ImageEx;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(img.BaseUri, "/ElkaUWP.Modularity.MapsModule/Resources/FloorFour.jpg");
            img.Source = bitmapImage;
        }

        private void FloorFive_Click(object sender, RoutedEventArgs e)
        {
            ResetColors();
            FloorFive.Background = new SolidColorBrush(Colors.DarkBlue);

            ImageEx img = map as ImageEx;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(img.BaseUri, "/ElkaUWP.Modularity.MapsModule/Resources/FloorFive.jpg");
            img.Source = bitmapImage;
        }

        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            ImageEx img = map as ImageEx;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(img.BaseUri, "/ElkaUWP.Modularity.MapsModule/Resources/FloorZero.png");
            img.Source = bitmapImage;

        }

        private void ResetColors()
        {
            FloorOne.Background = new SolidColorBrush(Colors.Silver);
            FloorTwo.Background = new SolidColorBrush(Colors.Silver);
            FloorThree.Background = new SolidColorBrush(Colors.Silver);
            FloorFour.Background = new SolidColorBrush(Colors.Silver);
            FloorFive.Background = new SolidColorBrush(Colors.Silver);
            FloorUnder.Background = new SolidColorBrush(Colors.Silver);
            FloorZero.Background = new SolidColorBrush(Colors.Silver);
        }

        private void NotificationGrid_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.NotificationManager = new LocalNotificationManager(grid: NotificationGrid);
        }

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainPivot.SelectedIndex == 1)
            {
                if(ViewModel.InternetCheck())
                {
                   VirtualWalk.Source =new Uri( "http://mapy.ii.pw.edu.pl/iv.PW_WEITI/");
                }
               
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            String search = Search.Text;

            char[] charsToTrim = { 'a', 'b', 'c', 'd', 'e', 'A', 'B', 'C', 'D', 'E', ' ' };
            search = search.TrimEnd(charsToTrim);
            if (search.IsNumeric())
            {
                int room = Int32.Parse(search);
                if(room >0 && room<100)
                {
                    FloorZero_Click(this, new RoutedEventArgs());
                }
                if(room >=100 && room <200)
                {
                    FloorOne_Click(this, new RoutedEventArgs());
                }
                if (room >= 200 && room < 300)
                {
                    FloorTwo_Click(this, new RoutedEventArgs());
                }
                if (room >= 300 && room < 400)
                {
                    FloorThree_Click(this, new RoutedEventArgs());
                }
                if (room >= 400 && room < 500)
                {
                    FloorFour_Click(this, new RoutedEventArgs());
                }
                if (room >= 500 && room < 600)
                {
                    FloorFive_Click(this, new RoutedEventArgs());
                }
            }
            else if(search.StartsWith("0"))
                FloorUnder_Click(this, new RoutedEventArgs());
            else
            {
                ViewModel.UnableToFind();
            }
           
        }

        private void Search_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == VirtualKey.Enter)
            {
                OK_Click(this, new RoutedEventArgs());
            }

        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            Search.Text = "";
        }
    }
}
