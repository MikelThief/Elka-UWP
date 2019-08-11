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
using ElkaUWP.Infrastructure;

namespace ElkaUWP.DataLayer.Usos.Services
{
    public class UsersService
    {
        private readonly UserInfoRequestWrapper _userInfoRequestWrapper;
        private readonly UsersSearch2RequestWrapper _usersSearch2RequestWrapper;
        private readonly UsersUserRequestWrapper _usersUserRequestWrapper;

        public UsersService(UserInfoRequestWrapper userInfoRequestWrapper, UsersSearch2RequestWrapper usersSearch2RequestWrapper, UsersUserRequestWrapper usersUserRequestWrapper)
        {
            _userInfoRequestWrapper = userInfoRequestWrapper;
            _usersSearch2RequestWrapper = usersSearch2RequestWrapper;
            _usersUserRequestWrapper = usersUserRequestWrapper;
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

        public async Task<Result<Maybe<List<MatchedUserItem>>>> Search2Async(string query, UserCategory userCategory)
        {
            string lang = "pl";

            var requestString = _usersSearch2RequestWrapper.GetRequestString(query: query, userCategory: userCategory, lang: lang);
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);
                var result = JsonConvert.DeserializeObject<SearchedUserMatches>(value: json);

                return Result.Ok(value: result.Items.Count > 0 ?
                    Maybe<List<MatchedUserItem>>.From(obj: result.Items)
                    : Maybe<List<MatchedUserItem>>.None);
            }
            catch (WebException wexc)
            {
                LogTo.FatalException(exception: wexc, message: "Unable to perform handshake with USOS.");
                return Result.Fail<Maybe<List<MatchedUserItem>>>(error: ErrorCodes.USOS_HANDSHAKE_FAILED);
            }
            catch (JsonException jexc)
            {
                LogTo.WarnException(exception: jexc, message: "Unable to deserialize incoming data from USOS.");
                return Result.Fail<Maybe<List<MatchedUserItem>>>(error: ErrorCodes.USOS_BAD_DATA_RECEIVED);
            }
            finally
            {
                webClient.Dispose();
                webClient = null;
            }
        }

        public async Task<Result<User>> UserAsync(int userId, bool includeStudentProperties = false, bool includeStaffProperties = false)
        {
            var requestString = _usersUserRequestWrapper.GetRequestString(userId: userId,
                includeStaffProperties: includeStaffProperties,
                includeStudentProperties: includeStudentProperties);

            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);
                var result = JsonConvert.DeserializeObject<User>(value: json);

                return Result.Ok(value: result);

            }
            catch (WebException wexc)
            {
                LogTo.FatalException(exception: wexc, message: "Unable to perform handshake with USOS.");
                return Result.Fail<User> (error: ErrorCodes.USOS_HANDSHAKE_FAILED);
            }
            catch (JsonException jexc)
            {
                LogTo.WarnException(exception: jexc, message: "Unable to deserialize incoming data from USOS.");
                return Result.Fail<User>(error: ErrorCodes.USOS_BAD_DATA_RECEIVED);
            }
            finally
            {
                webClient.Dispose();
                webClient = null;
            }
        }
    }
}
