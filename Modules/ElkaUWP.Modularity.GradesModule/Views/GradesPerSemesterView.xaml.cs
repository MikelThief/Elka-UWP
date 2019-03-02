﻿using System;
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
using ElkaUWP.Modularity.GradesModule.ViewModels;
using Prism.Mvvm;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ElkaUWP.Modularity.GradesModule.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GradesPerSemesterView : Page
    {
        private GradesPerSemesterViewModel ViewModel => DataContext as GradesPerSemesterViewModel;

        public GradesPerSemesterView()
        {
            this.InitializeComponent();
            ViewModelLocator.SetAutowireViewModel(obj: this, value: true);
        }
    }
}