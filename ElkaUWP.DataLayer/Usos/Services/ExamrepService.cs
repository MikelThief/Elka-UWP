using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Anotar.NLog;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.DataLayer.Usos.Converters.Json;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Requests;
using Newtonsoft.Json;
using NLog;
using Prism.Ioc;

namespace ElkaUWP.DataLayer.Usos.Services
{
    public class ExamrepService
    {
        private readonly ExamrepUser2RequestWrapper _examrepUser2RequestWrapper;
        /// <inheritdoc />
        public ExamrepService(ExamrepUser2RequestWrapper examrepUser2RequestWrapper)
        {
            _examrepUser2RequestWrapper = examrepUser2RequestWrapper;
        }

        public async Task<Dictionary<string, Dictionary<string, List<ExamRepGradedSubject>>>> GetUserGradesPerSemesterAsync()
        {
            var requestString = _examrepUser2RequestWrapper.GetRequestString();
            Dictionary<string, Dictionary<string, List<ExamRepGradedSubject>>> result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<ExamRepGradedSubject>>>>(value: json, converters: new JsonSubjectPassTypeConverter());
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

            return result;
        }
    }
}
