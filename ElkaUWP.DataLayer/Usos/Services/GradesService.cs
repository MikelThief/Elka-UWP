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
    public class GradesService
    {
        private readonly GradesTerms2RequestWrapper _gradesTerms2RequestWrapper;
        /// <inheritdoc />
        public GradesService(GradesTerms2RequestWrapper gradesTerms2RequestWrapper)
        {
            _gradesTerms2RequestWrapper = gradesTerms2RequestWrapper;
        }
        public async Task<Dictionary<string, Dictionary<string, GradesGradedSubject>>> Terms2Async()
        {
            var requestString = _gradesTerms2RequestWrapper.GetRequestString();
            Dictionary<string, Dictionary<string, GradesGradedSubject>> result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, GradesGradedSubject>>>(value: json, converters: new JsonTOrNBoolConverter());
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

            var filteredResult = new Dictionary<string, Dictionary<string, GradesGradedSubject>>();

            foreach (var semester in result.Keys)
            {
                // filtering removes null semesters (undefined) and empty semesters (no participation)
                if (result[key: semester] == null || result[key: semester]?.Values.Count < 1)
                    continue;
                // ArgumentException is never thrown as student cannot enroll more than once for same subject in the semester
                filteredResult.Add(key: semester, value: result[key: semester]);
            }

            return filteredResult;
        }
    }
}
