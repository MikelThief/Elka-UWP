using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.DataLayer.Usos.Converters;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Requests;
using Newtonsoft.Json;
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

            return result.CourseEditions;
        }

        public async Task<Dictionary<string, Dictionary<string, List<GradedSemester>>>> GetUserGradedSemestersAsync()
        {
            var request = (Container.Resolve<GradedSubjectsPerSemesterRequestWrapper>());
            var requestString = request.GetRequestString();
            Dictionary<string, Dictionary<string, List<GradedSemester>>> result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<GradedSemester>>>>(value: json, converters: new JsonSubjectPassTypeConverter());
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
    }
}
