using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Entities;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    public class StudentUser : User
    {
        public StudentStatus Status { get; set; }

        public string Email { get; set; }

        public string AltEmail { get; set; }

        /// <inheritdoc />
        public StudentUser(int id, string firstName, string lastName, Uri profileUri, StudentStatus status, string email, string altEmail) : base(id, firstName, lastName, profileUri)
        {
            Status = status;
            Email = email;
            AltEmail = altEmail;
        }
    }
}
