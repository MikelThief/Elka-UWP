using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Anotar.NLog;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.DataLayer.Usos.Converters;
using ElkaUWP.DataLayer.Usos.Converters.Json;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Requests;
using ElkaUWP.Infrastructure.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using Prism.Ioc;

namespace ElkaUWP.DataLayer.Usos.Services
{
    public class GradesService : UsosServiceBase
    {
        /// <inheritdoc />
        public GradesService(ILogger logger, IContainerExtension containerExtension) : base(logger, containerExtension)
        {

        }

        public async Task<Dictionary<string, List<CourseEdition>>> GetUserCoursesPerSemesterAsync()
        {
            var request = (Container.Resolve<UserCoursesPerSemesterRequestWrapper>());
            var requestString = request.GetRequestString();
            UserCoursesPerSemester result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<UserCoursesPerSemester>(value: json, converters: new JsonPassTypeBoolConverter());
            }
            catch (WebException wexc)
            {
                Logger.Fatal(exception: wexc, "Unable to start OAuth handshake");
                return null;
            }
            catch (JsonException jexc)
            {
                Logger.Warn(exception: jexc, "Unable to deserialize incoming data");
                return null;
            }

            // intentional, as API returns a json with just a single element 'course_editions'
            return result.CourseEditions;
        }

        public async Task<Dictionary<string, Dictionary<string, List<ExamRepGradedSubject>>>> GetUserGradesPerSemesterAsync()
        {
            var request = (Container.Resolve<GradedSubjectsPerSemesterRequestWrapper>());
            var requestString = request.GetRequestString();
            Dictionary<string, Dictionary<string, List<ExamRepGradedSubject>>> result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<ExamRepGradedSubject>>>>(value: json, converters: new JsonSubjectPassTypeConverter());
            }
            catch (WebException wexc)
            {
                Logger.Fatal(exception: wexc, "Unable to start OAuth handshake");
                return null;
            }
            catch (JsonException jexc)
            {
                Logger.Warn(exception: jexc, "Unable to deserialize incoming data");
                return null;
            }

            return result;
        }

        public async Task<List<Semester>> GetSemestersHistoryAsync()
        {
            var request = (Container.Resolve<SemestersHistoryRequestWrapper>());
            var requestString = request.GetRequestString();
            List<Semester> result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<List<Semester>>(value: json, converters: new UsosDateTimeConverter());
            }
            catch (WebException wexc)
            {
                LogTo.FatalException(message: "Unable to start OAuth handshake", exception: wexc);
                return null;
            }
            catch (JsonException jexc)
            {
                LogTo.FatalException(message: "Unable to deserialize incoming data", exception: jexc);
                return null;
            }

            return result;
        }

        public async Task<List<Semester>> GetSemestersHistoryAsync(DateTime maximumStartDate)
        {
            var request = (Container.Resolve<SemestersHistoryRequestWrapper>());
            var requestString = request.GetRequestString(maximumStartDate: maximumStartDate);
            List<Semester> result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<List<Semester>>(value: json, converters: new IsoDateTimeConverter());
            }
            catch (WebException wexc)
            {
                LogTo.FatalException(message: "Unable to start OAuth handshake", exception: wexc);
                return null;
            }
            catch (JsonException jexc)
            {
                LogTo.FatalException(message: "Unable to deserialize incoming data", exception: jexc);
                return null;
            }

            return result;
        }
        
        public async Task<Dictionary<string, Dictionary<string, GradesGradedSubject>>> GetUserGradedSemestersAsync()
        {
            var request = (Container.Resolve<UserGradesPerSemesterRequestWrapper>());
            var requestString = request.GetRequestString();
            Dictionary<string, Dictionary<string, GradesGradedSubject>> result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, GradesGradedSubject>>>(value: json, converters: new JsonTOrNBoolConverter());
            }
            catch (WebException wexc)
            {
                Logger.Fatal(exception: wexc, "Unable to start OAuth handshake");
                return null;
            }
            catch (JsonException jexc)
            {
                Logger.Warn(exception: jexc, "Unable to deserialize incoming data");
                return null;
            }
            var filteredResult = new Dictionary<string, Dictionary<string, GradesGradedSubject>>();

            foreach (var semester in result.Keys)
            {
                // filtering removes null semesters (undefined) and empty semesters (student didn't take part)
                if (result[key: semester] == null || result[key: semester]?.Values.Count < 1)
                    continue;
                // ArgumentException is never thrown as student cannot enroll more than once for same subject in the semester
                else filteredResult.Add(key: semester, value: result[key: semester]);
            }
            
            return filteredResult;
        }
    }
}
