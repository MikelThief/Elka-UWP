using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.Modularity.CalendarModule.ViewModels
{
    public class SummaryViewModel : BindableBase, INavigationAware
    {
        private Uri _calFileHyperlink;
        private Uri _webCalFeedHyperlink;
        private ObservableCollection<UserCustomEvent> _userCustomEvents;

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
        public ObservableCollection<UserCustomEvent> UserCustomEvents
        {
            get => _userCustomEvents;
            private set => _userCustomEvents = value;
        }

        #region CreateEventFlyout
        private DateTime _createEventFlyoutDateTime;
        private string _createEventFlyoutTitle;
        private string _createEventFlyoutDescription;


        public DateTimeOffset CreateEventFlyoutDateTime
        {
            get => _createEventFlyoutDateTime;
            set => SetProperty(storage: ref _createEventFlyoutDateTime, value: value.DateTime, propertyName: nameof(CreateEventFlyoutDateTime));
        }



        public string CreateEventFlyOutTitle
        {
            get => _createEventFlyoutTitle;
            set => SetProperty(storage: ref _createEventFlyoutTitle, value: value, propertyName: nameof(CreateEventFlyOutTitle));
        }

        public string CreateEventFlyoutDescription
        {
            get => _createEventFlyoutDescription;
            set => SetProperty(storage: ref _createEventFlyoutDescription, value: value, propertyName: nameof(CreateEventFlyoutDescription));
        }

        public DelegateCommand CreateEventCommand { get; private set; }
        #endregion

        public SummaryViewModel(TimeTableService timeTableService)
        {
            _timeTableService = timeTableService;
            CreateEventCommand = new DelegateCommand(executeMethod: CreateNewEvent);
            CreateEventFlyoutDateTime = DateTime.Now;
            CreateEventFlyOutTitle = string.Empty;
            CreateEventFlyoutDescription = string.Empty;

            var someEvent = new UserCustomEvent(DateTime.Now, "ECRYP", "Project deadlline");

            UserCustomEvents = new ObservableCollection<UserCustomEvent>();
            UserCustomEvents.Add(someEvent);
            UserCustomEvents.Add(someEvent);

        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            // TODO: user_id has to be saved before acessing
            //ICalFileHyperlink = new Uri(uriString: _timeTableService.GetICalFileUri());
            WebCalFeedHyperlink = new Uri(uriString: _timeTableService.GetWebCalFeedUri());
        }

        public void CreateNewEvent()
        {
            UserCustomEvents.Add(item: new UserCustomEvent(date: CreateEventFlyoutDateTime.DateTime, header: CreateEventFlyOutTitle, description: CreateEventFlyoutDescription));
            CreateEventFlyoutDateTime = DateTime.Now;
            CreateEventFlyOutTitle = string.Empty;
            CreateEventFlyoutDescription = string.Empty;
        }
    }
}
