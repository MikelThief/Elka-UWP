using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Requests;
using Newtonsoft.Json;
using NLog;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Extensions;
using ElkaUWP.DataLayer.Usos.Converters.Json;

namespace ElkaUWP.DataLayer.Usos.Services
{
    public class UsersService: UsosServiceBase
    {
        public async Task<UserInfoContainer> User()

        {
            var request = (Container.Resolve<UserInfoRequestWrapper>());

            var requestString = request.GetRequestString();
            var webClient = new WebClient();
            UserInfoContainer result;

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);
                result = JsonConvert.DeserializeObject<UserInfoContainer>(value: json);
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

            return result;

        }

        public UsersService(ILogger logger, IContainerExtension containerExtension) : base(logger, containerExtension)
        {
        }
    }
}
