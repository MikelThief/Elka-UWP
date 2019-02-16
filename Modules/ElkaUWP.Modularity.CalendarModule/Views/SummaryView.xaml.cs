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
using ElkaUWP.Modularity.CalendarModule.ViewModels;
using Prism.Mvvm;
using Syncfusion.UI.Xaml.Schedule;

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

            // Set up schedule
            CurrentWeekSchedule.TimeInterval = TimeInterval.OneHour;

            //CurrentWeekSchedule.CollapsedHours = new CollapsedHourCollection()
            //{
            //    new ScheduleCollapsedHour()
            //    {
            //        StartHour = 0,
            //        EndHour = 8
            //    },
            //    new ScheduleCollapsedHour()
            //    {
            //        StartHour = 21,
            //        EndHour = 24
            //    }
            //};

            CurrentWeekSchedule.ShowNonWorkingHours = false;
            CurrentWeekSchedule.WorkStartHour = 8;
            CurrentWeekSchedule.WorkEndHour = 20;
        }

        private void CreateEventButton_Click(object sender, RoutedEventArgs e)
        {
            CreateEventFlyout.Hide();
        }

        private void CurrentWeekSchedule_AppointmentEditorOpening(object sender, AppointmentEditorOpeningEventArgs e)
        {
            e.Cancel = true;

            ViewModel.OpenCalendarEventDialog(startDateTime: e.StartTime, appointment: (ScheduleAppointment) e.Appointment);
        }
    }
}
