using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Anotar.NLog;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Propertiary.Services;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Services;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Helpers;
using LiteDB;
using MvvmDialogs;
using Nito.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Syncfusion.UI.Xaml.Schedule;
using TimetableService = ElkaUWP.DataLayer.Propertiary.Services.TimetableService;

namespace ElkaUWP.Modularity.CalendarModule.ViewModels
{
    public class ScheduleViewModel : BindableBase, INavigatedAware
    {
        private Uri _calFileHyperlink;
        private Uri _webCalFeedHyperlink;
        private readonly IDialogService _dialogService;

        private readonly ApplicationDataContainer _calendarModuleDataContainer =
            ApplicationData.Current.LocalSettings.CreateContainer(name: SettingsKeys
                .CalendarModuleContainerKey, disposition: ApplicationDataCreateDisposition.Always);

        private readonly TimetableService _timeTableService;

        private bool _isScheduleAutoDownloadEnabled;

        public bool IsScheduleAutoDownloadEnabled
        {
            get => _isScheduleAutoDownloadEnabled;
            set
            {
                SetProperty(storage: ref _isScheduleAutoDownloadEnabled, value: value,
                    propertyName: nameof(IsScheduleAutoDownloadEnabled));
                _calendarModuleDataContainer.Values[key: SettingsKeys.IsScheduleAutoDownloadEnabledSetting] = value;
            }
        }

        public Uri ICalFileHyperlink
        {
            get => _calFileHyperlink;
            private set => SetProperty(storage: ref _calFileHyperlink, value: value,
                propertyName: nameof(ICalFileHyperlink));
        }

        public Uri WebCalFeedHyperlink
        {
            get => _webCalFeedHyperlink;
            private set => SetProperty(storage: ref _webCalFeedHyperlink, value: value,
                propertyName: nameof(WebCalFeedHyperlink));
        }

        public ObservableCollection<UserDeadline> UserDeadlines = new ObservableCollection<UserDeadline>();

        public ObservableCollection<CalendarEvent> CalendarEvents { get; set; }

        public NotifyTask<Uri> WebCalUrlTaskNotifier { get; private set; }
        public NotifyTask CalendarEventsNotifier { get; private set; }

        private DateTime _currentFirstDayOfWeekDate;

        public DateTime CurrentFirstDayOfWeekDate
        {
            get => _currentFirstDayOfWeekDate;
            set
            {
                SetProperty(storage: ref _currentFirstDayOfWeekDate, value: value,
                    propertyName: nameof(CurrentFirstDayOfWeekDate));
                RaisePropertyChanged(propertyName: nameof(CurrentMonthAndYear));

            }
        }

        private int VisibleDays { get; set; }

        public string CurrentMonthAndYear
        {
            get => _currentFirstDayOfWeekDate.ToString(format: "MMMM");
        }


        #region CreateEventFlyout

        private DateTime _createDeadlineFlyoutDateTime;
        private string _createDeadlineFlyoutTitle;
        private string _createDeadlineFlyoutDescription;


        public DateTime CreateDeadlineFlyoutDateTime
        {
            get => _createDeadlineFlyoutDateTime;
            set => SetProperty(storage: ref _createDeadlineFlyoutDateTime, value: value,
                propertyName: nameof(CreateDeadlineFlyoutDateTime));
        }

        public string CreateDeadlineFlyOutTitle
        {
            get => _createDeadlineFlyoutTitle;
            set => SetProperty(storage: ref _createDeadlineFlyoutTitle, value: value,
                propertyName: nameof(CreateDeadlineFlyOutTitle));
        }

        public string CreateDeadlineFlyoutDescription
        {
            get => _createDeadlineFlyoutDescription;
            set => SetProperty(storage: ref _createDeadlineFlyoutDescription, value: value,
                propertyName: nameof(CreateDeadlineFlyoutDescription));
        }

        public DelegateCommand CreateEventCommand { get; private set; }
        public DelegateCommand<UserDeadline> RemoveUserDeadlineCommand { get; private set; }
        public AsyncCommand DownloadScheduleFromUsosCommand { get; private set; }

        #endregion

