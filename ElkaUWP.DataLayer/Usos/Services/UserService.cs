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

namespace ElkaUWP.DataLayer.Usos.Services
{
    public class UserService : UsosServiceBase
    {
      // mrem
        public async Task<UserInfoElement> GetUserInformation()

        {
            var request = (Container.Resolve<UserInfoRequestWrapper>());

            var InfoUri = request.GetRequestString();
            string responseForInfo;
            var webClient = new WebClient();

            var InfoList = new UserInfoElement();

            try
            {
                responseForInfo = await webClient.DownloadStringTaskAsync(address: InfoUri);
                InfoList = JsonConvert.DeserializeObject<UserInfoElement>(value: responseForInfo);


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
       
        public UserService(ILogger logger, IContainerExtension containerExtension) : base(logger, containerExtension)
        {
        }


    }
}
