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
using ElkaUWP.DataLayer.Propertiary.Entities;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ElkaUWP.Modularity.GradesModule.Views
{
    public sealed partial class PartialGradesView : UserControl
    {
        private PartialGradesContainer Model => DataContext as PartialGradesContainer;
        public PartialGradesView()
        {
            this.InitializeComponent();
        }
    }
}
