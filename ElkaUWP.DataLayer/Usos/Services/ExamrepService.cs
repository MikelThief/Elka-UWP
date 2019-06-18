using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.DataLayer.Usos.Converters.Json;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Requests;
using Newtonsoft.Json;
using NLog;
using Prism.Ioc;

namespace ElkaUWP.DataLayer.Usos.Services
{
    public class ExamrepService : UsosServiceBase
    {
        /// <inheritdoc />
        public ExamrepService(ILogger logger, IContainerExtension containerExtension) : base(logger, containerExtension)
        {
        }

        public async Task<Dictionary<string, Dictionary<string, List<ExamRepGradedSubject>>>> GetUserGradesPerSemesterAsync()
        {
            var request = (Container.Resolve<ExamrepUser2RequestWrapper>());
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
    }
}
