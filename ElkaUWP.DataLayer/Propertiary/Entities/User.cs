using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Uri ProfileUri { get; set; }

        public User(int id, string firstName, string lastName, Uri profileUri)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            ProfileUri = profileUri;
        }
    }
}
