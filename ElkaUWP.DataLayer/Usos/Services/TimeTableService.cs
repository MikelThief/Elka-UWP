using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Abstractions.Bases;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Requests;
using ElkaUWP.Infrastructure.Converters;
using ElkaUWP.Infrastructure.Services;
using Newtonsoft.Json;
using NLog;
using Prism.Ioc;

namespace ElkaUWP.DataLayer.Usos.Services
{
    public class TimeTableService : UsosServiceBase
    {
        /// <summary>
        /// Fetches activities for next two weeks for current student
        /// </summary>
        /// <returns>List with elements of type <see cref="ClassGroup2"/></returns>
        public async Task<List<ClassGroup2>> GetTimeTableActivitiesForStudentAsync()
        {
            var request = (Container.Resolve<StudentTimeTableRequestWrapper>());

            var firstDayOfCurrentWeek =  DateTime.Now.AddDays(value: -((int)DateTime.Now.DayOfWeek - (int)DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek));

            var currentWeekRequestUri = request.GetRequestString(startDate: firstDayOfCurrentWeek);
            var nextWeekRequestUri = request.GetRequestString(startDate: firstDayOfCurrentWeek.AddDays(value: 7));

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
                Logger.Fatal(exception: wexc, "Unable to start OAuth handshake");
                throw new InvalidOperationException();
            }
            catch (JsonException jexc)
            {
                Logger.Warn(exception: jexc, "Unable to deserialize incoming data");
                throw new InvalidOperationException();
            }

            // merge both lists creating a two-weeks list
            currentWeekListOfActivities.AddRange(collection: nextWeekListOfActivities);

            return currentWeekListOfActivities;
        }

        public string GetICalFileUri()
        {
            return Container.Resolve<UpcomingICalRequestWrapper>().GetRequestString();
        }

        public string GetWebCalFeedUri()
        {
            return Container.Resolve<UpcomingWebCalFeedRequestWrapper>().GetRequestString();
        }

        /// <inheritdoc />
        public TimeTableService(ILogger logger, IContainerExtension containerExtension) : base(logger: logger, containerExtension: containerExtension)
        {

        }
    }
}