        public ScheduleViewModel(TimetableService timeTableService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _timeTableService = timeTableService;
            CreateEventCommand = new DelegateCommand(executeMethod: CreateNewDeadline);
            RemoveUserDeadlineCommand = new DelegateCommand<UserDeadline>(executeMethod: RemoveUserDeadline);
            DownloadScheduleFromUsosCommand = new AsyncCommand(executeAsync: DownloadScheduleFromUsosAsync);
            CreateDeadlineFlyoutDateTime = DateTime.Now;
            CreateDeadlineFlyOutTitle = string.Empty;
            CreateDeadlineFlyoutDescription = string.Empty;
            CalendarEvents = new ObservableCollection<CalendarEvent>();
            CurrentFirstDayOfWeekDate =
                DateTimeHelper.GetFirstDateOfWeek(dayInWeek: DateTime.Now, firstDay: DayOfWeek.Monday);
            VisibleDays = 7;

            WebCalUrlTaskNotifier = NotifyTask.Create(task: _timeTableService.GetWebCalFeedAsync());
            CalendarEventsNotifier = NotifyTask.Create(task: DownloadScheduleFromUsosAsync());

            if (_calendarModuleDataContainer.Values.ContainsKey(key: SettingsKeys.IsScheduleAutoDownloadEnabledSetting))
            {
                IsScheduleAutoDownloadEnabled =
                    (bool) _calendarModuleDataContainer.Values[key: SettingsKeys.IsScheduleAutoDownloadEnabledSetting];
            }
            else
            {
                IsScheduleAutoDownloadEnabled = true;
            }

            using (var db = new LiteRepository
                (connectionString: DatabaseConnectionStringHelper.GetCachedDatabaseConnectionString()))
            {
                foreach (var calendarEvent in db.Query<CalendarEvent>().ToList())
                {
                    CalendarEvents.Add(item: calendarEvent);
                }

                db.Delete<CalendarEvent>(predicate: x => x.EndTime < DateTime.Today.Subtract(TimeSpan.FromDays(60)));
            }

            using (var db = new LiteRepository
                    (connectionString: DatabaseConnectionStringHelper.GetRoamingDatabaseConnectionString()))
            {
                foreach (var userDeadline in db.Query<UserDeadline>().ToList())
                {
                    UserDeadlines.Add(item: userDeadline);
                }
            }
        }

        private async Task DownloadScheduleFromUsosAsync()
        {
            if (!IsScheduleAutoDownloadEnabled)
                return;

            var result = await _timeTableService.GetScheduleFromUsos(date: CurrentFirstDayOfWeekDate);
            var nextFirstDayOfWeekDate = CurrentFirstDayOfWeekDate.AddDays(value: VisibleDays);

            foreach (var calendarEvent in CalendarEvents.Where(x =>
                x.Origin == Origin.Usos
                && x.StartTime >= CurrentFirstDayOfWeekDate
                && x.EndTime <= nextFirstDayOfWeekDate).ToList())
            {
                CalendarEvents.Remove(item: calendarEvent);
            }

            foreach (var calendarEvent in result)
            {
                CalendarEvents.Add(item: calendarEvent);
            }

            using (var db = new LiteRepository(connectionString: DatabaseConnectionStringHelper.GetCachedDatabaseConnectionString()))
            {
                db.Delete<CalendarEvent>(x =>
                    x.Origin == Origin.Usos
                    && x.StartTime >= CurrentFirstDayOfWeekDate
                    && x.EndTime <= nextFirstDayOfWeekDate);
                db.Insert(entities: result);
            }
        }

        private void RemoveUserDeadline(UserDeadline obj)
        {
            using (var db = new LiteRepository
                (connectionString: DatabaseConnectionStringHelper.GetRoamingDatabaseConnectionString()))
            {
                db.Delete<UserDeadline>(x => x.Id == obj.Id);
            }

            UserDeadlines.Remove(item: obj);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public void CreateNewDeadline()
        {
            var deadline = new UserDeadline(date: CreateDeadlineFlyoutDateTime,
                header: CreateDeadlineFlyOutTitle, description: CreateDeadlineFlyoutDescription);

            UserDeadlines.Add(item: deadline);

            using (var db = new LiteRepository
                (connectionString: DatabaseConnectionStringHelper.GetRoamingDatabaseConnectionString()))
            {
                db.Insert(entity: deadline);
            }

            CreateDeadlineFlyoutDateTime = DateTime.Now;
            CreateDeadlineFlyOutTitle = string.Empty;
            CreateDeadlineFlyoutDescription = string.Empty;
        }

        public async Task OpenCalendarEventDialog(DateTime startDateTime, CalendarEvent appointment)
        {
            var vm = appointment is null
                ? new CalendarEventDialogViewModel(proposedStartTime: startDateTime)
                : new CalendarEventDialogViewModel(appointment: appointment);

            var result = await _dialogService.ShowContentDialogAsync(viewModel: vm);

            if (result != ContentDialogResult.Primary)
                return;

            if (appointment != null)
            {
                CalendarEvents.Remove(item: appointment);
            }

            var item = vm.GetResultingModel();
            CalendarEvents.Add(item: item);
        }


    }
}
