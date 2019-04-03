using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Services;

namespace ElkaUWP.DataLayer.Propertiary
{
    public class UserService
    {
        private UsersService _usersService;

        public UserService(UsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<List<UserInfoElement>> GetAsync()
        {
            var dataContainer = await _usersService.User();

            // standard attributes
            var elements = new List<UserInfoElement>
            {
                new UserInfoElement {Header = "EmailKey", Value = dataContainer.Email},
                new UserInfoElement {Header = "FirstNameKey", Value = dataContainer.FirstName},
                new UserInfoElement {Header = "LastNameKey", Value = dataContainer.LastName},
                new UserInfoElement {Header = "MiddleNamesKey", Value = dataContainer.MiddleNames},
                new UserInfoElement {Header = "PeselKey", Value = dataContainer.Pesel},
                new UserInfoElement {Header = "IdKey", Value = dataContainer.Id.ToString()},
                new UserInfoElement {Header = "IndexNumberKey", Value = dataContainer.IndexNumber.ToString()},
                new UserInfoElement {Header = "SexKey", Value = dataContainer.Sex.ToString()},
                new UserInfoElement {Header = "PhotoUriKey", Value = dataContainer.PhotoUrls.PhotoUri.ToString()}
            };

            // postal address attribute
            foreach (var postalAddress in dataContainer.PostalAddresses)
            {
                switch (postalAddress.Type)
                {
                    case PostalAddressType.Primary:
                        elements.Add(item: new UserInfoElement
                        {
                            Header = "PrimaryAddressKey",
                            Value = postalAddress.Address
                        });
                        break;
                    case PostalAddressType.Correspondence:
                        elements.Add(item: new UserInfoElement
                        {
                            Header = "CorrespondenceAddressKey",
                            Value = postalAddress.Address
                        });
                        break;
                    case PostalAddressType.Residence:
                        elements.Add(item: new UserInfoElement
                        {
                            Header = "ResidenceAddressKey",
                            Value = postalAddress.Address
                        });
                        break;
                    case PostalAddressType.Other:
                        elements.Add(item: new UserInfoElement
                        {
                            Header = "OtherAddressKey",
                            Value = postalAddress.Address
                        });
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // student status attribute
            switch (dataContainer.StudentStatus)
            {
                case 0:
                    elements.Add(item: new UserInfoElement
                    {
                        Header = "StudentStatusKey",
                        Value = "StudentStatusValue0"
                    });
                    break;
                case 1:
                    elements.Add(item: new UserInfoElement
                    {
                        Header = "StudentStatusKey",
                        Value = "StudentStatusValue1"
                    });
                    break;
                case 2:
                    elements.Add(item: new UserInfoElement
                    {
                        Header = "StudentStatusKey",
                        Value = "StudentStatusValue2"
                    });
                    break;
                case null:
                    elements.Add(item: new UserInfoElement
                    {
                        Header = "StudentStatusKey",
                        Value = "StudentStatusValueNull"
                    });
                    break;
            }

            return elements;
        }
    }
}
