using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Anotar.NLog;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Requests;
using ElkaUWP.Infrastructure.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using Prism.Ioc;

namespace ElkaUWP.DataLayer.Usos.Services
{
    public class TermsService : UsosServiceBase
    {
        /// <inheritdoc />
        public TermsService(ILogger logger, IContainerExtension containerExtension) : base(logger, containerExtension)
        {

        }

        public async Task<List<Semester>> SearchAsync()
        {
            var request = (Container.Resolve<TermsSearchRequestWrapper>());
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

        public async Task<List<Semester>> SearchAsync(DateTime maximumStartDate)
        {
            var request = (Container.Resolve<TermsSearchRequestWrapper>());
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
    }
}
