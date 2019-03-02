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

        public async Task<UserCoursesPerSemester> GetUserCoursesPerSemesterAsync()
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
                throw new InvalidOperationException();
            }
            catch (JsonException jexc)
            {
                Logger.Warn(exception: jexc, "Unable to deserialize incoming data");
                throw new InvalidOperationException();
            }





            throw new NotImplementedException();
        }
    }
}
