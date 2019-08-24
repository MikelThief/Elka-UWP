using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ElkaUWP.DataLayer.Usos.Extensions;
using ProprietaryEntities = ElkaUWP.DataLayer.Propertiary.Entities;
using UsosEntities = ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Services;

namespace ElkaUWP.DataLayer.Propertiary.Services
{
    public class UserService
    {
        private UsersService _usersService;
        private string _currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

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

            var employmentPositions = new List<string>();

            foreach (var employmentPositionEntry in value.EmploymentPositions)
            {
                var temp = employmentPositionEntry.Position.Name.GetValueForCurrentCulture()
                           + ", " + employmentPositionEntry.Faculty.Name.GetValueForCurrentCulture();

                employmentPositions.Add(item: temp);
            }

            var employmentPosition =
                string.Join(separator: "\n", values: employmentPositions);

            var phone = string.Join(separator: ", ", values: value.PhoneNumbers);

            var title = string.IsNullOrEmpty(value: value.Titles.After)
                ? string.IsNullOrEmpty(value: value.Titles.Before) ? string.Empty : value.Titles.Before
                : value.Titles.After;

            var officeHours = value.OfficeHours.GetValueForCurrentCulture();

            var staffUser = new ProprietaryEntities.StaffUser(
                id: value.Id, title: title,
                firstName: value.FirstName, lastName: value.LastName,
                phone: phone, profileUri: value.ProfileUrl,
                status: value.StaffStatus, employmentPosition: employmentPosition, officeHours: officeHours);

            return Result.Ok(value: staffUser);
        }
    }
}
