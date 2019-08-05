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
using Anotar.NLog;
using CSharpFunctionalExtensions;
using ElkaUWP.DataLayer.Usos.Converters.Json;

namespace ElkaUWP.DataLayer.Usos.Services
{
    public class UsersService
    {
        private readonly UserInfoRequestWrapper _userInfoRequestWrapper;

        public UsersService(UserInfoRequestWrapper userInfoRequestWrapper)
        {
            _userInfoRequestWrapper = userInfoRequestWrapper;
        }

        public async Task<UserInfoContainer> User()
        {
            var requestString = _userInfoRequestWrapper.GetRequestString();
            var webClient = new WebClient();
            UserInfoContainer result;

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);
                result = JsonConvert.DeserializeObject<UserInfoContainer>(value: json);
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

        public async Task Search2Async()
        {


            return;
        }
    }
}
