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

        public async Task<List<PostalAddresses>> GetUserInformation()

        {
            var request = (Container.Resolve<PostalAddressesWrapper>());

            var InfoUri = request.GetRequestString();
            string responseForInfo;

            var webClient = new WebClient();
            PostalAddressesContainer result;
            


            try
            {
                responseForInfo = await webClient.DownloadStringTaskAsync(address: InfoUri);
                result = JsonConvert.DeserializeObject<PostalAddressesContainer>(value: responseForInfo, converters: new JsonPostalAddressesConverter());
                
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



            return result.PostalAddresses;

        }

        public PostalAddressesService(ILogger logger, IContainerExtension containerExtension) : base(logger, containerExtension)
        {
        }
    }
}
