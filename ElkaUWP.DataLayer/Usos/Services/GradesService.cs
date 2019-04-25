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
        public async Task<Dictionary<string, Dictionary<string, GradesGradedSubject>>> Terms2Async()
        {
            var request = (Container.Resolve<GradesTerms2RequestWrapper>());
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
