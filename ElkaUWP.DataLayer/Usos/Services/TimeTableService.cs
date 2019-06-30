using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Anotar.NLog;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Requests;
using ElkaUWP.Infrastructure.Converters;
using ElkaUWP.Infrastructure.Services;
using ElkaUWP.Infrastructure.Exceptions;
using ElkaUWP.Infrastructure.Helpers;
using Newtonsoft.Json;
using NLog;
using Prism.Ioc;

namespace ElkaUWP.DataLayer.Usos.Services
{
    public class TimeTableService
    {
        private readonly TimetableStudentRequestWrapper _timetableStudentRequestWrapper;
        private readonly TimetableUpcomingICalRequestWrapper _timetableUpcomingICalRequestWrapper;
        private readonly TimetableUpcomingWebCalRequestWrapper _timetableUpcomingWebCalRequestWrapper;

        public TimeTableService(TimetableStudentRequestWrapper timetableStudentRequestWrapper,
            TimetableUpcomingICalRequestWrapper timetableUpcomingICalRequestWrapper,
            TimetableUpcomingWebCalRequestWrapper timetableUpcomingWebCalRequestWrapper)
        {
            _timetableStudentRequestWrapper = timetableStudentRequestWrapper;
            _timetableUpcomingICalRequestWrapper = timetableUpcomingICalRequestWrapper;
            _timetableUpcomingWebCalRequestWrapper = timetableUpcomingWebCalRequestWrapper;
        }

        /// <summary>
        /// Fetches activities for next two weeks for current student
        /// </summary>
        /// <returns>List with elements of type <see cref="ClassGroup2"/></returns>
        public async Task<List<ClassGroup2>> GetTimeTableActivitiesForStudentAsync()
        {

            var firstDayOfCurrentWeekDateTime = DateTimeHelper.GetFirstDateOfWeek(dayInWeek: DateTime.Now, firstDay: DayOfWeek.Monday);

            var currentWeekRequestUri = _timetableStudentRequestWrapper.GetRequestString(startDate: firstDayOfCurrentWeekDateTime);
            var nextWeekRequestUri = _timetableStudentRequestWrapper.GetRequestString(startDate: firstDayOfCurrentWeekDateTime.AddDays(7));

            var webClient = new WebClient();

            string reponseForCurrentWeek;
            string responseForNextWeek;

            var currentWeekListOfActivities = new List<ClassGroup2>();
            var nextWeekListOfActivities = new List<ClassGroup2>();

            try
            {
                reponseForCurrentWeek = await webClient.DownloadStringTaskAsync(address: currentWeekRequestUri);

                currentWeekListOfActivities = JsonConvert.DeserializeObject<List<ClassGroup2>>(value: reponseForCurrentWeek,
                    settings: new JsonSerializerSettings
                    {
                        DateParseHandling = DateParseHandling.DateTime,
                        Converters = {new UsosDateTimeConverter()}
                    });

                responseForNextWeek = await webClient.DownloadStringTaskAsync(address: nextWeekRequestUri);

                nextWeekListOfActivities = JsonConvert.DeserializeObject<List<ClassGroup2>>(value: responseForNextWeek,
                    settings: new JsonSerializerSettings
                    {
                        DateParseHandling = DateParseHandling.DateTime,
                        Converters = { new UsosDateTimeConverter() }
                    });
            }
            catch (WebException wexc)
            {
                LogTo.FatalException(exception: wexc, message: "Unable to perform OAuth data exchange.");
                return null;
            }
            catch (JsonException jexc)
            {
                LogTo.WarnException(exception: jexc, message: "Unable to deserialize incoming data.");
                return null;
            }

            // merge both lists creating a two-weeks list
            currentWeekListOfActivities.AddRange(collection: nextWeekListOfActivities);

            return currentWeekListOfActivities;
        }

        public string GetICalFileUri()
        {
            return _timetableUpcomingICalRequestWrapper.GetRequestString();
        }

        public string GetWebCalFeedUri()
        {
            return _timetableUpcomingWebCalRequestWrapper.GetRequestString();
        }
    }
}
