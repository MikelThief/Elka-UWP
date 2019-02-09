using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ElkaUWP.Modularity.CalendarModule.ViewModels;
using Prism.Mvvm;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ElkaUWP.Modularity.CalendarModule.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SummaryView : Page
    {
        private SummaryViewModel ViewModel => DataContext as SummaryViewModel;

        public SummaryView()
        {
            this.InitializeComponent();
            ViewModelLocator.SetAutowireViewModel(obj: this, value: true);
        }
    }
}
