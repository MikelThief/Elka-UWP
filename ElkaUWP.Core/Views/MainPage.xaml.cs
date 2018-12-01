using System;

using ElkaUWP.Core.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ElkaUWP.Core.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
