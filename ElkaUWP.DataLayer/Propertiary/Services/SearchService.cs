using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Requests;
using ElkaUWP.DataLayer.Usos.Services;

namespace ElkaUWP.DataLayer.Propertiary.Services
{
    public class SearchService
    {
        private readonly UsersService _usersService;

        public SearchService(UsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<Result<Maybe<IList<UserMatch>>>> SearchUsersAsync(string query)
        {
            var (isSuccess, _, value, error) = await _usersService.Search2Async(query: query, userCategory: UserCategory.CurrentStaff);
            var returningList = new List<UserMatch>();

            if (isSuccess && value.HasValue)
            {
                foreach (var matchedUserItem in value.Value)
                {
                    var title = (string.IsNullOrEmpty(value: matchedUserItem.User.Titles.After)) ? string.IsNullOrEmpty(value: matchedUserItem.User.Titles.Before) ?
                            string.Empty : matchedUserItem.User.Titles.Before : matchedUserItem.User.Titles.After;

                    var employmentPositionString = matchedUserItem.User.EmploymentPositions != null ?
                        string.Join(separator: ", ", values: matchedUserItem.User.EmploymentPositions) : string.Empty;

                    var userMatch = new UserMatch(
                        firstName: matchedUserItem.User.FirstName,
                        lastName: matchedUserItem.User.LastName,
                        title: title,
                        employmentPosition: employmentPositionString,
                        id: matchedUserItem.User.Id,
                        htmlMatchedName: matchedUserItem.Match);

                    returningList.Add(item: userMatch);
                }

                return Result.Ok(value: Maybe<IList<UserMatch>>.From(obj: returningList));
            }

            if(isSuccess && value.HasNoValue)
            {
                return Result.Ok(value: Maybe<IList<UserMatch>>.None);
            }

            return Result.Fail<Maybe<IList<UserMatch>>>(error: error);
        }
    }
}
