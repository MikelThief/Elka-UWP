using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.Modularity.GradesModule.ViewModels;
using Prism.Mvvm;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.TreeGrid;
using SelectionChangedEventArgs = Windows.UI.Xaml.Controls.SelectionChangedEventArgs;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ElkaUWP.Modularity.GradesModule.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FinalGradesView : Page
    {
        private FinalGradesViewModel ViewModel => DataContext as FinalGradesViewModel;

        public FinalGradesView()
        {
            this.InitializeComponent();
            ViewModelLocator.SetAutowireViewModel(obj: this, value: true);
        }

        private async void GradesMasterDetailsView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedSubjectApproach = e.AddedItems[0] as SubjectApproach;

            if(ViewModel.SelectedSubjectApproach != null)
                ViewModel.GetPartialGradesContainer(subjectId: ViewModel.SelectedSubjectApproach.Id,
                    semesterLiteral: ViewModel.SelectedSubjectApproach.SemesterLiteral);
        }

        private void GradesMasterDetailsView_OnLoaded(object sender, RoutedEventArgs e)
        {
            GradesMasterDetailsView.MapDetails = MapDetails;
        }

        private object MapDetails(object arg)
        {
            return ViewModel.DetailPaneInitilizationTaskNotifier;
        }


    }
}
