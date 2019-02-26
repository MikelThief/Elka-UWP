using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
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

namespace ElkaUWP.DataLayer.Usos.Services
{
    class UserService : UsosServiceBase
    {
        public UserService(ILogger logger, IContainerExtension containerExtension) : base(logger, containerExtension)
        {
        }

        public async Task<List<UserInfoElement>> GetUserInformation()

        {
            var request = (Container.Resolve<UserRequestWrapper>());

            var InfoUri = request.GetRequestString();
            string responseForInfo;
            var webClient = new WebClient();

            var InfoList = new List<UserInfoElement>();

            try
            {
                responseForInfo = await webClient.DownloadStringTaskAsync(address: InfoUri);
                InfoList = JsonConvert.DeserializeObject<List<UserInfoElement>>(value: responseForInfo,
                   settings: new JsonSerializerSettings
                   {
                       
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

            return InfoList;

        }

    }
}
