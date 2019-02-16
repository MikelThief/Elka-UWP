using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;
using Prism.Mvvm;
using Syncfusion.UI.Xaml.Schedule;

namespace ElkaUWP.Modularity.CalendarModule.ViewModels
{
    public class CalendarEventDialogViewModel : BindableBase
    {
        private ScheduleAppointment appointment;

        private string _title;

        private bool _isPrimaryButtonEnabled;

        public bool IsPrimaryButtonEnabled
        {
            get => _isPrimaryButtonEnabled;
            set => SetProperty(storage: ref _isPrimaryButtonEnabled, value: value, propertyName: nameof(IsPrimaryButtonEnabled));
        }

        private bool _isSecondaryButtonEnabled;

        public bool IsSecondaryButtonEnabled
        {
            get => _isSecondaryButtonEnabled;
            set => SetProperty(storage: ref _isSecondaryButtonEnabled, value: value, propertyName: nameof(IsSecondaryButtonEnabled));
        }

        public string Title
        {
            get => _title;
            set => SetProperty(storage: ref _title, value: value, propertyName: nameof(Title));
        }

        private string _location;

        public string Location
        {
            get => _location;
            set => SetProperty(storage: ref _location, value: value, propertyName: nameof(Location));
        }

        private string _notes;

        public string Notes
        {
            get => _notes;
            set => SetProperty(storage: ref _notes, value: value, propertyName: nameof(Notes));
        }
        private DateTime? _eventEndDateTime;
        private DateTime? _eventStartDateTime;

        public DateTime? EventEndDateTime
        {
            get => _eventEndDateTime;
            set
            {
                SetProperty(storage: ref _eventEndDateTime, value: value, propertyName: nameof(EventEndDateTime));
                RaisePropertyChanged(propertyName: nameof(TimeRange));
            }
        }

        public DateTime? EventStartDateTime
        {
            get => _eventStartDateTime;
            set
            {
                SetProperty(storage: ref _eventStartDateTime, value: value, propertyName: nameof(EventStartDateTime));
                RaisePropertyChanged(propertyName: nameof(TimeRange));
            }
        }


        public string TimeRange => EventStartDateTime?.ToString(format: "dd/MM/yyyy HH:mm")
                                   + " - "
                                   + EventEndDateTime?.ToString(format: "dd/MM/yyyy HH:mm");



        public CalendarEventDialogViewModel()
        {
            EventStartDateTime = DateTime.Now;
            EventEndDateTime = EventStartDateTime.Value.AddHours(value: 2);
        }

        public CalendarEventDialogViewModel(DateTime proposedStartTime)
        {
            EventStartDateTime = proposedStartTime;
            EventEndDateTime = EventStartDateTime?.AddHours(value: 2);
            IsPrimaryButtonEnabled = true;

            // cannot delete non-existing appointment
            IsSecondaryButtonEnabled = false;
        }

        public CalendarEventDialogViewModel(ScheduleAppointment appointment)
        {
            IsPrimaryButtonEnabled = true;
            IsSecondaryButtonEnabled = true;
            Title = appointment.Subject;
            Notes = appointment.Notes;
            Location = appointment.Location;
            EventStartDateTime = appointment.StartTime;
            EventEndDateTime = appointment.EndTime;
        }

        public ScheduleAppointment GetScheduleAppointment()
        {

            return new ScheduleAppointment()
            {
                Subject = Title,
                Notes = Notes,
                Location = Location,
                StartTime = EventStartDateTime.Value,
                EndTime = EventEndDateTime.Value
            };
        }
    }
}
