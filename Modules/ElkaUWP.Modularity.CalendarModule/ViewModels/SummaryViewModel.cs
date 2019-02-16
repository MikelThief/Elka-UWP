using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Anotar.NLog;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Services;
using MvvmDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Syncfusion.UI.Xaml.Schedule;

namespace ElkaUWP.Modularity.CalendarModule.ViewModels
{
    public class SummaryViewModel : BindableBase, INavigationAware
    {
        private Uri _calFileHyperlink;
        private Uri _webCalFeedHyperlink;
        private ObservableCollection<UserDeadline> _userDeadlines;
        private readonly IDialogService _dialogService;

        private TimeTableService _timeTableService;

        public Uri ICalFileHyperlink
        {
            get => _calFileHyperlink;
            private set => SetProperty(storage: ref _calFileHyperlink, value: value, propertyName: nameof(ICalFileHyperlink));
        }

        public Uri WebCalFeedHyperlink
        {
            get => _webCalFeedHyperlink;
            private set => SetProperty(storage: ref _webCalFeedHyperlink, value: value, propertyName: nameof(WebCalFeedHyperlink));
        }
        public ObservableCollection<UserDeadline> UserDeadlines
        {
            get => _userDeadlines;
            private set => _userDeadlines = value;
        }

        public ScheduleAppointmentCollection CalendarEvents { get; set; }


        #region CreateEventFlyout
        private DateTime _createDeadlineFlyoutDateTime;
        private string _createDeadlineFlyoutTitle;
        private string _createDeadlineFlyoutDescription;


        public DateTimeOffset CreateDeadlineFlyoutDateTime
        {
            get => _createDeadlineFlyoutDateTime;
            set => SetProperty(storage: ref _createDeadlineFlyoutDateTime, value: value.DateTime, propertyName: nameof(CreateDeadlineFlyoutDateTime));
        }



        public string CreateDeadlineFlyOutTitle
        {
            get => _createDeadlineFlyoutTitle;
            set => SetProperty(storage: ref _createDeadlineFlyoutTitle, value: value, propertyName: nameof(CreateDeadlineFlyOutTitle));
        }

        public string CreateDeadlineFlyoutDescription
        {
            get => _createDeadlineFlyoutDescription;
            set => SetProperty(storage: ref _createDeadlineFlyoutDescription, value: value,
                propertyName: nameof(CreateDeadlineFlyoutDescription));
        }

        public DelegateCommand CreateEventCommand { get; private set; }
        #endregion

        public SummaryViewModel(TimeTableService timeTableService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _timeTableService = timeTableService;
            CreateEventCommand = new DelegateCommand(executeMethod: CreateNewDeadline);
            CreateDeadlineFlyoutDateTime = DateTime.Now;
            CreateDeadlineFlyOutTitle = string.Empty;
            CreateDeadlineFlyoutDescription = string.Empty;
            CalendarEvents = new ScheduleAppointmentCollection();

            var someEvent = new UserDeadline(DateTime.Now, "ECRYP", "Project deadlline");

            UserDeadlines = new ObservableCollection<UserDeadline>();
            UserDeadlines.Add(someEvent);
            UserDeadlines.Add(someEvent);

        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {

            try
            {
                ICalFileHyperlink = new Uri(uriString: _timeTableService.GetICalFileUri());
            }
            catch (UriFormatException ufexc)
            {
                LogTo.WarnException(message: "Bad Uri string!\nStackTrace: " + ufexc.StackTrace + "\nException: ", exception: ufexc);
            }
            catch (NullReferenceException nrexc)
            {
                LogTo.WarnException(message: "No USOS user id specified. It should be obtained earlier!\nStackTrace: " + nrexc.StackTrace + "\nException: ", exception: nrexc);
            }

            try
            {
                WebCalFeedHyperlink = new Uri(uriString: _timeTableService.GetWebCalFeedUri());
            }
            catch (UriFormatException ufexc)
            {
                LogTo.WarnException(message: "Bad Uri string!\nStackTrace: " + ufexc.StackTrace + "\nException: ", exception: ufexc);
            }

        }

        public void CreateNewDeadline()
        {
            UserDeadlines.Add(item: new UserDeadline(date: CreateDeadlineFlyoutDateTime.DateTime, header: CreateDeadlineFlyOutTitle, description: CreateDeadlineFlyoutDescription));
            CreateDeadlineFlyoutDateTime = DateTime.Now;
            CreateDeadlineFlyOutTitle = string.Empty;
            CreateDeadlineFlyoutDescription = string.Empty;
        }

        public async void OpenCalendarEventDialog(DateTime startDateTime, ScheduleAppointment appointment)
        {
            CalendarEventDialogViewModel vm;

            if (appointment is null)
            {
                vm = new CalendarEventDialogViewModel(proposedStartTime: startDateTime);
            }
            else
            {
                vm = new CalendarEventDialogViewModel(appointment: appointment);
            }


            var result = await _dialogService.ShowContentDialogAsync(viewModel: vm);

            if (result == ContentDialogResult.Primary)
            {
                var item = vm.GetScheduleAppointment();
                CalendarEvents.Add(item: item);
            }

        }
    }
}
