using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Entities;
using PropertyChanged;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    [AddINotifyPropertyChangedInterface]
    public class StaffUser : User
    {
        public string Title { get; set; }

        public string Phone { get; set; }

        public StaffStatus Status { get; set; }

        public string EmploymentPosition { get; set; }

        public StaffUser(int id, string title, string firstName, string lastName, string phone, Uri profileUri,
            StaffStatus status, string employmentPosition) : base(id: id,  firstName: firstName, lastName: lastName,  profileUri: profileUri)
        {
            Id = id;
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            ProfileUri = profileUri;
            Status = status;
            EmploymentPosition = employmentPosition;
        }
    }
}
