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
    class PostalAddressesService: UsosServiceBase
    {

        public async Task<IEnumerable<UserInfoElement>> GetUserInformation()

        {
            var request = (Container.Resolve<PostalAddressesWrapper>());

            var InfoUri = request.GetRequestString();
            string responseForInfo;
            var webClient = new WebClient();

            var addressList = new List<PostalAddressesType>();


            try
            {
                responseForInfo = await webClient.DownloadStringTaskAsync(address: InfoUri);
                var result = JsonConvert.DeserializeObject<PostalAddressesContainer>(value: responseForInfo, converters: JsonPostalAddressesConverter);
                
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



            return infoList;

        }

        public UserService(ILogger logger, IContainerExtension containerExtension) : base(logger, containerExtension)
        {
        }
    }
}
