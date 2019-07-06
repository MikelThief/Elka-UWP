using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.Modularity.CalendarModule.ViewModels;
using Prism.Mvvm;
using Syncfusion.UI.Xaml.Schedule;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ElkaUWP.Modularity.CalendarModule.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScheduleView : Page
    {
        private ScheduleViewModel ViewModel => DataContext as ScheduleViewModel;

        public ScheduleView()
        {
            this.InitializeComponent();
            ViewModelLocator.SetAutowireViewModel(obj: this, value: true);

            // Set up schedule
            Schedule.ShowNonWorkingHours = false;
        }

        private void CreateEventButton_Click(object sender, RoutedEventArgs e)
        {
            CreateDeadlineFlyout.Hide();
        }

        private void CurrentWeekSchedule_AppointmentEditorOpening(object sender, AppointmentEditorOpeningEventArgs e)
        {
            e.Cancel = true;
        }

        private async void Schedule_OnScheduleTapped(object sender, ScheduleTappedEventArgs e)
        {
            var properlyConvertedEvent = e.Appointment as CalendarEvent;

            await ViewModel.OpenCalendarEventDialog(startDateTime: e.SelectedDate.GetValueOrDefault(),
                appointment: (CalendarEvent) e.Appointment);
        }

        private async void CalendarGoBackwardButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.CurrentFirstDayOfWeekDate =
                ViewModel.CurrentFirstDayOfWeekDate.Subtract(value: TimeSpan.FromDays(value: 7));
            await ViewModel.DownloadScheduleFromUsosCommand.ExecuteAsync(null);
            Schedule.Backward();
        }

        private async void CalendarGoForwardButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.CurrentFirstDayOfWeekDate =
                ViewModel.CurrentFirstDayOfWeekDate.Add(value: TimeSpan.FromDays(value: 7));
            await ViewModel.DownloadScheduleFromUsosCommand.ExecuteAsync(null);
            Schedule.Forward();
        }
    }
}
