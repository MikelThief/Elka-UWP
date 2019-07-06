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
    public class TimetableService
    {
        private readonly TimetableStudentRequestWrapper _timetableStudentRequestWrapper;
        private readonly TimetableUpcomingICalRequestWrapper _timetableUpcomingICalRequestWrapper;
        private readonly TimetableUpcomingWebCalRequestWrapper _timetableUpcomingWebCalRequestWrapper;

        public TimetableService(TimetableStudentRequestWrapper timetableStudentRequestWrapper,
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
        /// <returns>List with elements of type <see cref="ClassGroup2"/> for the next 7 days.</returns>
        public async Task<List<ClassGroup2>> StudentAsync(DateTime date)
        {
            var currentWeekRequestUri = _timetableStudentRequestWrapper.GetRequestString(startDate: date);

            var webClient = new WebClient();

            List<ClassGroup2> activities;

            try
            {
                var response = await webClient.DownloadStringTaskAsync(address: currentWeekRequestUri);

                activities = JsonConvert.DeserializeObject<List<ClassGroup2>>(value: response,
                    settings: new JsonSerializerSettings
                    {
                        DateParseHandling = DateParseHandling.DateTime
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

            return activities;
        }

        public string UpcomingICalAsync()
        {
            return _timetableUpcomingICalRequestWrapper.GetRequestString();
        }

        public async Task<UpcomingShare> UpcomingShareAsync()
        {
            var webClient = new WebClient();
            var requestString = _timetableUpcomingWebCalRequestWrapper.GetRequestString();

            UpcomingShare response;
            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);
                response = JsonConvert.DeserializeObject<UpcomingShare>(value: json);
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

            return response;
        }
    }
}
