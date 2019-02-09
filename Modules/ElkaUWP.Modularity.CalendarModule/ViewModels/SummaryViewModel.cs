using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Services;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.Modularity.CalendarModule.ViewModels
{
    public class SummaryViewModel : BindableBase, INavigationAware
    {
        private Uri _calFileHyperlink;
        private Uri _webCalFeedHyperlink;

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


        public SummaryViewModel(TimeTableService timeTableService)
        {
            _timeTableService = timeTableService;
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
    }
}
