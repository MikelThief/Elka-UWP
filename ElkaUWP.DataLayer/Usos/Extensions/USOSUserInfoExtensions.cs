using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.DataLayer.Usos.Extensions
{
    public static class USOSUserInfoExtensions
    {
        public static List<UserInfoElement> UserInfoElementList(this USOSUserInfo info)
        {
            var list = new List<UserInfoElement>();

            var firstName = new UserInfoElement
            {
                Header = nameof(info.FirstName)+"Key",
                Value = info.FirstName
            };
            list.Add(firstName);
            var lastName = new UserInfoElement
            {
                Header = nameof(info.LastName)+"Key",
                Value = info.LastName
            };
            list.Add(lastName);
            var email = new UserInfoElement
            {
                Header = nameof(info.Email) + "Key",
                Value = info.Email
            };
            list.Add(email);
            var sex = new UserInfoElement
            {
                Header = nameof(info.Sex) + "Key",
                Value = info.Sex
            };
            list.Add(sex);
            var studentNubmer = new UserInfoElement
            {
                Header = nameof(info.StudentNumber) + "Key",
                Value = info.StudentNumber
            };
            list.Add(studentNubmer);
            var USOSid = new UserInfoElement
            {
                Header = nameof(info.Id) + "Key",
                Value = info.Id.ToString()
            };
            list.Add(USOSid);
            var middleName= new UserInfoElement
            {
                Header = nameof(info.MiddleName) + "Key",
                Value = info.MiddleName
            };
            list.Add(middleName);
            var photoUri = new UserInfoElement
            {
                Header = nameof(info.PhotoUrls) + "Key",
                Value = info.PhotoUrls.PhotoUri.ToString()
            };
            list.Add(photoUri);
            var studentStatus = new UserInfoElement
            {
                Header = nameof(info.StudentStatus) + "Key",
                Value = info.StudentStatus.ToString()
            };
            list.Add(studentStatus);
            var pesel = new UserInfoElement
            {
                Header = nameof(info.Pesel) +"Key",
                Value = info.Pesel
            };
            list.Add(pesel);
          
         
            return list;

        }
    }
}
