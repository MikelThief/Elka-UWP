using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.UI;
using Windows.UI.Xaml.Media;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Helpers;
using ElkaUWP.Infrastructure.Helpers;
using Prism.Mvvm;
using Syncfusion.UI.Xaml.Schedule;

namespace ElkaUWP.Modularity.CalendarModule.ViewModels
{
    public class CalendarEventDialogViewModel : BindableBase
    {
        private readonly ResourceLoader _resourceLoader = ResourceLoaderHelper.GetResourceLoaderForView(loginViewType: typeof(CalendarModuleInitializer));

        private ScheduleAppointment appointment;

        private string _title;
        public bool IsPrimaryButtonEnabled => true;

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

        public Dictionary<CalendarEventType, string> CalendarEventTypeDictionary;

        public Dictionary<CalendarEventRecursionMode, string> CalendarEventRecursionModeDictionary;

        private CalendarEventType _selectedCalendarEventType;

        public CalendarEventType SelectedCalendarEventType
        {
            get => _selectedCalendarEventType;
            set => SetProperty(storage: ref _selectedCalendarEventType, value: value, propertyName: nameof(SelectedCalendarEventType));
        }

        private CalendarEventRecursionMode _selectedCalendarEventRecursionMode;

        public CalendarEventRecursionMode SelectedCalendarEventRecursionMode
        {
            get => _selectedCalendarEventRecursionMode;
            set => SetProperty(storage: ref _selectedCalendarEventRecursionMode, value: value, propertyName: nameof(SelectedCalendarEventRecursionMode));
        }


        public string TimeRange => EventStartDateTime?.ToString(format: "dd/MM/yyyy HH:mm")
                                   + " - "
                                   + EventEndDateTime?.ToString(format: "dd/MM/yyyy HH:mm");


        public CalendarEventDialogViewModel()
        {
            EventStartDateTime = DateTime.Now;
            EventEndDateTime = EventStartDateTime?.AddHours(value: 2);
        }

        public CalendarEventDialogViewModel(DateTime proposedStartTime)
        {
            EventStartDateTime = proposedStartTime;
            EventEndDateTime = EventStartDateTime?.AddHours(value: 2);

            CalendarEventTypeDictionary = new Dictionary<CalendarEventType, string>
            {
                {CalendarEventType.Lecture, _resourceLoader.GetString(resource: "LectureType") },
                {CalendarEventType.Tutorial, _resourceLoader.GetString(resource: "TutorialType")},
                {CalendarEventType.Laboratory, _resourceLoader.GetString(resource: "LaboratoryType")},
                {CalendarEventType.Seminar, _resourceLoader.GetString(resource: "SeminarType")},
                {CalendarEventType.Project, _resourceLoader.GetString(resource: "ProjectType")},
                {CalendarEventType.Conversatory, _resourceLoader.GetString(resource: "ConversatoryType")},
                {CalendarEventType.Other, _resourceLoader.GetString(resource: "OtherType")},
            };

            CalendarEventRecursionModeDictionary = new Dictionary<CalendarEventRecursionMode, string>
            {
                {CalendarEventRecursionMode.None, _resourceLoader.GetString(resource: "NoneRecursionMode") },
                {CalendarEventRecursionMode.EveryWeek, _resourceLoader.GetString(resource: "EveryWeekRecursionMode") },
                {CalendarEventRecursionMode.EveryTwoWeeks, _resourceLoader.GetString(resource: "EveryTwoWeeksRecursionMode") }
            };

            SelectedCalendarEventType = CalendarEventType.Other;
            SelectedCalendarEventRecursionMode = CalendarEventRecursionMode.None;
        }

        public CalendarEventDialogViewModel(ScheduleAppointment appointment)
        {
            Title = appointment.Subject;
            Notes = appointment.Notes;
            Location = appointment.Location;
            EventStartDateTime = appointment.StartTime;
            EventEndDateTime = appointment.EndTime;

            CalendarEventTypeDictionary = new Dictionary<CalendarEventType, string>
            {
                {CalendarEventType.Lecture, _resourceLoader.GetString(resource: "LectureType") },
                {CalendarEventType.Tutorial, _resourceLoader.GetString(resource: "TutorialType")},
                {CalendarEventType.Laboratory, _resourceLoader.GetString(resource: "LaboratoryType")},
                {CalendarEventType.Seminar, _resourceLoader.GetString(resource: "SeminarType")},
                {CalendarEventType.Project, _resourceLoader.GetString(resource: "ProjectType")},
                {CalendarEventType.Conversatory, _resourceLoader.GetString(resource: "ConversatoryType")},
                {CalendarEventType.Other, _resourceLoader.GetString(resource: "OtherType")},
            };

            CalendarEventRecursionModeDictionary = new Dictionary<CalendarEventRecursionMode, string>
            {
                {CalendarEventRecursionMode.None, _resourceLoader.GetString(resource: "NoneRecursionMode") },
                {CalendarEventRecursionMode.EveryWeek, _resourceLoader.GetString(resource: "EveryWeekRecursionMode") },
                {CalendarEventRecursionMode.EveryTwoWeeks, _resourceLoader.GetString(resource: "EveryTwoWeeksRecursionMode") }
            };

            SelectedCalendarEventType = CalendarEventType.Other;
            SelectedCalendarEventRecursionMode = CalendarEventRecursionMode.None;
        }

        public ScheduleAppointment GetScheduleAppointment()
        {
            var resultingAppointment = new ScheduleAppointment()
            {
                Subject = Title,
                Notes = CalendarEventTypeDictionary[key: SelectedCalendarEventType],
                Location = Location,
                // ReSharper disable once PossibleInvalidOperationException
                StartTime = EventStartDateTime.Value,
                // ReSharper disable once PossibleInvalidOperationException
                EndTime = EventEndDateTime.Value
            };

            if (SelectedCalendarEventRecursionMode != CalendarEventRecursionMode.None)
            {
                resultingAppointment.IsRecursive = true;
                var recurrenceProperties = new RecurrenceProperties();
                switch (SelectedCalendarEventRecursionMode)
                {
                    case CalendarEventRecursionMode.EveryTwoWeeks:
                        recurrenceProperties.IsDailyEveryNDays = true;
                        recurrenceProperties.DailyNDays = 14;
                        break;
                    case CalendarEventRecursionMode.EveryWeek:
                        recurrenceProperties.IsDailyEveryNDays = true;
                        recurrenceProperties.DailyNDays = 7;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                recurrenceProperties.RecurrenceRule = ScheduleHelper.RRuleGenerator(RecProp: recurrenceProperties, AppStartTime: resultingAppointment.StartTime, AppEndTime: resultingAppointment.EndTime);
                resultingAppointment.RecurrenceRule = recurrenceProperties.RecurrenceRule;
            }
            else
            {
                resultingAppointment.IsRecursive = false;
            }
            var brush = CalendarEventBackgroundHelper.GetBackgroundFromEventType(type: SelectedCalendarEventType);
            resultingAppointment.AppointmentBackground = brush;

            return resultingAppointment;
        }
    }
}
// TODO: ComboBox items doesn't initially show-up.
// Possible fix is the way of: var _enumval = Enum.GetValues(typeof(GetDetails)).Cast<GetDetails>();
