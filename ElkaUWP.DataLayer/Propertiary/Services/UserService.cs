using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProprietaryEntities = ElkaUWP.DataLayer.Propertiary.Entities;
using UsosEntities = ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Services;

namespace ElkaUWP.DataLayer.Propertiary.Services
{
    public class UserService
    {
        private UsersService _usersService;

        public UserService(UsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<Result<ProprietaryEntities.StaffUser>> GetStaffUserAsync(int userId)
        {
            var (_, isFailure, value, error) = await _usersService.UserAsync(userId: userId,
                includeStudentProperties: false, includeStaffProperties: true);

            if (isFailure)
            {
                return Result.Fail<ProprietaryEntities.StaffUser>(error: error);
            }

            var employmentPosition =
                string.Join(separator: ", ", values: ((UsosEntities.User) value).EmploymentPositions);

            var phone = string.Join(separator: ", ", values: ((UsosEntities.User) value).PhoneNumbers);

            var title = string.IsNullOrEmpty(value: value.Titles.After)
                ? string.IsNullOrEmpty(value: value.Titles.Before) ? string.Empty : value.Titles.Before
                : value.Titles.After;


            var staffUser = new ProprietaryEntities.StaffUser(
                id: value.Id, title: title,
                firstName: value.FirstName, lastName: value.LastName,
                phone: phone, profileUri: value.ProfileUrl,
                status: value.StaffStatus, employmentPosition: employmentPosition);

            return Result.Ok(value: staffUser);
        }
    }
}
